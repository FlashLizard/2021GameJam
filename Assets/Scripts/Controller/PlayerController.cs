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
    private Animator m_anim;
    public int giftNum = 0;
    public bool iced = false;           //是否在ice上
    public int team = 0;                //属于哪队
    private MakeMoveUI m_makeUI;        //制作雪球UI
    private GiftMoveUI m_giftUI;        //礼物UI
    private HealthMoveUI m_healthMoveUI;//血量UI

    [SerializeField]
    private float m_speed = 5, m_iceForce = 300, m_iceFriction = 400, m_maxSpeed = 10, m_pushSmooth = 7f, m_meleeR = 1f, m_mass = 1000;
    //雪地行走速度，冰上动力，冰上摩擦力，冰上最大速度，推动光滑度,近战半径,质量
    public virtual Vector3 attackDirection  //获取攻击方向（带完善）
    {
        get => new Vector3(m_throwDirection.x=Input.GetAxisRaw(pi.mouseXAxis), m_throwDirection.y = Input.GetAxisRaw(pi.mouseYAxis));
    }
    public virtual bool makeDown            //是否按下做雪球
    {
        get => Input.GetKeyDown(pi.makeSnowBall);
    }
    public virtual bool makeUp              //同上
    {
        get => Input.GetKeyUp(pi.makeSnowBall);
    }
    public virtual float vx                 //获取x轴偏移
    {
        get => m_moveDirection.x = Input.GetAxisRaw(pi.horizontalAxis);
    }
    public virtual float vy
    {
        get => m_moveDirection.y = Input.GetAxisRaw(pi.verticalAxis);
    }
    public virtual bool putGiftUp           //是否按下放礼物
    {
        get => Input.GetKeyUp(pi.putGift);
    }
    public virtual bool meleeDown           //是否按下近战
    {
        get => Input.GetKeyDown(pi.melee);
    }
    public GameObject isEnemyNear           //敌人是否在近战范围
    {
        get
        {
            for (int i = -75; i <= 75; i += 15)//150张角范围
            {
                Debug.Log(attackDirection.normalized);
                RaycastHit2D[] raycastHit2Ds =
                    Physics2D.RaycastAll(transform.position, Quaternion.AngleAxis(i, new Vector3(0, 0, 1)) * attackDirection.normalized, m_meleeR, LayerMask.GetMask("Player"));


                foreach (var it in raycastHit2Ds)
                {
                    if (it.collider && it.collider.GetComponent<PlayerController>().team != team)
                    {
                        return it.collider.gameObject;
                    }
                }
            }
            return null;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.grey;
        for (int i = -75; i <= 75; i += 15)//150张角范围
        {
            Ray ray = new Ray(transform.position, Quaternion.AngleAxis(i, new Vector3(0, 0, 1)) * attackDirection.normalized);
            Gizmos.DrawRay(ray);
        }
    }
    public virtual bool fire
    {
        get => Input.GetAxisRaw(pi.fireAxis)!=0;
    }
    public virtual bool putIce
    {
        get => Input.GetKeyDown(pi.layIceBlock);
    }
    public virtual bool switchBall
    {
        get => Input.GetKeyDown(pi.switchIceBlock);
    }

    void Awake()
    {
        whichSnowball = 0;
        m_anim = GetComponent<Animator>();
        m_healthMoveUI = HealthMoveUI.Generate(gameObject);//添加血量UI
        rb = GetComponent<Rigidbody2D>();
        rb.mass = m_mass;
        tc = track.GetComponent<TrajectoryController>();
        pi = GetComponent<PlayerInput>();
    }

    void Update()
    {
        // 手柄左摇杆移动
        //m_moveDirection.x = Input.GetAxisRaw(pi.horizontalAxis);
        //m_moveDirection.y = Input.GetAxisRaw(pi.verticalAxis);

        // 手柄右摇杆控制投掷方向
        m_throwDirection.x = Input.GetAxisRaw(pi.mouseXAxis);
        m_throwDirection.y = Input.GetAxisRaw(pi.mouseYAxis);
        m_throwDirection = SquareToCicle(m_throwDirection);

        var maxDistance = snowballList[whichSnowball].GetComponent<SnowballController>().throwDistance;
        m_attackPoint = transform.position - maxDistance * m_throwDirection;

        if (!m_anim.GetBool("Making"))
            Move();
        if (makeDown && !iced)
        {
            Debug.Log("makedown");
            BeginMake();
        }
        if (makeUp && m_anim.GetBool("Making"))
        {
            CancelMake();
        }
        if (putGiftUp)
        {
            GiftFall();
        }
        if (meleeDown)
        {
            GameObject enemy = isEnemyNear;
            if (enemy)
            {
                Vector3 direction = (enemy.transform.position - transform.position).normalized;//推动方向
                StartCoroutine(enemy.GetComponent<PlayerController>().GetMeleed(direction));
                if (iced)
                {
                    rb.velocity = -direction * m_maxSpeed * 0.3f;//在冰上时，自己也要移动
                }
            }
        }
        if (m_throwDirection.magnitude != 0)   // 右摇杆显示轨迹
        {
            ShowTrajectory();
            preparedToThrow = true;
        }
        else
        {
            tc.ClearTrack();    // 清除轨迹
        }
        if (fire && preparedToThrow && Time.time - m_throwTimeStamp > throwCD)   // 按RT发射雪球
        {
            throwSnowBall();
            m_throwTimeStamp = Time.time;
            preparedToThrow = false;
        }
        if (putIce)       // 按RB放置冰块
        {
            layIceBlock();
        }
        if(switchBall)     // 按LB切换雪球类型
        {
            whichSnowball = 1 - whichSnowball;
            tc.snowballObj = snowballList[whichSnowball];
        }

    }
    public void BeginMake()
    {
        rb.velocity = Vector2.zero;//制作雪球时停下
        m_anim.SetBool("Making", true);
        m_makeUI = MakeMoveUI.Generate(gameObject);
    }                                             //开始制作
    public void CancelMake()
    {
        m_anim.SetBool("Making", false);
        Destroy(m_makeUI.gameObject);
    }                                            //取消制作
    public void GetGift()
    {
        m_giftUI = GiftMoveUI.Generate(gameObject);
        giftNum++;
    }                                               //获得礼物
    public IEnumerator GetMeleed(Vector3 direction)                         //受到推动
    {
        rb.GetComponent<PlayerController>().GiftFall();//礼物掉落
        if (iced)
        {
            rb.velocity = direction * m_maxSpeed * 0.5f;
        }
        else
        {
            Vector3 target = transform.position + direction * 1f;
            while ((target - transform.position).magnitude > 0.05f)
            {
                rb.MovePosition(Vector3.Lerp(transform.position, target, m_pushSmooth * Time.deltaTime));
                yield return null;
            }
        }
    }
    public void GiftFall()                                                 //礼物掉落
    {
        if (giftNum > 0)
        {
            Gift.Generate(transform.position);
            giftNum--;
            Destroy(m_giftUI.gameObject);
        }
    }
    //void FixedUpdate()
    //{
    //    if(!iced) rb.MovePosition(rb.position + SquareToCicle(m_moveDirection) * moveSpeed * Time.fixedDeltaTime);
    //    else
    //    {

    //    }
    //}
    private void Move()
    {
        Vector2 direction = new Vector2(vx, vy);//方向
        Debug.Log(direction);
        if (direction.magnitude > 0.1f)
        {
            direction = direction.normalized;
        }
        else direction = Vector2.zero;

        if (!iced)
        {
            rb.velocity = direction * m_speed;
        }

        else//冰面滑动
        {
            if (rb.velocity.magnitude > m_maxSpeed * 0.6)
                rb.AddForce(direction * m_iceForce);
            else rb.AddForce(direction * m_iceForce * 8);
            if (rb.velocity.magnitude > 0.01f) rb.AddForce(-rb.velocity.normalized * m_iceFriction);
            else rb.velocity = Vector2.zero;
            if (rb.velocity.magnitude > m_maxSpeed)
            {
                rb.velocity = rb.velocity.normalized * m_maxSpeed;
            }
        }
    }
    //移动
    public void MakeFinished()                                              //完成制作
    {
        CancelMake();
        if (whichSnowball == 0)
        {
            //hard++
        }
        else
        {
            //soft++
        }
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
