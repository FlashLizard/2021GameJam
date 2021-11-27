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

    void Awake()
    {
        whichSnowball = 0;
        rb = GetComponent<Rigidbody2D>();
        tc = track.GetComponent<TrajectoryController>();
        pi = GetComponent<PlayerInput>();
    }

    void Update()
    {
        // �ֱ���ҡ���ƶ�
        m_moveDirection.x = Input.GetAxisRaw(pi.horizontalAxis);
        m_moveDirection.y = Input.GetAxisRaw(pi.verticalAxis);

        // �ֱ���ҡ�˿���Ͷ������
        m_throwDirection.x = Input.GetAxisRaw(pi.mouseXAxis);
        m_throwDirection.y = Input.GetAxisRaw(pi.mouseYAxis);
        m_throwDirection = SquareToCicle(m_throwDirection);

        var maxDistance = snowballList[whichSnowball].GetComponent<SnowballController>().throwDistance;
        m_attackPoint = transform.position - maxDistance * m_throwDirection;

        if (m_throwDirection.magnitude != 0)   // ��ҡ����ʾ�켣
        {
            ShowTrajectory();
            preparedToThrow = true;
        }
        else
        {
            tc.ClearTrack();    // ����켣
        }
        if (Input.GetAxisRaw(pi.fireAxis) != 0 && preparedToThrow && Time.time - m_throwTimeStamp > throwCD)   // ��RT����ѩ��
        {
            throwSnowBall();
            m_throwTimeStamp = Time.time;
            preparedToThrow = false;
        }
        if (Input.GetKeyDown(pi.layIceBlock))       // ��RB���ñ���
        {
            layIceBlock();
        }
        if(Input.GetKeyDown(pi.switchIceBlock))     // ��LB�л�ѩ������
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
