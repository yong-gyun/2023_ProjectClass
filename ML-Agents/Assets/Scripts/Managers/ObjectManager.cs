using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : GlobalManager<ObjectManager>
{
    //public AgentController Agent
    //{
    //    get
    //    {
    //        if (_agent == null)
    //            _agent = GameObject.FindObjectOfType<AgentController>();

    //        return _agent;
    //    }
    //}

    //public SpawnPool SpawnPool
    //{
    //    get
    //    {
    //        SpawnPool spawnPool = GameObject.FindObjectOfType<SpawnPool>();
    //        return spawnPool;
    //    }
    //}

    //public List<GameObject> BulletPool
    //{
    //    get
    //    {
    //        _bulletPool.RemoveAll(x => x == null);
    //        return _bulletPool;
    //    }
    //}

    //AgentController _agent;
    
    //public GameObject Spawn(Define.EnemyType type)
    //{
    //    GameObject go = ResourceManager.Instance.Instantiate($"Enemy/{type}");
    //    _enemyPool.Add(go);

    //    EnemyController ec = go.GetComponent<EnemyController>();
    //    ec.Type = type;
    //    return go;
    //}

    //public DroneController DroneSpawn(GameObject go)
    //{
    //    DroneController dc = Instantiate(go).GetComponent<DroneController>();
    //    _enemyPool.Add(dc.gameObject);
    //    dc.Type = Define.EnemyType.Dron;
    //    dc.name = go.name;
    //    return dc;
    //}

    //public GameObject CreateItem(Define.ItemType type)
    //{
    //    GameObject go = ResourceManager.Instance.Instantiate($"Item/{type}Item");
    //    ItemController ic = Util.GetOrAddComponent<ItemController>(go);
    //    ic.SetType(type);
    //    _itemPool.Add(go);
    //    return go;
    //}

    //public void Clear()
    //{
    //    foreach (GameObject go in _enemyPool)
    //    {
    //        if(go != null)
    //            ResourceManager.Instance.Destory(go);
    //    }

    //    foreach (GameObject go in _bulletPool)
    //    {
    //        if (go != null)
    //            ResourceManager.Instance.Destory(go);
    //    }

    //    foreach (GameObject go in _itemPool)
    //    {
    //        if (go != null)
    //            ResourceManager.Instance.Destory(go);
    //    }

    //    _enemyPool.Clear();
    //    _bulletPool.Clear();
    //    _itemPool.Clear();
    //}
}