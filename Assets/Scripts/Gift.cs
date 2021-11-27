using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gift : MonoBehaviour
{
    public static GameObject gifts;     //父亲组件,结束后方便统计是否在基地
    [SerializeField]
    private bool m_pickable=false;      //是否可捡，（刚扔出时不可捡）
    private void Awake()
    {
        if (!gifts)
            gifts = GameObject.Find("Gifts");//初始化父亲组件
    }
    public static void Generate(Vector3 pos)//生成一个gift
    {
        GameObject newGift= Data.Generate("Gift", gifts);
        newGift.transform.position = pos;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == Data.tagIdToString[Data.TagId.player])
        {
            if (!m_pickable|| collision.transform.GetComponent<PlayerControl>().giftNum>0) return;//如果不可捡或玩家已得礼物
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
