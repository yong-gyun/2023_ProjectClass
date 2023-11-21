using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Clear : UI_Popup
{
    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        StartCoroutine(CoForWait());
        return true;
    }

    IEnumerator CoForWait()
    {
        yield return new WaitForSeconds(1.5f);
        ClosePopupUI();
    }

    public override void ClosePopupUI()
    {
        base.ClosePopupUI();

        if (GameManager.Instance.CurrentStage == 1)
        {
            GameManager.Instance.CurrentStage = 2;
            UIManager.Instance.ShowPopupUI<UI_StartCountdown>().OnStartEvent += () => { ObjectManager.Instance.SpawnPool.OnStart(); };
        }
        else
        {
            ObjectManager.Instance.Agent.EndEpisode();
        }
    }
}