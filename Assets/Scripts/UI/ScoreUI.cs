using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour//�÷�UI
{
    [SerializeField]
    private Text m_text;//����
    [SerializeField]
    private int m_score;//����ֵ
    public int score
    {
        get { return m_score; }
    }
    private void Start()
    {
        SetScore(0);
    }
    public void SetScore(int num)//���õ÷�
    {
        m_score = num;
        Fresh();
    }
    public void ChangeScore(int num)//�ı�÷�
    {
        m_score += num;
        Fresh();
    }
    private void Fresh()//ˢ�µ÷�
    {
        m_text.text = score.ToString();
    }
}
