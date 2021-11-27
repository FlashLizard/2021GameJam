using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowballController : MonoBehaviour
{
    [Header("属性")]
    public float shotSpeed;         // 抛出速度
    public float g;                 // 重力加速度

    private float time;             // 起点到终点的时间
    private float passTime;         // 经过的时间
    private Vector3 attackPoint;    // 终点
    private Vector3 speed;          // 初速度向量
    private Vector3 Gravity;        // 重力向量


    void Awake()
    {
        Gravity = Vector3.zero;
        passTime = float.MaxValue;

    }

    void Update()
    {
        if (passTime < time)
        {
            Gravity.y = g * (passTime += Time.deltaTime);              // v=gt
            transform.position += (speed + Gravity) * Time.deltaTime;  //模拟位移
        }
    }

    public void Shoot()
    {
        passTime = 0f;
    }

    /// <summary>
    /// 设置攻击点
    /// </summary>
    public void SetAttackPoint(Vector3 target)
    {
        attackPoint = target;
        time = Vector3.Distance(transform.position, attackPoint) / shotSpeed;
        // 计算初速度
        speed = new Vector3((attackPoint.x - transform.position.x) / time,
            (attackPoint.y - transform.position.y) / time - 0.5f * g * time, (attackPoint.z - transform.position.z) / time);
    }
}
