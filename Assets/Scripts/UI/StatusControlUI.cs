using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusControlUI : MonoBehaviour//״̬���ڵ�UI
{
    public static StatusControlUI current;//��ȡ��ǰ�ű�
    public List<StatusUI> statusUIs = new List<StatusUI>();//Startʱ��ֵ����ȡ״̬ui����ô��ֶΣ�StatusControlUI.current.statusUIs[i])
    private void Awake()
    {
        current = this;
        for (int i = 0; i < transform.childCount; i++)//���ԭ��statusui
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
        }//������������statusui
    }
    private void Start()
    {
        
    }
}
