using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadyUI : MonoBehaviour//321׼��ui
{
    public void MinusNums()//������
    {
        Text nums = GetComponent<Text>();
        nums.text = (int.Parse(nums.text) - 1).ToString();
    }
    public void Start()
    {
        GetComponent<Animator>().updateMode = AnimatorUpdateMode.UnscaledTime;//ʹ�Լ�������ͣӰ��
        Time.timeScale = 0;
    }
    public void ContinueGame()
    {
        Time.timeScale = 1;
        Destroy(transform.parent.gameObject);
    }
}
