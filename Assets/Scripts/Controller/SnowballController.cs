using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowballController : MonoBehaviour
{
    [Header("属性")]
    public int team;
    public int attackDamage;      // 攻击伤害
    public float shotSpeed;         // 抛出速度
    public float g;                 // 重力加速度
    public float throwDistance;     // 投掷距离
    public GameObject hitEffect;    // 爆炸特效

    private float time;             // 起点到终点的时间
    private float passTime;         // 经过的时间
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


    public void OnTriggerEnter2D(Collider2D collider2D)
    {
        if(collider2D.tag != "Player")
        {
            if(collider2D.GetComponent<IBeAttacked>() != null)
            {
                collider2D.GetComponent<IBeAttacked>().BeAttacked(attackDamage);
                DestroySelf();
            }
        }
        else if(collider2D.GetComponent<PlayerController>().team!=team)
        {
            collider2D.GetComponent<IBeAttacked>().BeAttacked(attackDamage);
            DestroySelf();
        }
    }

    public void Shoot(int fromTeam)
    {
        int team = fromTeam;
        passTime = 0f;
        Invoke("DestroySelf", time);
    }

    /// <summary>
    /// 设置攻击点
    /// </summary>
    public void SetAttackPoint(Vector3 target)
    {
        time = Vector3.Distance(transform.position, target) / shotSpeed;
        // 计算初速度
        speed = new Vector3((target.x - transform.position.x) / time,
            (target.y - transform.position.y) / time - 0.5f * g * time, (target.z - transform.position.z) / time);
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
        GameObject hit = Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(hit, 0.1f);
    }
}
