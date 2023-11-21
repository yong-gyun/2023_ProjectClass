using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : GlobalManager<GameManager>
{
    public int Score
    {
        get
        {
            return _score;
        }
        set
        {
            _score = value;
            (UIManager.Instance.SceneUI as UI_Game).SetScore();
        }
    }

    int _score;
    public float CurrentTime
    {
        get
        {
            return _currentTime;
        }
        set
        {
            _currentTime = value;
            (UIManager.Instance.SceneUI as UI_Game).SetTime();
        }
    }

    float _currentTime;

    public int CurrentStage
    {
        get
        {
            return _currentStage;
        }
        set
        {
            _currentStage = value;

            if (value == 2)
                ObjectManager.Instance.Agent.Stat.Hp = ObjectManager.Instance.Agent.Stat.MaxHp;
            (UIManager.Instance.SceneUI as UI_Game).RefreshUI();
        }
    }

    int _currentStage; 

    public void Clear()
    {
        _score = 0;
        _currentTime = 0f;
    }
}