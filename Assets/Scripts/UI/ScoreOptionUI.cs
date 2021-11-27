using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreOptionUI : MonoBehaviour//Ω· ¯µ√∑÷¿∏ui
{
    [SerializeField]
    private Text m_text;
    public void SetScore(int score)
    {
        m_text.text = score.ToString();
    }
}
