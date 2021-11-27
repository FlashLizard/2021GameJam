using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IBeAttacked
{
    [Header("属性")]
    public float hp;                        // 血量
    public int hardSnowballNum;             // 硬雪球数量
    public int softSnowballNum;             // 软雪球数量
    public float moveSpeed;                 // 移动速度
    public float throwCD;                   // 扔雪球CD
    public List<GameObject> snowballList;      // 雪球列表
    public GameObject iceBlock;             // 放置的冰块
    public GameObject track;                // 轨迹线

    private float m_throwTimeStamp;          // CD计时器
    private Vector2 m_moveDirection;         // 移动方向
    private Vector3 m_throwDirection;        // 投掷方向
    private Vector3 m_attackPoint;          // 攻击点
    private bool preparedToThrow;           // 是否准备发射
    private int whichSnowball;              // 0为硬雪球, 1为软雪球

    // 组件
    private Rigidbody2D rb;
    private TrajectoryController tc;
    private PlayerInput pi;

    void Awake()
    {
        whichSnowball = 0;
        rb = GetComponent<Rigidbody2D>();
        tc = track.GetComponent<TrajectoryController>();
        pi = GetComponent<PlayerInput>();
    }

    void Update()
    {
        // 手柄左摇杆移动
        m_moveDirection.x = Input.GetAxisRaw(pi.horizontalAxis);
        m_moveDirection.y = Input.GetAxisRaw(pi.verticalAxis);

        // 手柄右摇杆控制投掷方向
        m_throwDirection.x = Input.GetAxisRaw(pi.mouseXAxis);
        m_throwDirection.y = Input.GetAxisRaw(pi.mouseYAxis);
        m_throwDirection = SquareToCicle(m_throwDirection);

        var maxDistance = snowballList[whichSnowball].GetComponent<SnowballController>().throwDistance;
        m_attackPoint = transform.position - maxDistance * m_throwDirection;

        if (m_throwDirection.magnitude != 0)   // 右摇杆显示轨迹
        {
            ShowTrajectory();
            preparedToThrow = true;
        }
        else
        {
            tc.ClearTrack();    // 清除轨迹
        }
        if (Input.GetAxisRaw(pi.fireAxis) != 0 && preparedToThrow && Time.time - m_throwTimeStamp > throwCD)   // 按RT发射雪球
        {
            throwSnowBall();
            m_throwTimeStamp = Time.time;
            preparedToThrow = false;
        }
        if (Input.GetKeyDown(pi.layIceBlock))       // 按RB放置冰块
        {
            layIceBlock();
        }
        if(Input.GetKeyDown(pi.switchIceBlock))     // 按LB切换雪球类型
        {
            whichSnowball = 1 - whichSnowball;
            tc.snowballObj = snowballList[whichSnowball];
        }

    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + SquareToCicle(m_moveDirection) * moveSpeed * Time.fixedDeltaTime);
    }


    /// <summary>
    /// 显示轨迹线
    /// </summary>
    public void ShowTrajectory()
    {
        tc.SetShootParameter();
        tc.SetDestination(m_attackPoint);
        tc.SetTrackPoint();
    }

    /// <summary>
    /// 扔雪球攻击
    /// </summary>
    public void throwSnowBall()
    {
        if (whichSnowball == 0 && hardSnowballNum > 0)
            hardSnowballNum--;
        else if (whichSnowball == 1 && softSnowballNum > 0)
            softSnowballNum--;
        else return;
        GameObject obj = Instantiate(snowballList[whichSnowball], transform.position, Quaternion.identity);
        obj.GetComponent<SnowballController>().SetAttackPoint(m_attackPoint);
        obj.GetComponent<SnowballController>().Shoot();
    }


    /// <summary>
    /// 放置冰块
    /// </summary>
    public void layIceBlock()
    {
        Instantiate(iceBlock, transform.position, Quaternion.identity);
    }


    /// <summary>
    /// 椭圆映射法, 解决斜45度移动速度问题
    /// </summary>
    private Vector2 SquareToCicle(Vector2 input)
    {
        Vector2 output = Vector2.zero;
        output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) / 2.0f);
        output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) / 2.0f);
        return output;
    }

    /// <summary>
    /// 被攻击接口
    /// </summary>
    public void BeAttacked(float damage) 
    {
        hp -= damage;
        if(hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
