using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUI : MonoBehaviour//可移动ui（头顶UI）父类
{
    protected float m_height=0.5f;//距离玩家偏移量
    public PlayerController pc;//玩家
    public static GameObject Generate(Data.UIId id,GameObject player)//生成一个MoveUI
    {
        MoveUI newUI = Data.Generate("UIs/"+Data.uIIdToString[id],player).GetComponent<MoveUI>();
        newUI.pc = player.GetComponent<PlayerController>();
        return newUI.gameObject;
    }
    public void Start()
    {
        transform.position = transform.position + new Vector3(0, m_height);//设置位置
    }
}
