using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBlockController : MonoBehaviour, IBeAttacked
{
    [Header("属性")]
    public int hp;

    // 组件
    private Collider2D coll;

    void Awake()
    {
        coll = GetComponent<Collider2D>();
        coll.isTrigger = true;
    }

    /// <summary>
    /// 玩家生成冰块, 退出时冰块的碰撞器启用
    /// </summary>
    public void OnTriggerExit2D(Collider2D collider2D)
    {
        if(collider2D.tag == "Player")
        {
            coll.isTrigger = false;
        }
    }

    /// <summary>
    /// 被攻击接口
    /// </summary>
    public void BeAttacked(int damage)
    {
        hp -= damage;
        if(hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
