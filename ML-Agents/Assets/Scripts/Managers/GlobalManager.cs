using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalManager<T> : MonoBehaviour where T : MonoBehaviour
{
    static T s_instance = null;

    public static T Instance
    {
        get
        {
            if(s_instance == null)
            {
                GameObject go = GameObject.Find($"@{typeof(T).Name}");

                if(go == null)
                {
                    //string name = typeof(T).Name;
                    //int idx = name.IndexOf("Manager");
                    //name = name.Substring(0, idx);
                    go = new GameObject($"@{typeof(T).Name}");
                    go.AddComponent<T>();
                }

                s_instance = go.GetComponent<T>();
                DontDestroyOnLoad(go);
            }

            return s_instance;
        }
    }
}
