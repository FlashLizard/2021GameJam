using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//先看HealthMoveUI，大部分类似
public class GiftMoveUI : MoveUI//移动礼物UI
{
    public void Awake()
    {
        m_height += 0.65f;
    }
    public static GiftMoveUI Generate(GameObject player)
    {
        return MoveUI.Generate(Data.UIId.giftMove, player).GetComponent<GiftMoveUI>();
    }
}
