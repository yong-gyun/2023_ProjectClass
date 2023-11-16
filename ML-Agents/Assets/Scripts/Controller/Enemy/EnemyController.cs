using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Stat Stat { get { return _stat; } }
    public Define.EnemyType Type;
    protected Stat _stat;
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
        _stat.OnDamagedEventHandler += () =>
        {
            ObjectManager.Instance.Agent.AddReward(1f);
        };

        _init = true;
        return true;
    }

    protected virtual void OnDead()
    {
        int rand = Random.Range(0, 3);

        if(rand == 0)
        {
            Define.ItemType type = (Define.ItemType)Random.Range(0, (int) Define.ItemType.MaxCount);
            ObjectManager.Instance.CreateItem(type).transform.position = transform.position;
        }

        Debug.Log($"Dead {name}");
        ObjectManager.Instance.Agent.AddReward(5f);
        GameManager.Instance.Score += 100;
        ResourceManager.Instance.Destory(gameObject);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Agent"))
            OnCollisionAgent(other);

        if(other.CompareTag("Wall"))
            ResourceManager.Instance.Destory(gameObject, callback: () => 
            {
                ObjectManager.Instance.Agent.AddReward(-10f); 
            });
    }

    protected virtual void OnCollisionAgent(Collider other)
    {
        AgentStat stat = other.gameObject.GetComponent<AgentStat>();
        stat.OnDamaged(_stat.Attack);
    }
}