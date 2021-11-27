using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IBeAttacked
{
    [Header("属性")]
    public float hp;                        // 血量
    public float moveSpeed;                 // 移动速度
    public GameObject snowBall;             // 雪球
    public GameObject iceBlock;             // 放置的冰块
    public GameObject track;                // 轨迹线

    private Vector2 m_moveDirection;        // 移动方向
    private Vector3 m_attackPoint;          // 攻击点

    // 组件
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        m_moveDirection.x = Input.GetAxisRaw("Horizontal");
        m_moveDirection.y = Input.GetAxisRaw("Vertical");

        // 鼠标的世界坐标
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 mousePosOnScreen = new Vector3(Input.mousePosition.x, Input.mousePosition.y, pos.z);
        Vector3 mousePosOnWorld = Camera.main.ScreenToWorldPoint(mousePosOnScreen);
        m_attackPoint = 2 * transform.position - mousePosOnWorld;

        if (Input.GetMouseButton(0))        // 一直按下左键显示轨迹
        {
            ShowTrajectory();
        }
        if(Input.GetMouseButtonUp(0))       // 松开左键发射雪球
        {
            throwSnowBall();
        }
        if(Input.GetKeyDown(KeyCode.Space)) // 按下空格放置冰块
        {

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
        track.GetComponent<TrajectoryController>().SetDestination(m_attackPoint);
        track.GetComponent<TrajectoryController>().SetTrackPoint();
    }

    /// <summary>
    /// 扔雪球攻击
    /// </summary>
    public void throwSnowBall()
    {
        GameObject obj = Instantiate(snowBall, transform.position, Quaternion.identity);
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

    public void BeAttacked() { }
}
