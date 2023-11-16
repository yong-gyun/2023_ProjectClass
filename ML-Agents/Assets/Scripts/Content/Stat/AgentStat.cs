using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AgentStat : Stat
{
    public override float Hp 
    {
        get
        {
            return _hp;
        }
        set
        {
            _hp = value;
            (UIManager.Instance.SceneUI as UI_Game).SetHp();
        }
    }

    GameObject _shield;

    public override void OnDamaged(float damage)
    {
        if (Hp <= 0f || _shield != null)
            return;

        if (OnDamagedEventHandler != null)
            OnDamagedEventHandler.Invoke();

        Hp -= damage;

        if (Hp <= 0f)
            OnDead();
    }

    public void OnShield(float t)
    {
        ResourceManager.Instance.Destory(_shield);
        _shield = null;

        if(_shield == null)
            _shield = ResourceManager.Instance.Instantiate("Item/ShieldEffect", transform);
        _shield.transform.DORotate(Vector3.up * 120f, 1f).SetLoops(-1, LoopType.Incremental);
        ResourceManager.Instance.Destory(_shield, t);
    }
}
