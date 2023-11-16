using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : GlobalManager<UIManager>
{
    public UI_Scene SceneUI;
    List<UI_Popup> _popupList = new List<UI_Popup>();
    int _order = 10;

    public void SetCanvas(GameObject go, bool sort)
    {
        if (go == null)
            return;
        Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
        canvas.overrideSorting = true;

        if (sort)
            canvas.sortingOrder = _order++;
        else
            canvas.sortingOrder = 0;
    }

    public T ShowPopupUI<T>(string name = null) where T : UI_Popup
    {
        if(string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = ResourceManager.Instance.Instantiate($"UI/Popup/{name}");

        if (go == null)
            return null;

        T popup = Util.GetOrAddComponent<T>(go);
        _popupList.Add(popup);
        return popup;
    }

    public T ShowSceneUI<T>(string name = null) where T : UI_Scene
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = ResourceManager.Instance.Instantiate($"UI/Scene/{name}");

        if (go == null)
            return null;

        T scene = Util.GetOrAddComponent<T>(go);
        SceneUI = scene;
        return scene;
    }

    public void ClosePopupUI(UI_Popup popup)
    {
        if (_popupList.Count == 0)
            return;
        
        _popupList.Remove(popup);
        ResourceManager.Instance.Destory(popup.gameObject);
    }

    public void CloseAllPopupUI()
    {
        while (_popupList.Count > 0)
            ClosePopupUI(_popupList[0]);
    }

    public void Clear()
    {
        CloseAllPopupUI();

        if(SceneUI != null)
        {
            ResourceManager.Instance.Destory(SceneUI.gameObject);
            SceneUI = null;
        }
    }
}
