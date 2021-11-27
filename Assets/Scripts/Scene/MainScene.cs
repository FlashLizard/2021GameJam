using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : Scene
{
    public override bool Check()
    {
        return true;
    }
    public override IEnumerator Load()
    {
        yield return null;
        Controller.current.Reset();
        Controller.current.FindUI(Data.UIId.settings);
        Debug.Log("MainSceneLoaded");
    }
}