using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : PlayerControl
{
    private void Start()
    {
        team = 1;//设置为和玩家不同队伍
    }
    public override bool makeDown
    {
        get => false;
    }
    public override bool makeUp
    {
        get => false;
    }
    public override float vx
    {
        get => 0;
    }
    public override float vy
    {
        get => 0;
    }
    public override bool putGiftUp
    {
        get => false;
    }
}
