using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthMoveUI : MoveUI//移动血量UI
{
    private int m_health=7;                             //血量
    private float m_delta=0.2f;                         //一格长度
    public void Awake()
    {
        m_height += 0.95f;//避免与其他moveui重合
    }
    public static HealthMoveUI Generate(GameObject player)
    {
        return MoveUI.Generate(Data.UIId.healthMove, player).GetComponent<HealthMoveUI>();
    }
    public void Set(int num)                            //设置ui血量为num
    {
        m_health = num;
        Fresh();
    }
    public void Change(int num)                         //ui血量改变num
    {
        m_health += num;
        Fresh();
    }
    public void Fresh()                                 //刷新ui
    {
        transform.localScale = new Vector3(m_health * m_delta, m_delta, 0);
        transform.position = new Vector3(pc.transform.position.x-(7f-m_health)*m_delta/2, transform.position.y);
    }
    public void Update()                                //调试用，不管
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
