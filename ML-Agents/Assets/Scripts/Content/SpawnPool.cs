using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DronePoint
{
    public Transform Point;// { get; private set; }
    public bool IsExist;// { get; set; }

    public void Init(Transform point) 
    {
        Point = point;
    }
}

public class SpawnPool : MonoBehaviour
{
    Dictionary<Define.EnemyType, Transform> _roots = new Dictionary<Define.EnemyType, Transform>();
    Dictionary<int, DronePoint[]> _dronPoints = new Dictionary<int, DronePoint[]>();
    Transform[] _spawnPoints;
    Coroutine _coroutine;

    bool _init;
    float _minSpawnTime = 1f;
    float _maxSpawnTime = 3.5f;
    [SerializeField] float _currentTime;                     
    [SerializeField] float _maxTime;
    
    bool Init()
    {
        if (_init)
            return false;

        #region Initialize points
        Transform spawnPoints = transform.Find("SpawnPoints");
        _spawnPoints = new Transform[spawnPoints.childCount];

        for (int i = 0; i < spawnPoints.childCount; i++)
            _spawnPoints[i] = spawnPoints.GetChild(i);

        Transform frontDronPoints = Util.FindChild<Transform>(gameObject, "Front", true);
        _dronPoints.Add(0, new DronePoint[frontDronPoints.childCount]);

        for (int i = 0; i < frontDronPoints.childCount; i++)
        {
            DronePoint point = new DronePoint();
            point.Init(frontDronPoints.GetChild(i));
            _dronPoints[0][i] = point;
        }

        Transform backDronPoints = Util.FindChild<Transform>(gameObject, "Back", true);
        _dronPoints.Add(1, new DronePoint[backDronPoints.childCount]);

        for (int i = 0; i < backDronPoints.childCount; i++)
        {
            DronePoint point = new DronePoint();
            point.Init(backDronPoints.GetChild(i));
            _dronPoints[1][i] = point;
        }

        Transform meleeDronPoints = Util.FindChild<Transform>(gameObject, "Melee", true);
        _dronPoints.Add(2, new DronePoint[meleeDronPoints.childCount]);

        for (int i = 0; i < meleeDronPoints.childCount; i++)
        {
            DronePoint point = new DronePoint();
            point.Init(meleeDronPoints.GetChild(i));
            _dronPoints[2][i] = point;
        }

        GameObject meteorRoot = new GameObject("Meteor_Root");
        meteorRoot.transform.parent = transform;
        _roots.Add(Define.EnemyType.Meteor, meteorRoot.transform);

        GameObject dronRoot = new GameObject("Dron_Root");
        dronRoot.transform.parent = transform;
        _roots.Add(Define.EnemyType.Dron, dronRoot.transform);
        _init = true;
        #endregion
        return true;
    }

    private void Update()
    {
        GameManager.Instance.CurrentTime += Time.deltaTime;
    }

    public void OnStart()
    {
        Init();
        _currentTime = 0f;
        _maxTime = 60f;

        for (int y = 0; y < 2; y++)
        {
            for (int x = 0; x < _dronPoints[y].Length; x++)
                _dronPoints[y][x].IsExist = false;
        }

        if(_coroutine != null)
            StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(CoSpawning());
    }

    IEnumerator CoSpawning()
    {
        float spawnTime = 0f;
        float destTime = Random.Range(_minSpawnTime, _maxSpawnTime);

        while(_currentTime < _maxTime)
        {
            _currentTime += Time.deltaTime;
            spawnTime += Time.deltaTime;
            
            if (destTime <= spawnTime)
            {
                while(true)
                {
                    Define.EnemyType type = (Define.EnemyType)Random.Range(0, (int)Define.EnemyType.MaxCount);
                    bool result = OnSpawn(type);

                    if (result)
                        break;
                    
                    yield return null;
                }

                spawnTime = 0f;
                destTime = Random.Range(_minSpawnTime, _maxSpawnTime);
            }

            yield return null;
        }

        UIManager.Instance.ShowPopupUI<UI_BossAppear>();
    }

    public bool OnSpawn(Define.EnemyType type)
    {
        bool isSpawning = false;

        switch(type)
        {
            case Define.EnemyType.Boss:
                {
                    GameObject go = ObjectManager.Instance.Spawn(type);
                    go.transform.position = transform.position;
                    isSpawning = true;
                }
                break;
            case Define.EnemyType.Dron:
                {
                    Define.DroneType droneType = (Define.DroneType)Random.Range(0, (int)Define.DroneType.MaxCount);
                    GameObject dronOrigin = ResourceManager.Instance.Load<GameObject>($"Prefabs/Enemy/{droneType}");
                    bool isRanged = dronOrigin.GetComponent<DroneController>() is IRangedAttackDrone;
                    
                    if(isRanged)
                    {
                        List<DronePoint> points = new List<DronePoint>();

                        for (int i = 0; i < _dronPoints[0].Length; i++)
                        {
                            bool isFront = _dronPoints[0][i].IsExist;

                            if (isFront == false)
                                points.Add(_dronPoints[0][i]);
                        }

                        if (points.Count == 0)
                        {
                            for (int i = 0; i < _dronPoints[1].Length; i++)
                            {
                                bool isBack = _dronPoints[1][i].IsExist;

                                if (isBack == false)
                                    points.Add(_dronPoints[1][i]);
                            }
                        }

                        if (points.Count > 0)
                        {
                            int idx = Random.Range(0, points.Count);
                            Vector3 pos = points[idx].Point.position + Vector3.up * 20f;

                            DroneController dc = ObjectManager.Instance.DroneSpawn(dronOrigin);
                            dc.DroneType = droneType;
                            dc.transform.position = pos;
                            dc.SetInfo(droneType, points[idx].Point);                            
                            dc.Stat.OnDeadEventHandler += () => { points[idx].IsExist = false; };
                            dc.transform.parent = _roots[type];

                            points[idx].IsExist = true;
                            isSpawning = true;
                        }
                    }
                    else
                    {
                        int idx = Random.Range(0, _dronPoints[2].Length);
                        DronePoint point = _dronPoints[2][idx];
                        Vector3 pos = point.Point.position + Vector3.up * 20f;

                        DroneController dc = ObjectManager.Instance.DroneSpawn(dronOrigin);
                        dc.DroneType = droneType;
                        dc.transform.position = pos;
                        dc.SetInfo(droneType, point.Point);
                        dc.Stat.OnDeadEventHandler += () => { point.IsExist = false; };
                        dc.transform.parent = _roots[type];
                        isSpawning = true;
                    }
                }
                break;
            default:
                {
                    int idx = Random.Range(0, _spawnPoints.Length);
                    GameObject go = ObjectManager.Instance.Spawn(type);
                    go.transform.position = _spawnPoints[idx].position;
                    go.transform.parent = _roots[type];
                    isSpawning = true;
                }
                break;
        }

        return isSpawning;
    }
}