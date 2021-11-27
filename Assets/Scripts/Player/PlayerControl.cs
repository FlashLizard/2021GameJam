using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private Rigidbody2D m_rb;
    private Animator m_anim;
    private int m_status = 0;           //是软雪球还是硬雪球0hard 1soft
    public int giftNum = 0;
    public bool iced = false;           //是否在ice上
    public int team = 0;                //属于哪队
    private MakeMoveUI m_makeUI;        //制作雪球UI
    private GiftMoveUI m_giftUI;        //礼物UI
    private HealthMoveUI m_healthMoveUI;//血量UI

    [SerializeField]
    private float m_speed = 5, m_iceForce = 300, m_iceFriction = 400, m_maxSpeed = 10, m_pushSmooth = 7f,m_meleeR=1f,m_mass=1000;
                                        //雪地行走速度，冰上动力，冰上摩擦力，冰上最大速度，推动光滑度,近战半径,质量

    public virtual Vector3 attackDirection  //获取攻击方向（带完善）
    {
        get => new Vector3(0, 1);
    }
    public virtual bool makeDown            //是否按下做雪球
    {
        get => Input.GetButtonDown("Make");
    }
    public virtual bool makeUp              //同上
    {
        get => Input.GetButtonUp("Make");
    }
    public virtual float vx                 //获取x轴偏移
    {
        get => Input.GetAxis("Horizontal");
    }
    public virtual float vy
    {
        get => Input.GetAxis("Vertical");
    }
    public virtual bool putGiftUp           //是否按下放礼物
    {
        get => Input.GetButtonUp("PutGift");
    }
    public virtual bool meleeDown           //是否按下近战
    {
        get => Input.GetButtonDown("Melee");
    }
    public GameObject isEnemyNear           //敌人是否在近战范围
    {
        get
        {
            for (int i = -75; i <= 75; i += 15)//150张角范围
            {
                RaycastHit2D[] raycastHit2Ds =
                    Physics2D.RaycastAll(transform.position, Quaternion.AngleAxis(i, new Vector3(0, 0, 1)) * attackDirection, m_meleeR, LayerMask.GetMask("Player"));


                foreach (var it in raycastHit2Ds)
                {
                    if (it.collider && it.collider.GetComponent<PlayerControl>().team != team)
                    {
                        return it.collider.gameObject;
                    }
                }
            }
            return null;
        }
    }

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_anim = GetComponent<Animator>();
        m_healthMoveUI = HealthMoveUI.Generate(gameObject);//添加血量UI
        m_rb.mass = m_mass;
    }
    private void Update()
    {
        if (!m_anim.GetBool("Making"))
            Move();
        if (makeDown && !iced)
        {
            BeginMake();
        }
        if (makeUp&&m_anim.GetBool("Making"))
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
                StartCoroutine(enemy.GetComponent<PlayerControl>().GetMeleed(direction));
                if (iced)
                {
                    m_rb.velocity = -direction * m_maxSpeed * 0.3f;//在冰上时，自己也要移动
                }
            }
        }
    }
    public void BeginMake()
    {
        m_rb.velocity = Vector2.zero;//制作雪球时停下
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
        m_rb.GetComponent<PlayerControl>().GiftFall();//礼物掉落
        if (iced)
        {
            m_rb.velocity = direction * m_maxSpeed * 0.5f;
        }
        else
        {
            Vector3 target = transform.position + direction * 1f;
            while ((target - transform.position).magnitude > 0.05f)
            {
                m_rb.MovePosition(Vector3.Lerp(transform.position, target, m_pushSmooth * Time.deltaTime));
                yield return null;
            }
        }
    }
    private void GiftFall()                                                 //礼物掉落
    {
        if (giftNum > 0)
        {
            Gift.Generate(transform.position);
            giftNum--;
            Destroy(m_giftUI.gameObject);
        }
    }
    private void Move()
    {
        Vector2 direction = new Vector2(vx, vy);//方向
        if (direction.magnitude > 0.1f)
        {
            direction = direction.normalized;
        }
        else direction = Vector2.zero;
        if (!iced)
        {
            m_rb.velocity = direction * m_speed;
        }

        else//冰面滑动
        {
            if (m_rb.velocity.magnitude > m_maxSpeed * 0.6)
                m_rb.AddForce(direction * m_iceForce);
            else m_rb.AddForce(direction * m_iceForce * 8);
            if (m_rb.velocity.magnitude > 0.01f) m_rb.AddForce(-m_rb.velocity.normalized * m_iceFriction);
            else m_rb.velocity = Vector2.zero;
            if (m_rb.velocity.magnitude > m_maxSpeed)
            {
                m_rb.velocity = m_rb.velocity.normalized * m_maxSpeed;
            }
        }
    }                                                   //移动
    public void MakeFinished()                                              //完成制作
    {
        CancelMake();
        if (m_status == 0)
        {
            //hard++
        }
        else
        {
            //soft++
        }
    }
    private void OnDrawGizmos()                                             //方便调试近战范围，不用管
    {
        Gizmos.color = Color.grey;
        for (int i = -75; i <= 75; i += 10)
        {
            Ray ray = new Ray(transform.position, Quaternion.AngleAxis(i, new Vector3(0, 0, 1)) * attackDirection);
            Gizmos.DrawRay(ray);
        }
    }
}
