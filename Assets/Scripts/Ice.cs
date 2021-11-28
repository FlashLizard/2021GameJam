using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : MonoBehaviour
{
    [SerializeField]
    public int team = -1;   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == Data.tagIdToString[Data.TagId.player])
        {
            collision.transform.GetComponent<PlayerController>().iced = this;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == Data.tagIdToString[Data.TagId.player])
        {
            collision.gameObject.GetComponent<PlayerController>().iced = null;
        }
    }
}
