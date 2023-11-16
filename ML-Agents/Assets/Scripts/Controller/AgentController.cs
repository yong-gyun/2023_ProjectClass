using DG.Tweening;
using Google.Protobuf.WellKnownTypes;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class AgentController : Agent
{
    GameObject _model;
    Transform[] _firePoints;
    [SerializeField] bool _isAttackable;
    public AgentStat Stat { get { return _stat; } }
    public int UpgradeAttackCount;
    [SerializeField] float _rightLimitPos;
    [SerializeField] float _leftLimitPos;
    [SerializeField] SpawnPool _spawnPool;
    AgentStat _stat;
    Vector3 _camOriginPos;
    [SerializeField] Field _field;  
    [SerializeField] Vector3 _originPos;

    public override void Initialize()
    {
        _originPos = transform.position;
        _rightLimitPos = _originPos.x + Define.LIMITED_MOVE;
        _leftLimitPos = _originPos.x - Define.LIMITED_MOVE;
        _field = transform.root.GetComponent<Field>();

        _model = Util.FindChild(gameObject, "Model", true);
        _stat = GetComponent<AgentStat>();
        Transform firePoints = transform.Find("FirePoints");
        _firePoints = new Transform[firePoints.childCount];

        for (int i = 0; i < firePoints.childCount; i++)
            _firePoints[i] = firePoints.GetChild(i);

        _camOriginPos = Camera.main.transform.position;
        _stat.OnDeadEventHandler += OnDead;
        _stat.OnDamagedEventHandler += () =>
        {
            AddReward(-10f);
            //bool result = FindObjectOfType<UI_HitEffect>();
            //if(result == false)
            //    UIManager.Instance.ShowPopupUI<UI_HitEffect>();
            //Camera.main.DOShakePosition(0.5f, 2f).OnComplete(() => { Camera.main.transform.DOMove(_camOriginPos, 0.1f); });
        };
    }
            
    public override void OnEpisodeBegin()
    {
        transform.position = _originPos;
        transform.rotation = Quaternion.identity;
        _stat.SetStat(100f, 100f, 5f, 12f, 0.1f);
        Camera.main.transform.position = _camOriginPos;
        _field.SpawnPool.Clear();
        GameManager.Instance.Clear();
        UIManager.Instance.Clear();

        if (UIManager.Instance.SceneUI == null)
            UIManager.Instance.ShowSceneUI<UI_Game>();
        UpgradeAttackCount = 0;

        _field.SpawnPool.OnStart();
        _isAttackable = true;
        (UIManager.Instance.SceneUI as UI_Game).RefreshUI();
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        MoveAgent(actions.DiscreteActions); 
        OnAttacked(actions.DiscreteActions);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        float horizontal = Input.GetAxis("Horizontal");
        var discrete = actionsOut.DiscreteActions;
        if (horizontal > 0)
            discrete[0] = 2;
        else if(horizontal < 0)
            discrete[0] = 1;

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            discrete[1] = 1;
    }

    void MoveAgent(ActionSegment<int> segment)
    {
        int action = segment[0];
        Vector3 moveDir = Vector3.zero;

        switch (action)
        {
            case 0:
                moveDir = Vector3.zero;
                //_model.transform.DORotate(Vector3.zero, 0.5f);
                break;  
            case 1:
                moveDir = Vector3.left;
                //_model.transform.DORotate(Vector3.forward * 30f, 0.5f);
                //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, 0f, 15f), 15 * Time.deltaTime);
                break;
            case 2:
                moveDir = Vector3.right;
                //_model.transform.DORotate(Vector3.back * 30f, 0.5f);
                //ransform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, 0f, -15f), 15 * Time.deltaTime);
                break;
        }

        if (transform.position.x <= _leftLimitPos && moveDir.x < 0f || transform.position.x >= _rightLimitPos && moveDir.x > 0f)
        {
            AddReward(-0.1f);
            return;
        }
       
        _model.transform.DORotate((Vector3.forward * -moveDir.x) * 30f, 0.5f);
        transform.position += moveDir * _stat.MoveSpeed * Time.deltaTime;
    }

    void OnAttacked(ActionSegment<int> segment)
    {
        int action = segment[1];
        bool isAttackable = action == 1 && _isAttackable;

        if (isAttackable == false)
            return;

        StartCoroutine(CoAttacked());

        for (int i = 0; i < _firePoints.Length; i++)
        {
            Bullet bullet = _field.SpawnPool.CreateBullet("PlayerBullet", (go) => 
            {
                go.transform.position = _firePoints[i].position;
            });
            bullet.Init(_stat.Attack + (UpgradeAttackCount * 2.5f), true);
        }
    }

    IEnumerator CoAttacked()
    {
        _isAttackable = false;
        yield return new WaitForSeconds(_stat.AttackSpeed);
        _isAttackable = true;
    }

    void OnDead()
    {
        AddReward(-20);
        EndEpisode();
    }
}