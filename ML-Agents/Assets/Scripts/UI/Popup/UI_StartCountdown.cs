using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_StartCountdown : UI_Popup
{
    enum Texts
    {
        CountdownText
    }

    public Action OnStartEvent = null;

    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindText(typeof(Texts));
        StartCoroutine(CoCountdown());
        return true;
    }

    IEnumerator CoCountdown()
    {
        GetText((int)Texts.CountdownText).text = "3";
        yield return new WaitForSeconds(1f);
        GetText((int)Texts.CountdownText).text = "2";
        yield return new WaitForSeconds(1f);
        GetText((int)Texts.CountdownText).text = "1";
        yield return new WaitForSeconds(1f);
        GetText((int)Texts.CountdownText).text = "Start";
        yield return new WaitForSeconds(0.5f);

        OnStartEvent.Invoke();
        ClosePopupUI();
    }
}
