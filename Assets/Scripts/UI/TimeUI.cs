using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeUI : MonoBehaviour//游戏倒计时UI
{
    private int m_time;
    private Text m_text;
    private void Awake()
    {
        m_text = GetComponent<Text>();
    }
    void Start()
    {
        try
        {
            m_time = Controller.current.time;
        }
        catch
        {
            m_time = 5 * 60;
        }
        Invoke("MinusTime", 1f);
    }
    void ShowTime()
    {
        m_text.text = string.Format("{0:00}:{1:00}", m_time / 60, m_time % 60);
    }
    void MinusTime()
    {
        m_time--;
        ShowTime();
        if (m_time == 0)
        {
            Controller.current.SwitchScene("victory");//等于0，就切换到胜利界面
        }
        Invoke("MinusTime", 1f);
    }
}
