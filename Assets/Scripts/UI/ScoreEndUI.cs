using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreEndUI : MonoBehaviour//得分结束榜UI
{
    [SerializeField]
    private Text m_text;//胜利标题
    void Start()
    {
        int mx = 0,j=1,team=1;
        foreach(var i in Controller.current.scores)//有几个队，就加几个得分栏
        {
            ScoreOptionUI temp= Data.Generate("UIs/" + Data.uIIdToString[Data.UIId.scoreOption], gameObject).GetComponent<ScoreOptionUI>();
            temp.SetScore(i);
            if(mx<i)//获取得分最高队伍
            {
                mx = i;
                team = j;
            }
            j++;
        }
        m_text.text = "恭喜"+team.ToString() + "队胜利";
    }

}
