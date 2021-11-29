using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//�ȿ�HealthMoveUI���󲿷�����
public class MakeMoveUI : MoveUI//������UI
{
    private void Awake()
    {
        m_height += 0.95f;
    }
    public static MakeMoveUI Generate(GameObject player)
    {
        return MoveUI.Generate(Data.UIId.makeMove, player).GetComponent<MakeMoveUI>();
    }
}
