using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeDroneController : DroneController
{
    private void Update()
    {
        transform.position += Vector3.back * _stat.MoveSpeed * Time.deltaTime;   
    }

    protected override void OnDead()
    {
        base.OnDead();

        if(DroneType == Define.DroneType.ExplosionDrone)
        {
            for(int i = 0; i < 360; i+=45) 
            {
                GameObject go = ResourceManager.Instance.Instantiate("ExplosionFragment", transform.position, Quaternion.Euler(0f, i, 0f));
                Bullet bullet = go.GetComponent<Bullet>();
                bullet.Init(_stat.Attack, false);
            }
        }
    }

    protected override void OnCollisionAgent(Collider other)
    {
        AgentStat stat = other.gameObject.GetComponent<AgentStat>();
        stat.OnDamaged(_stat.Attack);
        ResourceManager.Instance.Destory(gameObject);
    }
}