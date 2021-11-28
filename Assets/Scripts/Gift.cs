using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gift : MonoBehaviour
{
    public static GameObject gifts;     //�������,�����󷽱�ͳ���Ƿ��ڻ���
    [SerializeField]
    private bool m_pickable=false;      //�Ƿ�ɼ񣬣����ӳ�ʱ���ɼ�
    [SerializeField]
    public int team = -1;
    private void Awake()
    {
        if (!gifts)
            gifts = GameObject.Find("Gifts");//��ʼ���������
    }
    public static Gift Generate(Vector3 pos)//����һ��gift
    {
        GameObject newGift= Data.Generate("Gift", gifts);
        newGift.transform.position = pos;
        return newGift.GetComponent<Gift>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == Data.tagIdToString[Data.TagId.player])
        {
            if (!m_pickable|| collision.transform.GetComponent<PlayerController>().giftNum>0) return;//������ɼ������ѵ�����
            collision.transform.GetComponent<PlayerController>().GetGift();
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
    public static void EndScore()
    {
        for (int i = 0; i < gifts.transform.childCount; i++)
        {
            Gift gift = gifts.transform.GetChild(i).gameObject.GetComponent<Gift>();
            if (gift.team!=-1)
            {
                ScoreControlUI.current.scoreUIs[gift.team].ChangeScore(30);
            }
        }
    }
}
