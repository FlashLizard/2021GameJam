using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthMoveUI : MoveUI//�ƶ�Ѫ��UI
{
    private int m_health=7;                             //Ѫ��
    private float m_delta=0.2f;                         //һ�񳤶�
    public void Awake()
    {
        m_height += 0.95f;//����������moveui�غ�
    }
    public static HealthMoveUI Generate(GameObject player)
    {
        return MoveUI.Generate(Data.UIId.healthMove, player).GetComponent<HealthMoveUI>();
    }
    public void Set(int num)                            //����uiѪ��Ϊnum
    {
        m_health = num;
        Fresh();
    }
    public void Change(int num)                         //uiѪ���ı�num
    {
        m_health += num;
        Fresh();
    }
    public void Fresh()                                 //ˢ��ui
    {
        transform.localScale = new Vector3(m_health * m_delta, m_delta, 0);
        transform.position = new Vector3(pc.transform.position.x-(7f-m_health)*m_delta/2, transform.position.y);
    }
    public void Update()                                //�����ã�����
    {
        //if(Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    Change(1);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    Change(-1);
        //}
    }
}
