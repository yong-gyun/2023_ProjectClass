using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
{
    public virtual float MaxHp { get { return _maxHp; } set { _maxHp = value; } }
    public virtual float Hp { get { return _hp; } set { _hp = value; } }
    public virtual float Attack { get { return _attack; } set { _attack = value; } }
    public virtual float MoveSpeed { get { return _moveSpeed; } set { _moveSpeed = value; } }
    public virtual float AttackSpeed { get { return _attackSpeed; } set { _attackSpeed = value; } }

    public Action OnDeadEventHandler = null;
    public Action OnDamagedEventHandler = null;

    [SerializeField] protected float _maxHp;
    [SerializeField] protected float _hp;
    [SerializeField] protected float _attack;
    [SerializeField] protected float _moveSpeed;
    [SerializeField] protected float _attackSpeed;

    public void SetStat(float maxHp, float hp, float attack, float moveSpeed, float attackSpeed)
    {
        _maxHp = maxHp;
        _hp = hp;
        _attack = attack;
        _moveSpeed = moveSpeed;
        _attackSpeed = attackSpeed;
    }

    public virtual void OnDamaged(float damage)
    {
        if(Hp <= 0f)
            return;

        Hp -= damage;

        if (OnDamagedEventHandler != null)
            OnDamagedEventHandler.Invoke();

        if (Hp <= 0f)
            OnDead();
    }

    public virtual void OnDead()
    {
        Hp = 0f;

        if (OnDeadEventHandler != null)
            OnDeadEventHandler.Invoke();
    }
}