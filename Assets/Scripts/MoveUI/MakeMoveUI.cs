using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//先看HealthMoveUI，大部分类似
public class MakeMoveUI : MoveUI//制作条UI
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
