using Google.Protobuf.WellKnownTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Stat Stat { get { return _stat; } }
    public Define.EnemyType Type;
    protected Stat _stat;
    protected Field Field
    {
        get
        {
            if (_field == null)
                _field = transform.root.GetComponent<Field>();
            return _field;
        }
    }
    Field _field;

    bool _init;

    private void Awake()
    {
        Init();
    }

    protected virtual bool Init()
    {
        if (_init)
            return false;

        tag = "Enemy";
        _stat = GetComponent<Stat>();
        _stat.OnDeadEventHandler += OnDead;
        //_stat.OnDamagedEventHandler += () =>
        //{
        //    Field.Agent.AddReward(1f);
        //};

        _init = true;
        return true;
    }

    protected virtual void OnDead()
    {
        int rand = Random.Range(0, 3);

        if(rand == 0)
        {
            Define.ItemType type = (Define.ItemType)Random.Range(0, (int) Define.ItemType.MaxCount);
            Field.SpawnPool.CreateItem(type).transform.position = transform.position;
        }

        Debug.Log($"Dead {name}");
        Field.Agent.AddReward(5f);
        GameManager.Instance.Score += 100;
        ResourceManager.Instance.Destory(gameObject);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Agent"))
            OnCollisionAgent(other);

        if(other.CompareTag("Wall"))
        {
            Field.Agent.AddReward(-1f);
            ResourceManager.Instance.Destory(gameObject);
        }
    }

    protected virtual void OnCollisionAgent(Collider other)
    {
        AgentStat stat = other.gameObject.GetComponent<AgentStat>();
        stat.OnDamaged(_stat.Attack);
    }
}