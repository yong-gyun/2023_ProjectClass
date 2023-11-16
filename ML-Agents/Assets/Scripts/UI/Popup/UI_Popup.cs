using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Popup : UI_Base
{
    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        UIManager.Instance.SetCanvas(gameObject, true);
        return true;
    }

    public virtual void ClosePopupUI()
    {
        UIManager.Instance.ClosePopupUI(this);
    }
}