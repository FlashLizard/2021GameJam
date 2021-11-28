using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBlockController : MonoBehaviour, IBeAttacked
{
    [Header("����")]
    public int hp;

    // ���
    private Collider2D coll;

    void Awake()
    {
        coll = GetComponent<Collider2D>();
        coll.isTrigger = true;
    }

    /// <summary>
    /// ������ɱ���, �˳�ʱ�������ײ������
    /// </summary>
    public void OnTriggerExit2D(Collider2D collider2D)
    {
        if(collider2D.tag == "Player")
        {
            coll.isTrigger = false;
        }
    }

    /// <summary>
    /// �������ӿ�
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
