using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BossHp : UI_Popup
{
    enum Scrollbars
    {
        HPBar
    }

    protected override bool Init()
    {
        if (base.Init() == false)
            return false;
        BindScrollbar(typeof(Scrollbars));
        return true;
    }

    public void SetHP(float value)
    {
        GetScrollbar((int)Scrollbars.HPBar).size = value;
    }
}
