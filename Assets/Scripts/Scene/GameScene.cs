using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : Scene
{
    public override bool Check()
    {
        return true;
    }
    public override IEnumerator Load()
    {
        yield return null;
        Controller.current.FindUI(Data.UIId.settings);
        Controller.current.FindUI(Data.UIId.pause);
        Debug.Log("GameSceneLoaded");
    }
    public override void Exit()
    {
        base.Exit();
        List<int> temp=new List<int>();
        foreach(var i in ScoreControlUI.current.scoreUIs)
        {
            temp.Add(i.score);
        }
        Controller.current.SetScores(temp);
    }
}