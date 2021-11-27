using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IBeAttacked
{
    [Header("����")]
    public float hp;                        // Ѫ��
    public float moveSpeed;                 // �ƶ��ٶ�
    public GameObject snowBall;             // ѩ��
    public GameObject iceBlock;             // ���õı���
    public GameObject track;                // �켣��

    private Vector2 m_moveDirection;        // �ƶ�����
    private Vector3 m_attackPoint;          // ������

    // ���
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        m_moveDirection.x = Input.GetAxisRaw("Horizontal");
        m_moveDirection.y = Input.GetAxisRaw("Vertical");

        // ������������
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 mousePosOnScreen = new Vector3(Input.mousePosition.x, Input.mousePosition.y, pos.z);
        Vector3 mousePosOnWorld = Camera.main.ScreenToWorldPoint(mousePosOnScreen);
        m_attackPoint = 2 * transform.position - mousePosOnWorld;

        if (Input.GetMouseButton(0))        // һֱ���������ʾ�켣
        {
            ShowTrajectory();
        }
        if(Input.GetMouseButtonUp(0))       // �ɿ��������ѩ��
        {
            throwSnowBall();
        }
        if(Input.GetKeyDown(KeyCode.Space)) // ���¿ո���ñ���
        {

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
        track.GetComponent<TrajectoryController>().SetDestination(m_attackPoint);
        track.GetComponent<TrajectoryController>().SetTrackPoint();
    }

    /// <summary>
    /// ��ѩ�򹥻�
    /// </summary>
    public void throwSnowBall()
    {
        GameObject obj = Instantiate(snowBall, transform.position, Quaternion.identity);
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

    public void BeAttacked() { }
}
