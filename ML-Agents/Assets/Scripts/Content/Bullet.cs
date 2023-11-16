using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float _damage;
    [SerializeField] float _moveSpeed;
    bool _isAgent;
    bool _init;

    public bool Init(float damage, bool isAgent)
    {
        if (_init)
            return false;

        _isAgent = isAgent;
        _damage = damage;

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
            stat.OnDamaged(_damage);
            GameObject go = ResourceManager.Instance.Instantiate("BulletHit", transform.position, Quaternion.identity);
            ResourceManager.Instance.Destory(go, 1f);
            ResourceManager.Instance.Destory(gameObject);
        }
    }
}