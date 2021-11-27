using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gift : MonoBehaviour
{
    public static GameObject gifts;     //�������,�����󷽱�ͳ���Ƿ��ڻ���
    [SerializeField]
    private bool m_pickable=false;      //�Ƿ�ɼ񣬣����ӳ�ʱ���ɼ�
    private void Awake()
    {
        if (!gifts)
            gifts = GameObject.Find("Gifts");//��ʼ���������
    }
    public static void Generate(Vector3 pos)//����һ��gift
    {
        GameObject newGift= Data.Generate("Gift", gifts);
        newGift.transform.position = pos;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == Data.tagIdToString[Data.TagId.player])
        {
            if (!m_pickable|| collision.transform.GetComponent<PlayerControl>().giftNum>0) return;//������ɼ������ѵ�����
            collision.transform.GetComponent<PlayerControl>().GetGift();
            Destroy(gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == Data.tagIdToString[Data.TagId.player])
        {
            m_pickable = true;
        }
    }
}
