using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : EnemyController
{
    Transform[] _firePoints;
    Coroutine _coroutine;

    bool IsActioning
    {
        get
        {
            return _isActioning;
        }
        set
        {
            _isActioning = value;
            
            if(_isActioning == false)
            {
                WaitFor(() => { OnPattern(_currentIndex++ % _maxIndex); }, 3f);
            }
        }
    }

    bool _isActioning;
    
    int _currentIndex;
    int _maxIndex;

    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        Transform firePoints = transform.Find("FirePoints");
        _firePoints = new Transform[firePoints.childCount];

        for (int i = 0; i < _firePoints.Length; i++)
            _firePoints[i] = firePoints.GetChild(i);

        _currentIndex = 0;
        _maxIndex = 4;
        _stat.SetStat(500f, 500f, 5f, 0f, 0f);
        IsActioning = false;
        return true;
    }

    void OnPattern(int idx)
    {
        if (IsActioning)
            return;

        IsActioning = true;

        switch(idx)
        {
            case 0:
                StartCoroutine(CoFirstPattern());
                break;
            case 1:
                StartCoroutine(CoSecondPattern());
                break;
            case 2:
                StartCoroutine(CoThirdPattern());
                break;
            case 3:     
                StartCoroutine(CoForthPattern());
                break;
        }
    }

    void WaitFor(Action action, float t)
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(CoWaitFor(action, t));
    }

    IEnumerator CoWaitFor(Action action, float t)
    {
        yield return new WaitForSeconds(t);
        action.Invoke();
    }

    IEnumerator CoFirstPattern()
    {
        WaitForSeconds wfs = new WaitForSeconds(0.25f);

        for (int i = 0; i < 5; i++)
        {
            ResourceManager.Instance.Instantiate("BossBullet", _firePoints[0].position, Quaternion.identity).GetComponent<Bullet>().Init(_stat.Attack, false);
            ResourceManager.Instance.Instantiate("BossBullet", _firePoints[0].position, Quaternion.Euler(0f, 20f, 0f)).GetComponent<Bullet>().Init(_stat.Attack, false);
            ResourceManager.Instance.Instantiate("BossBullet", _firePoints[0].position, Quaternion.Euler(0f, -20f, 0f)).GetComponent<Bullet>().Init(_stat.Attack, false);
            ResourceManager.Instance.Instantiate("BossBullet", _firePoints[0].position, Quaternion.Euler(0f, 40f, 0f)).GetComponent<Bullet>().Init(_stat.Attack, false);
            ResourceManager.Instance.Instantiate("BossBullet", _firePoints[0].position, Quaternion.Euler(0f, -40f, 0f)).GetComponent<Bullet>().Init(_stat.Attack, false);
            yield return wfs;
        }

        IsActioning = false;
    }

    IEnumerator CoSecondPattern()
    {
        WaitForSeconds wfs = new WaitForSeconds(0.5f);

        for (int i = 0; i < 5; i++)
        {
            ResourceManager.Instance.Instantiate("BossBullet", _firePoints[1].position, Quaternion.identity).GetComponent<Bullet>().Init(_stat.Attack, false);
            ResourceManager.Instance.Instantiate("BossBullet", _firePoints[1].position, Quaternion.Euler(0f, 25f, 0f)).GetComponent<Bullet>().Init(_stat.Attack, false);
            //ResourceManager.Instance.Instantiate("BossBullet", _firePoints[1].position, Quaternion.Euler(0f, -15f, 0f)).GetComponent<Bullet>().Init(_stat.Attack, false);

            ResourceManager.Instance.Instantiate("BossBullet", _firePoints[2].position, Quaternion.identity).GetComponent<Bullet>().Init(_stat.Attack, false);
            //ResourceManager.Instance.Instantiate("BossBullet", _firePoints[2].position, Quaternion.Euler(0f, 15f, 0f)).GetComponent<Bullet>().Init(_stat.Attack, false);
            ResourceManager.Instance.Instantiate("BossBullet", _firePoints[2].position, Quaternion.Euler(0f, -25f, 0f)).GetComponent<Bullet>().Init(_stat.Attack, false);
            yield return wfs;
        }

        IsActioning = false;
    }

    IEnumerator CoThirdPattern()
    {
        WaitForSeconds wfs = new WaitForSeconds(0.1f);

        for (int i = 0; i < 2; i++)
        {
            for (int j = -90; j < 90; j += 30)
            {
                GameObject go = ResourceManager.Instance.Instantiate("BossBullet", _firePoints[0].position, Quaternion.Euler(0f, j, 0f));
                Bullet bullet = go.GetComponent<Bullet>();
                bullet.Init(_stat.Attack, false); 
                yield return wfs;
            }

            yield return new WaitForSeconds(0.25f);

            for (int j = 90; j > -90; j -= 30)
            {
                GameObject go = ResourceManager.Instance.Instantiate("BossBullet", _firePoints[0].position, Quaternion.Euler(0f, j, 0f));
                Bullet bullet = go.GetComponent<Bullet>();
                bullet.Init(_stat.Attack, false);
                yield return wfs;
            }
        }

        IsActioning = false;
    }

    IEnumerator CoForthPattern()
    {
        WaitForSeconds wfs = new WaitForSeconds(0.2f);

        for (int i = 0; i < 6; i++)
        {
            for (int j = -90; j < 90; j += 20)
            {
                GameObject go = ResourceManager.Instance.Instantiate("BossBullet", _firePoints[0].position, Quaternion.Euler(0f, j, 0f));
                Bullet bullet = go.GetComponent<Bullet>();
                bullet.Init(_stat.Attack, false);
            }

            yield return wfs;
        }

        IsActioning = false;
    }

    protected override void OnDead()
    {
        Debug.Log("Boss Die");
        ObjectManager.Instance.Agent.AddReward(50f);
        ResourceManager.Instance.Destory(gameObject);
        GameManager.Instance.Score += 500;
        ObjectManager.Instance.Agent.EndEpisode();
    }
}