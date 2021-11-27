using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryScene : Scene
{
    public override bool Check()
    {
        return true;
    }
    public override IEnumerator Load()
    {
        yield return null;
        Debug.Log("VictorySceneLoaded");
    }
}