using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadyUI : MonoBehaviour//321准备ui
{
    public void MinusNums()//剪数字
    {
        Text nums = GetComponent<Text>();
        nums.text = (int.Parse(nums.text) - 1).ToString();
    }
    public void Start()
    {
        GetComponent<Animator>().updateMode = AnimatorUpdateMode.UnscaledTime;//使自己不受暂停影响
        Time.timeScale = 0;
    }
    public void ContinueGame()
    {
        Time.timeScale = 1;
        Destroy(transform.parent.gameObject);
    }
}
