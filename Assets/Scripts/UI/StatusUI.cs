using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusUI : MonoBehaviour
{
    [SerializeField]
    private GameObject m_health, m_bullets;//血量与雪球条
    [SerializeField]
    private Color m_color;
    private int m_healthBegin=-1, m_hardBulletBegin=-1, m_softBulletBegin=-1;//先放置硬雪球，后放置软雪球
    public void Recover()
    {
        Data.ClearChildren(m_health);
        Data.ClearChildren(m_bullets);
        m_healthBegin = m_hardBulletBegin = m_softBulletBegin = -1;
        GetComponent<Image>().color = m_color;
    }
    public void GetToGrey()
    {
        GetComponent<Image>().color = Color.grey;
    }
    public void SetHealth(int num)
    {
        ChangeHealth(num - m_healthBegin - 1);
    }
    public void SetHardBullet(int num)
    {
        ChangeHardBullet(num - m_hardBulletBegin - 1);
    }
    public void SetSoftBullet(int num)
    {
        ChangeSoftBullet(num - (m_softBulletBegin- m_hardBulletBegin));
    }
    public void ChangeHealth(int num)//增加血量，num为数量，如果为负数，则减去（请保证在范围内）
    {
        ChangeValue(m_health, Data.UIId.blood, ref m_healthBegin, num);
    }
    public void ChangeSoftBullet(int num)
    {
        ChangeValue(m_bullets, Data.UIId.softBullet, ref m_softBulletBegin, num);
    }//增加软雪球，num为数量，如果为负数，则减去（请保证在范围内）
    public void ChangeHardBullet(int num)//增加硬雪球，num为数量，如果为负数，则减去（请保证在范围内）
    {
        m_softBulletBegin += num;
        ChangeValue(m_bullets,Data.UIId.hardBullet,ref m_hardBulletBegin,num);
    }
    public void ChangeValue(GameObject values,Data.UIId id, ref int begin,int num)
    {
        if(num<0)
        {
            num = -num;
            while(num>0)
            {
                Destroy(values.transform.GetChild(begin--).gameObject);
                num--;
            }
        }
        else
        {
            while (num > 0)
            {
                GameObject image=Data.Generate("UIs/" + Data.uIIdToString[id], values);
                image.transform.SetSiblingIndex(++begin);
                num--;
            }
        }
    }
    private void Update()
    {
        #region debug
        //if(Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    ChangeHealth(-1);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    ChangeHealth(1);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    ChangeHardBullet(1);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha4))
        //{
        //    ChangeHardBullet(-1);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha5))
        //{
        //    ChangeSoftBullet(1);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha6))
        //{
        //    ChangeSoftBullet(-1);
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha7))
        //{
        //    SetHardBullet(2);
        //}
        #endregion
    }

}
