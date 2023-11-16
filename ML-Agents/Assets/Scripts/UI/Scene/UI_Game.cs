using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Game : UI_Scene
{
    enum Texts
    {
        ScoreText,
        TimeText,
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

    public void RefreshUI()
    {
        Init();
        SetHp();
        SetTime();
        SetScore();
    }

    public void SetHp()
    {
        //AgentStat stat = ObjectManager.Instance.Agent.Stat;
        //GetScrollbar((int)Scrollbars.HPBar).size = stat.Hp / stat.MaxHp;
        //GetText((int)Texts.HPText).text = $"{stat.Hp} / {stat.MaxHp}";
    }

    public void SetScore()
    {
        int score = GameManager.Instance.Score;
        GetText((int)Texts.ScoreText).text = string.Format("{0:0000000000}", score);
    }

    public void SetTime()
    {
        int sec = (int)GameManager.Instance.CurrentTime % 60;
        int min = (int)GameManager.Instance.CurrentTime / 60;

        GetText((int)Texts.TimeText).text = string.Format("{0:00} : {1:00}", min, sec);
    }
}