using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IBeAttacked
{
    [Header("����")]
    public float hp;                        // Ѫ��
    public int hardSnowballNum;             // Ӳѩ������
    public int softSnowballNum;             // ��ѩ������
    public float moveSpeed;                 // �ƶ��ٶ�
    public float throwCD;                   // ��ѩ��CD
    public List<GameObject> snowballList;      // ѩ���б�
    public GameObject iceBlock;             // ���õı���
    public GameObject track;                // �켣��

    private float m_throwTimeStamp;          // CD��ʱ��
    private Vector2 m_moveDirection;         // �ƶ�����
    private Vector3 m_throwDirection;        // Ͷ������
    private Vector3 m_attackPoint;          // ������
    private bool preparedToThrow;           // �Ƿ�׼������
    private int whichSnowball;              // 0ΪӲѩ��, 1Ϊ��ѩ��

    // ���
    private Rigidbody2D rb;
    private TrajectoryController tc;
    private PlayerInput pi;
    private Animator m_anim;
    public int giftNum = 0;
    public bool iced = false;           //�Ƿ���ice��
    public int team = 0;                //�����Ķ�
    private MakeMoveUI m_makeUI;        //����ѩ��UI
    private GiftMoveUI m_giftUI;        //����UI
    private HealthMoveUI m_healthMoveUI;//Ѫ��UI

    [SerializeField]
    private float m_speed = 5, m_iceForce = 300, m_iceFriction = 400, m_maxSpeed = 10, m_pushSmooth = 7f, m_meleeR = 1f, m_mass = 1000;
    //ѩ�������ٶȣ����϶���������Ħ��������������ٶȣ��ƶ��⻬��,��ս�뾶,����
    public virtual Vector3 attackDirection  //��ȡ�������򣨴����ƣ�
    {
        get => new Vector3(m_throwDirection.x=Input.GetAxisRaw(pi.mouseXAxis), m_throwDirection.y = Input.GetAxisRaw(pi.mouseYAxis));
    }
    public virtual bool makeDown            //�Ƿ�����ѩ��
    {
        get => Input.GetKeyDown(pi.makeSnowBall);
    }
    public virtual bool makeUp              //ͬ��
    {
        get => Input.GetKeyUp(pi.makeSnowBall);
    }
    public virtual float vx                 //��ȡx��ƫ��
    {
        get => m_moveDirection.x = Input.GetAxisRaw(pi.horizontalAxis);
    }
    public virtual float vy
    {
        get => m_moveDirection.y = Input.GetAxisRaw(pi.verticalAxis);
    }
    public virtual bool putGiftUp           //�Ƿ��·�����
    {
        get => Input.GetKeyUp(pi.putGift);
    }
    public virtual bool meleeDown           //�Ƿ��½�ս
    {
        get => Input.GetKeyDown(pi.melee);
    }
    public GameObject isEnemyNear           //�����Ƿ��ڽ�ս��Χ
    {
        get
        {
            for (int i = -75; i <= 75; i += 15)//150�ŽǷ�Χ
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
        for (int i = -75; i <= 75; i += 15)//150�ŽǷ�Χ
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
        m_healthMoveUI = HealthMoveUI.Generate(gameObject);//���Ѫ��UI
        rb = GetComponent<Rigidbody2D>();
        rb.mass = m_mass;
        tc = track.GetComponent<TrajectoryController>();
        pi = GetComponent<PlayerInput>();
    }

    void Update()
    {
        // �ֱ���ҡ���ƶ�
        //m_moveDirection.x = Input.GetAxisRaw(pi.horizontalAxis);
        //m_moveDirection.y = Input.GetAxisRaw(pi.verticalAxis);

        // �ֱ���ҡ�˿���Ͷ������
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
                Vector3 direction = (enemy.transform.position - transform.position).normalized;//�ƶ�����
                StartCoroutine(enemy.GetComponent<PlayerController>().GetMeleed(direction));
                if (iced)
                {
                    rb.velocity = -direction * m_maxSpeed * 0.3f;//�ڱ���ʱ���Լ�ҲҪ�ƶ�
                }
            }
        }
        if (m_throwDirection.magnitude != 0)   // ��ҡ����ʾ�켣
        {
            ShowTrajectory();
            preparedToThrow = true;
        }
        else
        {
            tc.ClearTrack();    // ����켣
        }
        if (fire && preparedToThrow && Time.time - m_throwTimeStamp > throwCD)   // ��RT����ѩ��
        {
            throwSnowBall();
            m_throwTimeStamp = Time.time;
            preparedToThrow = false;
        }
        if (putIce)       // ��RB���ñ���
        {
            layIceBlock();
        }
        if(switchBall)     // ��LB�л�ѩ������
        {
            whichSnowball = 1 - whichSnowball;
            tc.snowballObj = snowballList[whichSnowball];
        }

    }
    public void BeginMake()
    {
        rb.velocity = Vector2.zero;//����ѩ��ʱͣ��
        m_anim.SetBool("Making", true);
        m_makeUI = MakeMoveUI.Generate(gameObject);
    }                                             //��ʼ����
    public void CancelMake()
    {
        m_anim.SetBool("Making", false);
        Destroy(m_makeUI.gameObject);
    }                                            //ȡ������
    public void GetGift()
    {
        m_giftUI = GiftMoveUI.Generate(gameObject);
        giftNum++;
    }                                               //�������
    public IEnumerator GetMeleed(Vector3 direction)                         //�ܵ��ƶ�
    {
        rb.GetComponent<PlayerController>().GiftFall();//�������
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
    public void GiftFall()                                                 //�������
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
        Vector2 direction = new Vector2(vx, vy);//����
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

        else//���滬��
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
    //�ƶ�
    public void MakeFinished()                                              //�������
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
    /// ��ʾ�켣��
    /// </summary>
    public void ShowTrajectory()
    {
        tc.SetShootParameter();
        tc.SetDestination(m_attackPoint);
        tc.SetTrackPoint();
    }

    /// <summary>
    /// ��ѩ�򹥻�
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
    /// ���ñ���
    /// </summary>
    public void layIceBlock()
    {
        Instantiate(iceBlock, transform.position, Quaternion.identity);
    }


    /// <summary>
    /// ��Բӳ�䷨, ���б45���ƶ��ٶ�����
    /// </summary>
    private Vector2 SquareToCicle(Vector2 input)
    {
        Vector2 output = Vector2.zero;
        output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) / 2.0f);
        output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) / 2.0f);
        return output;
    }

    /// <summary>
    /// �������ӿ�
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
