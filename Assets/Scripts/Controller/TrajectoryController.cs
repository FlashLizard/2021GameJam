using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryController : MonoBehaviour
{
    [Header("抛物线轨迹")]
    public int pointCount;              // 轨迹点个数
    public GameObject snowballObj;         // 雪球

    private float shotSpeed;         // 抛出速度
    private float g;                 // 重力加速度
    private float time;             // 起点到终点的时间
    private Vector3 speed;              // 初速度向量
    private Vector3 Gravity;            // 重力向量
    private float intervalTime;         // 两个轨迹点之间的间隔时间

    // 组件
    private LineRenderer lineRenderer;
    private SnowballController snowball;

    void Awake()
    {
        Gravity = Vector3.zero;
        lineRenderer = GetComponent<LineRenderer>();
    }

    /// <summary>
    /// 设置轨迹的发射参数
    /// </summary>
    public void SetShootParameter()
    {
        snowball = snowballObj.GetComponent<SnowballController>();
        shotSpeed = snowball.shotSpeed;
        g = snowball.g;
    }

    /// <summary>
    /// 设置抛物线轨迹终点
    /// </summary>
    public void SetDestination(Vector3 destination)
    {
        time = Vector3.Distance(transform.position, destination) / shotSpeed;
        // 计算初速度
        if(time != 0)
        {
            speed = new Vector3((destination.x - transform.position.x) / time, 
                (destination.y - transform.position.y) / time - 0.5f * g * time, (destination.z - transform.position.z) / time);
        }
        // 计算机轨迹点间隔时间
        intervalTime = time / pointCount;
    }

    /// <summary>
    /// 计算所有的轨迹点
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

    /// <summary>
    /// 清除轨迹
    /// </summary>
    public void ClearTrack()
    {
        lineRenderer.positionCount = 0;
    }
}
