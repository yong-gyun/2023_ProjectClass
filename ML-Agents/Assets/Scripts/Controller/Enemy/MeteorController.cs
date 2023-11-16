using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorController : EnemyController
{
    GameObject _model;
    float _rotSpeed = 120f;

    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        _stat.SetStat(10f, 10f, 10f, 5f, 0f);
        _model = transform.Find("Model").gameObject;
        return true;
    }

    private void Update()
    {
        _model.transform.Rotate((Vector3.right + Vector3.up).normalized * _rotSpeed * Time.deltaTime, Space.Self);
        transform.position += Vector3.back * _stat.MoveSpeed * Time.deltaTime;    
    }

    protected override void OnCollisionAgent(Collider other)
    {
        base.OnCollisionAgent(other);
        ResourceManager.Instance.Destory(gameObject);
    }

    protected override void OnDead()
    {
        base.OnDead();
        GameObject go = ResourceManager.Instance.Instantiate("MeteorExplosion", transform.position, Quaternion.identity);
        ResourceManager.Instance.Destory(go, 1f);
    }
}