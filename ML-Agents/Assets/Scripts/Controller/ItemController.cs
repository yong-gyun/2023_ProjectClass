using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    Define.ItemType _type;

    public void SetType(Define.ItemType type)
    {
        _type = type;
        tag = "Item";
        transform.DORotate(Vector3.up * 180f, 2f).SetLoops(-1, LoopType.Incremental);
        ResourceManager.Instance.Destory(gameObject, 7f);
    }

    private void Update()
    {
        transform.position += Vector3.back * 15f * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Agent"))
        {
            AgentController agent = ObjectManager.Instance.Agent;

            switch (_type)
            {
                case Define.ItemType.Heal:
                    {
                        agent.Stat.Hp += 10f;

                        if (agent.Stat.Hp > agent.Stat.MaxHp)
                            agent.Stat.Hp = agent.Stat.MaxHp;
                    }
                    break;
                case Define.ItemType.Attack:
                    {
                        if (agent.UpgradeAttackCount < 5)
                            agent.UpgradeAttackCount++;
                    }
                    break;
                case Define.ItemType.Shield:
                    {
                        agent.Stat.OnShield(4f);
                    }
                    break;
            }

            agent.AddReward(5f);
            ResourceManager.Instance.Destory(gameObject);
        }
    }
}
