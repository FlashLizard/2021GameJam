using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreControlUI : MonoBehaviour
{
    public static ScoreControlUI current;
    [SerializeField]
    public List<ScoreUI> scoreUIs = new List<ScoreUI>();//已在面板中赋值，获取得分ui请调用此字段（ScoreControlUI.current.scoreUIs[i])
    private void Awake()
    {
        current = this;
    }
}
