using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryController : MonoBehaviour
{
    [Header("�����߹켣")]
    public float shotSpeed;         // �׳��ٶ�
    public float g;                 // �������ٶ�
    public int pointCount;              // �켣�����

    private float time;             // ��㵽�յ��ʱ��
    private Vector3 speed;              // ���ٶ�����
    private Vector3 Gravity;            // ��������
    private float intervalTime;         // �����켣��֮��ļ��ʱ��

    // ���
    private LineRenderer lineRenderer;

    void Awake()
    {
        Gravity = Vector3.zero;
        lineRenderer = GetComponent<LineRenderer>();
    }

    /// <summary>
    /// ���������߹켣�յ�
    /// </summary>
    public void SetDestination(Vector3 destination)
    {
        time = Vector3.Distance(transform.position, destination) / shotSpeed;
        // ������ٶ�
        speed = new Vector3((destination.x - transform.position.x) / time,
            (destination.y - transform.position.y) / time - 0.5f * g * time, (destination.z - transform.position.z) / time);
        // ������켣����ʱ��
        intervalTime = time / pointCount;
    }

    /// <summary>
    /// �������еĹ켣��
    /// </summary>
    public void SetTrackPoint()
    {
        Vector3 pos = transform.position;
        Vector3 fall = Gravity;
        lineRenderer.positionCount = pointCount;
        for (int i = 0; i < pointCount; i++)
        {
            fall.y = g * i * intervalTime;
            pos += (speed + fall) * intervalTime;
            lineRenderer.SetPosition(i, pos);
        }
    }
}
