using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : PlayerController
{
    public override void StartInit()
    {
        //base.StartInit();
        team = 1;
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
    public override bool fire
    {
        get => false;
    }
    public override bool putIce
    {
        get => false;
    }
    public override bool switchBall
    {
        get => false;
    }
    public override Vector3 attackDirection
    {
        get => new Vector3(0, 1);
    }
    public override bool meleeDown
    {
        get => false;
    }
}
