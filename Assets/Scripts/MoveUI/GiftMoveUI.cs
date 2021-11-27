using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//�ȿ�HealthMoveUI���󲿷�����
public class GiftMoveUI : MoveUI//�ƶ�����UI
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
