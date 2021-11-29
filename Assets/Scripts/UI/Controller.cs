using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour//���Ƴ����л�����ͣ������ui�ĳ��֣����������ݵĴ���
{
    static public Controller current;//��ȡ��ǰ�ű�
    [SerializeField]
    public int fixedTime = 20, fixedPlayerNums = 4;
    public int time,playerNums;//����ʱ���������
    [SerializeField]
    public Dictionary<Data.UIId, GameObject> uIs = new Dictionary<Data.UIId, GameObject>();//������Ҫ��õ�UI��������ͣ�˵�
    public List<int> scores = new List<int>();                                              //�÷���Ϣ
    public Data.SceneId scene=Data.SceneId.main;                                            //��ǰ����
    private Stack<GameObject> m_uIStack=new Stack<GameObject>();                            //��¼����UI��˳�򣬷��㰴˳�����UI
    private void Awake()
    {
        if (!current)                                                                       //��ֻ֤��һ��Controller����
        {
            current = this;
        }
        else Destroy(gameObject);
        for(Data.UIId i=0;i<Data.UIId.controlEnd;i++)                                       //��ʼ��UI,����Ҫ����ʱ�ٸ�gameobject
        {
            uIs.Add(i, null);
        }
    }
    private void Start()
    {
        StartCoroutine(Data.scenes[scene].Load());                                          //��һ������ʱ�����ó�ʼ�������غ���
    }
    public void Warning(string context)
    {
        Debug.Log(context);
    }                                                   //������ʾ��δ����UI��ʾ��
    public void SwitchScene(string name)                                                    //�л�����
    {
        Scene tryScene = Data.scenes[Data.stringToSceneId[name]];
        if(tryScene.Check())                                                                //�����л�
        {
            Data.scenes[scene].Exit();                                                      //�ӵ�ǰ�����˳�
            DontDestroyOnLoad(gameObject);
            SceneManager.LoadScene(name);                                                   //������һ������
            scene = Data.stringToSceneId[name];
            StartCoroutine(tryScene.Load());                                                //���ó������غ���
        }
    }
    public void LoadUI(string name)
    {
        Data.UIId id = Data.stringToUIId[name];
        uIs[id].SetActive(true);
        m_uIStack.Push(uIs[id]);
        Time.timeScale = 0;
    }                                                       //ui����
    public void ReturnFromUI()
    {
        if(m_uIStack.Count>0)
        {
            m_uIStack.Pop().SetActive(false);
        }
        Time.timeScale = 1;
    }                                                            //ui�˳�
    public void Reset()
    {
        time = fixedTime;
        playerNums = fixedPlayerNums;
        m_uIStack.Clear();
        scores.Clear();
        Time.timeScale = 1;
    }                                                                   //�ص�������ʱ���ã���������Ϊ��ֵ
    public void FindUI(Data.UIId id)                                                          //����UI
    {
        uIs[id] = GameObject.Find(Data.uIIdToString[id]);
        Debug.Log(uIs[id]);
        uIs[id].SetActive(false);//�������false
    }
    public void SetScores(List<int> scores)                                                    //���õ÷֣���Ϸ����ʱ����
    {
        this.scores = scores;
    }
}
