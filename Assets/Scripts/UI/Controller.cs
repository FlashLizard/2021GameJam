using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour//控制场景切换，暂停，设置ui的出现，场景间数据的传递
{
    static public Controller current;//获取当前脚本
    [SerializeField]
    public int fixedTime = 20, fixedPlayerNums = 4;
    public int time,playerNums;//倒计时与玩家数量
    [SerializeField]
    public Dictionary<Data.UIId, GameObject> uIs = new Dictionary<Data.UIId, GameObject>();//各种需要获得的UI，比如暂停菜单
    public List<int> scores = new List<int>();                                              //得分信息
    public Data.SceneId scene=Data.SceneId.main;                                            //当前场景
    private Stack<GameObject> m_uIStack=new Stack<GameObject>();                            //记录进入UI的顺序，方便按顺序回退UI
    private void Awake()
    {
        if (!current)                                                                       //保证只有一个Controller对象
        {
            current = this;
        }
        else Destroy(gameObject);
        for(Data.UIId i=0;i<Data.UIId.controlEnd;i++)                                       //初始化UI,后面要加载时再赋gameobject
        {
            uIs.Add(i, null);
        }
    }
    private void Start()
    {
        StartCoroutine(Data.scenes[scene].Load());                                          //第一次启用时，调用初始场景加载函数
    }
    public void Warning(string context)
    {
        Debug.Log(context);
    }                                                   //警告提示（未来有UI显示）
    public void SwitchScene(string name)                                                    //切换场景
    {
        Scene tryScene = Data.scenes[Data.stringToSceneId[name]];
        if(tryScene.Check())                                                                //尝试切换
        {
            Data.scenes[scene].Exit();                                                      //从当前场景退出
            DontDestroyOnLoad(gameObject);
            SceneManager.LoadScene(name);                                                   //加载下一个场景
            scene = Data.stringToSceneId[name];
            StartCoroutine(tryScene.Load());                                                //调用场景加载函数
        }
    }
    public void LoadUI(string name)
    {
        Data.UIId id = Data.stringToUIId[name];
        uIs[id].SetActive(true);
        m_uIStack.Push(uIs[id]);
        Time.timeScale = 0;
    }                                                       //ui加载
    public void ReturnFromUI()
    {
        if(m_uIStack.Count>0)
        {
            m_uIStack.Pop().SetActive(false);
        }
        Time.timeScale = 1;
    }                                                            //ui退出
    public void Reset()
    {
        time = fixedTime;
        playerNums = fixedPlayerNums;
        m_uIStack.Clear();
        scores.Clear();
        Time.timeScale = 1;
    }                                                                   //回到主界面时调用，把数据设为初值
    public void FindUI(Data.UIId id)                                                          //查找UI
    {
        uIs[id] = GameObject.Find(Data.uIIdToString[id]);
        Debug.Log(uIs[id]);
        uIs[id].SetActive(false);//查找完就false
    }
    public void SetScores(List<int> scores)                                                    //设置得分，游戏结束时调用
    {
        this.scores = scores;
    }
}
