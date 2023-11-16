using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_HitEffect : UI_Popup
{
    enum Images
    {
        Image
    }

    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindImage(typeof(Images));
        GetImage((int)Images.Image).DOFade(0.5f, 0.25f).OnComplete(() => 
        {
            GetImage((int)Images.Image).DOFade(0f, 0.25f).OnComplete(ClosePopupUI);
        });
        return true;
    }
}
