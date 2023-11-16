using DG.Tweening;
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
        Vector3 dir = (transform.position - ObjectManager.Instance.Agent.transform.position).normalized;
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
                    GameObject go = ResourceManager.Instance.Instantiate("DronBullet", _firePoints[0].position, transform.rotation);
                    Bullet bullet = Util.GetOrAddComponent<Bullet>(go);
                    bullet.Init(_stat.Attack, false);
                }
                break;
            case Define.DroneType.DoubleShotDrone:
                {
                    for (int i = 0; i < _firePoints.Count; i++)
                    {
                        GameObject go = ResourceManager.Instance.Instantiate("DronBullet", _firePoints[i].position, transform.rotation);
                        Bullet bullet = Util.GetOrAddComponent<Bullet>(go);
                        bullet.Init(_stat.Attack, false);
                    }
                }
                break;
        }
    }
}