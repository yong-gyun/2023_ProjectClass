using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
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
        _stat.SetStat(1500f * GameManager.Instance.CurrentStage, 1500f * GameManager.Instance.CurrentStage, 10f, 0f, 0f);
        UI_BossHp hpBar = UIManager.Instance.ShowPopupUI<UI_BossHp>();
        hpBar.SetHP(_stat.Hp, _stat.MaxHp);

        _stat.OnDamagedEventHandler += () => { hpBar.SetHP(_stat.Hp, _stat.MaxHp); };
        _stat.OnDeadEventHandler += () => { hpBar.ClosePopupUI(); };

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
            Field.SpawnPool.CreateBullet("BossBullet", (go) =>
            {
                go.transform.position = _firePoints[0].position;
                go.transform.rotation = Quaternion.identity;
                go.GetComponent<Bullet>().Init(_stat.Attack, false);
            });
            Field.SpawnPool.CreateBullet("BossBullet", (go) =>
            {
                go.transform.position = _firePoints[0].position;
                go.transform.rotation = Quaternion.Euler(0f, 20f, 0f);
                go.GetComponent<Bullet>().Init(_stat.Attack, false);
            });
            Field.SpawnPool.CreateBullet("BossBullet", (go) =>
            {
                go.transform.position = _firePoints[0].position;
                go.transform.rotation = Quaternion.Euler(0f, -20f, 0f);
                go.GetComponent<Bullet>().Init(_stat.Attack, false);
            });
            Field.SpawnPool.CreateBullet("BossBullet", (go) =>
            {
                go.transform.position = _firePoints[0].position;
                go.transform.rotation = Quaternion.Euler(0f, 40f, 0f);
                go.GetComponent<Bullet>().Init(_stat.Attack, false);
            });
            Field.SpawnPool.CreateBullet("BossBullet", (go) =>
            {
                go.transform.position = _firePoints[0].position;
                go.transform.rotation = Quaternion.Euler(0f, -40f, 0f);
                go.GetComponent<Bullet>().Init(_stat.Attack, false);
            });

            yield return wfs;
        }

        IsActioning = false;
    }

    IEnumerator CoSecondPattern()
    {
        WaitForSeconds wfs = new WaitForSeconds(0.5f);

        if(GameManager.Instance.CurrentStage == 1)
        {
            for (int i = 0; i < 5; i++)
            {
                Field.SpawnPool.CreateBullet("BossBullet", (go) =>
                {
                    go.transform.position = _firePoints[1].position;
                    go.transform.rotation = Quaternion.Euler(0f, 25f, 0f);
                    go.GetComponent<Bullet>().Init(_stat.Attack, false);
                });

                Field.SpawnPool.CreateBullet("BossBullet", (go) =>
                {
                    go.transform.position = _firePoints[1].position;
                    go.transform.rotation = Quaternion.identity;
                    go.GetComponent<Bullet>().Init(_stat.Attack, false);
                });

                Field.SpawnPool.CreateBullet("BossBullet", (go) =>
                {
                    go.transform.position = _firePoints[2].position;
                    go.transform.rotation = Quaternion.identity;
                    go.GetComponent<Bullet>().Init(_stat.Attack, false);
                });

                Field.SpawnPool.CreateBullet("BossBullet", (go) =>
                {
                    go.transform.position = _firePoints[2].position;
                    go.transform.rotation = Quaternion.Euler(0f, -25f, 0f);
                    go.GetComponent<Bullet>().Init(_stat.Attack, false);
                });

                yield return wfs;
            }
        }
        else
        {
            {
                Bullet bullet = Field.SpawnPool.CreateBullet("BossBullet", (go) =>
                {
                    go.transform.position = _firePoints[0].position;
                    go.transform.rotation = transform.rotation;
                });


                bullet.Init(_stat.Attack, type: Define.BulletType.Bomb);
                yield return new WaitForSeconds(0.35f);
            }
            {
                Bullet bullet = Field.SpawnPool.CreateBullet("BossBullet", (go) =>
                {
                    go.transform.position = _firePoints[1].position;
                    go.transform.rotation = transform.rotation;
                    go.transform.Rotate(Vector3.up * 30f);
                });
                bullet.Init(_stat.Attack, type: Define.BulletType.Bomb, t: 1f);
            }
            {
                Bullet bullet = Field.SpawnPool.CreateBullet("BossBullet", (go) =>
                {
                    go.transform.position = _firePoints[2].position;
                    go.transform.rotation = transform.rotation;
                    go.transform.Rotate(Vector3.up * -30f);
                });
                bullet.Init(_stat.Attack, type: Define.BulletType.Bomb, t: 1f);
            }

            yield return new WaitForSeconds(1f);
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
                Field.SpawnPool.CreateBullet("BossBullet", (go) =>
                {
                    go.transform.position = _firePoints[0].position;
                    go.transform.rotation = Quaternion.Euler(0f, j, 0f);
                    go.GetComponent<Bullet>().Init(_stat.Attack, false);
                });
                yield return wfs;
            }

            yield return new WaitForSeconds(0.25f);

            for (int j = 90; j > -90; j -= 30)
            {
                Field.SpawnPool.CreateBullet("BossBullet", (go) =>
                {
                    go.transform.position = _firePoints[0].position;
                    go.transform.rotation = Quaternion.Euler(0f, j, 0f);
                    go.GetComponent<Bullet>().Init(_stat.Attack, false);
                });
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
                Field.SpawnPool.CreateBullet("BossBullet", (go) =>
                {
                    go.transform.position = _firePoints[0].position;
                    go.transform.rotation = Quaternion.Euler(0f, j, 0f);
                    go.GetComponent<Bullet>().Init(_stat.Attack, false);
                });
            }

            yield return wfs;
        }

        IsActioning = false;
    }

    protected override void OnDead()
    {
        Debug.Log("Boss Die");
        AgentController agent = transform.root.GetComponent<Field>().Agent;
        agent.AddReward(30f);

        GameObject go = ResourceManager.Instance.Instantiate("DroneExplosion", transform.position, Quaternion.identity);
        go.transform.localScale *= 20f;
        ResourceManager.Instance.Destory(go, 1f);

        StartCoroutine(CoWait());
        GameManager.Instance.Score += 500;
    }

    IEnumerator CoWait()
    {
        yield return new WaitForSeconds(1f);
        UIManager.Instance.ShowPopupUI<UI_Clear>();
        ResourceManager.Instance.Destory(gameObject);
    }
}