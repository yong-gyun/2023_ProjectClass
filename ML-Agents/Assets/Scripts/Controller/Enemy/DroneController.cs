using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

interface IRangedAttackDrone
{
    void OnAttacked();
}

public class DroneController : EnemyController
{
    public Define.DroneType DroneType;
    protected bool _isArrive = false;

    public void SetInfo(Define.DroneType type, Transform point)
    {
        Collider col = GetComponent<Collider>();
        col.enabled = false;
        DroneType = type;
        transform.DOMove(point.position, 1.25f).SetEase(Ease.OutQuad).OnComplete(() => 
        {
            col.enabled = true;
            _isArrive = true; 
        });
    }

    protected override bool Init()
    {
        if(base.Init() == false)
            return false;

        _stat.SetStat(10f, 10f, 7.5f, 5f, 1.2f);
        return true;
    }

    protected override void OnDead()
    {
        base.OnDead();
        GameObject go = ResourceManager.Instance.Instantiate("DroneExplosion", transform.position, Quaternion.identity);
        ResourceManager.Instance.Destory(go, 1f);
    }
}
