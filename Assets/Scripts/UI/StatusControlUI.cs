using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusControlUI : MonoBehaviour//状态父节点UI
{
    public static StatusControlUI current;//获取当前脚本
    public List<StatusUI> statusUIs = new List<StatusUI>();//Start时赋值，获取状态ui请调用此字段（StatusControlUI.current.statusUIs[i])
    private void Awake()
    {
        current = this;
        for (int i = 0; i < transform.childCount; i++)//清空原有statusui
        {
            Destroy(transform.GetChild(i));
        }
        int nums;
        try
        {
            nums = Controller.current.playerNums;
        }
        catch
        {
            nums = 2;
        }
        for (int i = 0; i < nums; i++)
        {
            statusUIs.Add(Data.Generate("UIs/" + Data.uIIdToString[Data.UIId.status], gameObject).GetComponent<StatusUI>());
        }//根据玩家数添加statusui
    }
    private void Start()
    {
        
    }
}
