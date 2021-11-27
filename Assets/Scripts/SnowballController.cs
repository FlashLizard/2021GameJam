using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowballController : MonoBehaviour
{
    [Header("����")]
    public float shotSpeed;         // �׳��ٶ�
    public float g;                 // �������ٶ�

    private float time;             // ��㵽�յ��ʱ��
    private float passTime;         // ������ʱ��
    private Vector3 attackPoint;    // �յ�
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

    public void Shoot()
    {
        passTime = 0f;
    }

    /// <summary>
    /// ���ù�����
    /// </summary>
    public void SetAttackPoint(Vector3 target)
    {
        attackPoint = target;
        time = Vector3.Distance(transform.position, attackPoint) / shotSpeed;
        // ������ٶ�
        speed = new Vector3((attackPoint.x - transform.position.x) / time,
            (attackPoint.y - transform.position.y) / time - 0.5f * g * time, (attackPoint.z - transform.position.z) / time);
    }
}
