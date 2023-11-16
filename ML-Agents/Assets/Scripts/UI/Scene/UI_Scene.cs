using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Scene : UI_Base
{
    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        UIManager.Instance.SetCanvas(gameObject, false);
        return true;
    }
}