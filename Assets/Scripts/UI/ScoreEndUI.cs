using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreEndUI : MonoBehaviour//�÷ֽ�����UI
{
    [SerializeField]
    private Text m_text;//ʤ������
    void Start()
    {
        int mx = 0,j=1,team=1;
        foreach(var i in Controller.current.scores)//�м����ӣ��ͼӼ����÷���
        {
            ScoreOptionUI temp= Data.Generate("UIs/" + Data.uIIdToString[Data.UIId.scoreOption], gameObject).GetComponent<ScoreOptionUI>();
            temp.SetScore(i);
            if(mx<i)//��ȡ�÷���߶���
            {
                mx = i;
                team = j;
            }
            j++;
        }
        m_text.text = "��ϲ"+team.ToString() + "��ʤ��";
    }

}
