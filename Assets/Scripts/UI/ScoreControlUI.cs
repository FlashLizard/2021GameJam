using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreControlUI : MonoBehaviour
{
    public static ScoreControlUI current;
    [SerializeField]
    public List<ScoreUI> scoreUIs = new List<ScoreUI>();//��������и�ֵ����ȡ�÷�ui����ô��ֶΣ�ScoreControlUI.current.scoreUIs[i])
    private void Awake()
    {
        current = this;
    }
}
