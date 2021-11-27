using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour//得分UI
{
    [SerializeField]
    private Text m_text;//分数
    [SerializeField]
    private int m_score;//分数值
    public int score
    {
        get { return m_score; }
    }
    private void Start()
    {
        SetScore(0);
    }
    public void SetScore(int num)//设置得分
    {
        m_score = num;
        Fresh();
    }
    public void ChangeScore(int num)//改变得分
    {
        m_score += num;
        Fresh();
    }
    private void Fresh()//刷新得分
    {
        m_text.text = score.ToString();
    }
}
