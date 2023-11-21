using DG.Tweening;
using Google.Protobuf.WellKnownTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackDroneController : DroneController, IRangedAttackDrone
{
    List<Transform> _firePoints = new List<Transform>();
    float _currentTime = 0.25f;
    int _fireCount = 0;
    bool _isAttackable = true;

    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        Transform firePoints = Util.FindChild<Transform>(gameObject, "FirePoints", true);

        for (int i = 0; i < firePoints.childCount; i++)
            _firePoints.Add(firePoints.GetChild(i));
        return true;
    }

    void Update()
    {
        if (_isArrive == false)
            return;

        _currentTime -= Time.deltaTime;
        Vector3 dir = (transform.position - Field.Agent.transform.position).normalized;
        Quaternion qua = Quaternion.LookRotation(dir);
        transform.DORotateQuaternion(qua, 2f);

        if (_currentTime <= 0f)
        {
            if (_fireCount == 5)
            {
                _fireCount = 0;
                StartCoroutine(CoAttackWait());
            }
            else
            {
                if (_isAttackable == false)
                    return;

                OnAttacked();
                _currentTime = _stat.AttackSpeed;
                _fireCount++;
            }
        }
    }

    IEnumerator CoAttackWait()
    {
        _isAttackable = false;
        yield return new WaitForSeconds(2f);
        _isAttackable = true;
    }

    public void OnAttacked()
    {
        switch (DroneType)
        {
            case Define.DroneType.OneShotDrone:
                {
                    if(GameManager.Instance.CurrentStage == 1)
                    {
                        Bullet bullet = Field.SpawnPool.CreateBullet("DronBullet", (go) =>
                        {
                            go.transform.position = _firePoints[0].position;
                            go.transform.rotation = transform.rotation;
                        });

                        bullet.Init(_stat.Attack, false);
                    }
                    else
                    {
                        Bullet bullet = Field.SpawnPool.CreateBullet("DronBullet", (go) =>
                        {
                            go.transform.position = _firePoints[0].position;
                            go.transform.rotation = transform.rotation;
                        });

                        bullet.Init(_stat.Attack, false, Define.BulletType.Follow, t: 0.75f);
                    }
                }
                break;
            case Define.DroneType.DoubleShotDrone:
                {
                    for (int i = 0; i < _firePoints.Count; i++)
                    {
                        Bullet bullet = Field.SpawnPool.CreateBullet("DronBullet", (go) =>
                        {
                            go.transform.position = _firePoints[i].position;
                            go.transform.rotation = transform.rotation;
                        });

                        bullet.Init(_stat.Attack, false);
                    }
                }
                break;
        }
    }
}
