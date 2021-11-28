using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowballController : MonoBehaviour
{
    [Header("����")]
    public int team;
    public int attackDamage;      // �����˺�
    public float shotSpeed;         // �׳��ٶ�
    public float g;                 // �������ٶ�
    public float throwDistance;     // Ͷ������
    public GameObject hitEffect;    // ��ը��Ч

    private float time;             // ��㵽�յ��ʱ��
    private float passTime;         // ������ʱ��
    private Vector3 speed;          // ���ٶ�����
    private Vector3 Gravity;        // ��������


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
            transform.position += (speed + Gravity) * Time.deltaTime;  //ģ��λ��
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
    /// ���ù�����
    /// </summary>
    public void SetAttackPoint(Vector3 target)
    {
        time = Vector3.Distance(transform.position, target) / shotSpeed;
        // ������ٶ�
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
