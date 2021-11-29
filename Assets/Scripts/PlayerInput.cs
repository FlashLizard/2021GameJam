using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [Header("��λ/������")]
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
        //�洢���еİ��� 
        for (int x = 0; x < values.Length; x++)
        {
            if (Input.GetKeyDown((KeyCode)values.GetValue(x)))
            {
                Debug.Log(values.GetValue(x).ToString());
            }
        }
    }
}
