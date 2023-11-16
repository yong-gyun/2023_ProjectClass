using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Object = UnityEngine.Object;

public abstract class UI_Base : MonoBehaviour
{
    protected Dictionary<Type, Object[]> _objects = new Dictionary<Type, Object[]>();
    bool _init;

    private void Start()
    {
        Init();
    }

    protected virtual bool Init()
    {
        if (_init)
            return false;

        _init = true;
        return true;
    }

    protected void Bind<T>(Type type) where T : Object
    {
        string[] names = Enum.GetNames(type);
        Object[] objects = new Object[names.Length];
        _objects.Add(typeof(T), objects);

        for (int i = 0; i < names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))
                objects[i] = Util.FindChild(gameObject, names[i], true);
            else
                objects[i] = Util.FindChild<T>(gameObject, names[i], true);

            if (objects[i] == null)
                Debug.Log($"Bind faild {names[i]}");
        }
    }

    protected void BindText(Type type) { Bind<TMP_Text>(type); }
    protected void BindScrollbar(Type type) { Bind<Scrollbar>(type); }
    protected void BindImage(Type type) { Bind<Image>(type); }
    protected void BindButton(Type type) { Bind<Button>(type); }

    protected T Get<T>(int idx) where T : Object
    {
        if (_objects.TryGetValue(typeof(T), out Object[] objects))
            return objects[idx] as T;

        return null;
    }

    protected TMP_Text GetText(int idx) { return Get<TMP_Text>(idx); }
    protected Image GetImage(int idx) { return Get<Image>(idx); }
    protected Button GetButton(int idx) { return Get<Button>(idx); }
    protected Scrollbar GetScrollbar(int idx) { return Get<Scrollbar>(idx); }
}