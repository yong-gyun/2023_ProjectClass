using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class ResourceManager : GlobalManager<ResourceManager>
{
    Dictionary<string, Object> _resources = new Dictionary<string, Object>();

    public T Load<T>(string path) where T : Object
    {
        Object origin = null;

        if (_resources.TryGetValue(path, out origin))
            return origin as T;
        else
            origin = Resources.Load<T>(path);

        if (origin == null)
        {
            Debug.Log($"Faild load {path}");
            return null;
        }

        _resources.Add(path, origin);
        return origin as T;
    }

    public GameObject Instantiate(string path, Transform parent = null, Action<GameObject> callback = null)
    {
        GameObject origin = Load<GameObject>($"Prefabs/{path}");

        if(origin == null)
            return null;

        GameObject go = GameObject.Instantiate(origin, parent);

        if (go == null)
            return null;
        go.name = origin.name;

        if(callback != null)
            callback.Invoke(go);

        return go;
    }

    public GameObject Instantiate(string path, Vector3 pos, Quaternion rot, Transform parent = null, Action<GameObject> callback = null)
    {
        GameObject go = Instantiate(path, parent);

        if (go == null)
            return null;

        go.transform.position = pos;
        go.transform.rotation = rot;
        return go;
    }

    public void Destory(GameObject go, float t = 0f, Action callback = null)
    {
        if (go == null)
            return;

        if(callback != null)
            StartCoroutine(CoWaitForDestoryEvent(t, callback));
        GameObject.Destroy(go, t);
    }

    IEnumerator CoWaitForDestoryEvent(float t, Action evt)
    {
        yield return new WaitForSeconds(t);
        evt.Invoke();
    }
}