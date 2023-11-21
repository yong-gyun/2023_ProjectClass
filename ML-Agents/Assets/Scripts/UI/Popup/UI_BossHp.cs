using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BossHp : UI_Popup
{
    enum Texts
    {
        HPText
    }

    enum Scrollbars
    {
        HPBar
    }

    protected override bool Init()
    {
        if (base.Init() == false)
            return false;
        BindText(typeof(Texts));
        BindScrollbar(typeof(Scrollbars));
        return true;
    }

    public void SetHP(float hp, float maxHp)
    {
        float value = hp / maxHp;
        GetScrollbar((int)Scrollbars.HPBar).size = value;
        GetText((int)Texts.HPText).text = $"{hp} / {maxHp}";
    }
}
