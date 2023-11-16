using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BossAppear : UI_Popup
{
    enum Images
    {
        Image
    }

    Sequence _sequnce;

    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindImage(typeof(Images));
        _sequnce = DOTween.Sequence();
        _sequnce.Append(GetImage((int)Images.Image).DOFade(0.5f, 0.75f))
        .Append(GetImage((int)Images.Image).DOFade(0.2f, 0.75f))
        .Append(GetImage((int)Images.Image).DOFade(0.5f, 0.75f))
        .Append(GetImage((int)Images.Image).DOFade(0.2f, 0.75f))
        .Append(GetImage((int)Images.Image).DOFade(0.5f, 0.75f))
        .Append(GetImage((int)Images.Image).DOFade(0f, 0.75f))
        .OnComplete(() =>
        {
            ObjectManager.Instance.Clear();
            ObjectManager.Instance.SpawnPool.OnSpawn(Define.EnemyType.Boss);
            ClosePopupUI();
        });
        return true;
    }
}
