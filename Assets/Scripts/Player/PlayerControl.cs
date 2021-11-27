using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private Rigidbody2D m_rb;
    private Animator m_anim;
    private int m_status = 0;           //����ѩ����Ӳѩ��0hard 1soft
    public int giftNum = 0;
    public bool iced = false;           //�Ƿ���ice��
    public int team = 0;                //�����Ķ�
    private MakeMoveUI m_makeUI;        //����ѩ��UI
    private GiftMoveUI m_giftUI;        //����UI
    private HealthMoveUI m_healthMoveUI;//Ѫ��UI

    [SerializeField]
    private float m_speed = 5, m_iceForce = 300, m_iceFriction = 400, m_maxSpeed = 10, m_pushSmooth = 7f,m_meleeR=1f,m_mass=1000;
                                        //ѩ�������ٶȣ����϶���������Ħ��������������ٶȣ��ƶ��⻬��,��ս�뾶,����

    public virtual Vector3 attackDirection  //��ȡ�������򣨴����ƣ�
    {
        get => new Vector3(0, 1);
    }
    public virtual bool makeDown            //�Ƿ�����ѩ��
    {
        get => Input.GetButtonDown("Make");
    }
    public virtual bool makeUp              //ͬ��
    {
        get => Input.GetButtonUp("Make");
    }
    public virtual float vx                 //��ȡx��ƫ��
    {
        get => Input.GetAxis("Horizontal");
    }
    public virtual float vy
    {
        get => Input.GetAxis("Vertical");
    }
    public virtual bool putGiftUp           //�Ƿ��·�����
    {
        get => Input.GetButtonUp("PutGift");
    }
    public virtual bool meleeDown           //�Ƿ��½�ս
    {
        get => Input.GetButtonDown("Melee");
    }
    public GameObject isEnemyNear           //�����Ƿ��ڽ�ս��Χ
    {
        get
        {
            for (int i = -75; i <= 75; i += 15)//150�ŽǷ�Χ
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
        m_healthMoveUI = HealthMoveUI.Generate(gameObject);//���Ѫ��UI
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
                Vector3 direction = (enemy.transform.position - transform.position).normalized;//�ƶ�����
                StartCoroutine(enemy.GetComponent<PlayerControl>().GetMeleed(direction));
                if (iced)
                {
                    m_rb.velocity = -direction * m_maxSpeed * 0.3f;//�ڱ���ʱ���Լ�ҲҪ�ƶ�
                }
            }
        }
    }
    public void BeginMake()
    {
        m_rb.velocity = Vector2.zero;//����ѩ��ʱͣ��
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
        m_rb.GetComponent<PlayerControl>().GiftFall();//�������
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
    private void GiftFall()                                                 //�������
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
        Vector2 direction = new Vector2(vx, vy);//����
        if (direction.magnitude > 0.1f)
        {
            direction = direction.normalized;
        }
        else direction = Vector2.zero;
        if (!iced)
        {
            m_rb.velocity = direction * m_speed;
        }

        else//���滬��
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
    }                                                   //�ƶ�
    public void MakeFinished()                                              //�������
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
    private void OnDrawGizmos()                                             //������Խ�ս��Χ�����ù�
    {
        Gizmos.color = Color.grey;
        for (int i = -75; i <= 75; i += 10)
        {
            Ray ray = new Ray(transform.position, Quaternion.AngleAxis(i, new Vector3(0, 0, 1)) * attackDirection);
            Gizmos.DrawRay(ray);
        }
    }
}
