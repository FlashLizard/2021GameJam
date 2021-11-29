using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [Header("键位/轴设置")]
    public string horizontalAxis;
    public string verticalAxis;
    public string mouseXAxis;
    public string mouseYAxis;
    public string fireAxis;
    public KeyCode layIceBlock;
    public KeyCode switchIceBlock;
    public KeyCode makeSnowBall;
    public KeyCode putGift;
    // public KeyCode melee;

    void Update()
    {
        var values = Enum.GetValues(typeof(KeyCode));
        //存储所有的按键 
        for (int x = 0; x < values.Length; x++)
        {
            if (Input.GetKeyDown((KeyCode)values.GetValue(x)))
            {
                Debug.Log(values.GetValue(x).ToString());
            }
        }
    }
}
