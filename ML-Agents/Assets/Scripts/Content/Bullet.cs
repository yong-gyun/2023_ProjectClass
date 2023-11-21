using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bullet : MonoBehaviour
{
    Define.BulletType _type;
    [SerializeField] float _attack;
    [SerializeField] float _moveSpeed;
    bool _isAgent;
    bool _init;

    public bool Init(float attack, bool isAgent = false, Define.BulletType type = Define.BulletType.None, float t = 1.5f)
    {
        if (_init)
            return false;

        _isAgent = isAgent;
        _attack = attack;
        _type = type;

        switch (type)
        {
            case Define.BulletType.Bomb:
                StartCoroutine(CoForWait(t, () =>
                {
                    for (int i = 0; i <= 360; i+= 45)
                    {
                        Bullet bullet = ObjectManager.Instance.SpawnPool.CreateBullet("BossBullet", (go) =>
                        {
                            go.transform.position = transform.position;
                            go.transform.localScale *= 1.75f;
                            go.transform.rotation = Quaternion.Euler(0f, i, 0f);
                        });

                        bullet.Init(_attack, false);
                    }

                    ResourceManager.Instance.Destory(gameObject);
                }));
                break;
            case Define.BulletType.Follow:
                StartCoroutine(CoForWait(t, () =>
                {
                    Vector3 dir = (transform.position - ObjectManager.Instance.Agent.transform.position).normalized;
                    Quaternion qua = Quaternion.LookRotation(dir);
                    transform.DORotateQuaternion(qua, 0.5f);
                }));
                break;
        }

        if (_isAgent)
            _moveSpeed = 20f;
        else
            _moveSpeed = 25f;

        ResourceManager.Instance.Destory(gameObject, 8f);
        _init = true;
        return true;
    }


    private void Update()
    {
        if (_init == false)
            return;

        if (_isAgent)
            transform.position += transform.forward * _moveSpeed * Time.deltaTime;
        else
            transform.Translate(Vector3.back * _moveSpeed * Time.deltaTime, Space.Self);
    }

    IEnumerator CoForWait(float t, Action callback)
    {
        yield return new WaitForSeconds(t);
        callback.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_init == false)
            return;

        if (other.CompareTag("Wall"))
        {
            ResourceManager.Instance.Destory(gameObject);
            return;
        }

        if (other.CompareTag("Agent") && _isAgent == false || other.CompareTag("Enemy") && _isAgent == true)
        {
            Stat stat = other.GetComponent<Stat>();
            stat.OnDamaged(_attack);
            GameObject go = ResourceManager.Instance.Instantiate("BulletHit", transform.position, Quaternion.identity);
            ResourceManager.Instance.Destory(go, 1f);
            ResourceManager.Instance.Destory(gameObject);
        }
    }
}