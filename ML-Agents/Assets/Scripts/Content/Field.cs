using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    public SpawnPool SpawnPool
    {
        get
        {
            if (_spawnPool == null)
                _spawnPool = Util.FindChild<SpawnPool>(gameObject, recursive: true);

            return _spawnPool;
        }
    }

    SpawnPool _spawnPool;

    public AgentController Agent
    {
        get
        {
            if (_agent == null)
                _agent = Util.FindChild<AgentController>(gameObject, recursive: true);

            return _agent;
        }
    }

    AgentController _agent;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
            Time.timeScale = 5f;
        else if(Input.GetKeyDown(KeyCode.J))
            Time.timeScale = 1f;
    }
}