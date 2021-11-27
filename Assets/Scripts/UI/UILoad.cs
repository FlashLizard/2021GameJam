using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILoad : MonoBehaviour//方便面板中按钮点击事件调用各种函数的一个类
{
    public void LoadUI(string name)//加载ui
    {
        Controller.current.LoadUI(name);
    }
    public void ReturnFromUI()//从ui返回
    {
        Controller.current.ReturnFromUI();
    }
    public void SwitchSence(string name)//切换界面
    {
        Controller.current.SwitchScene(name);
    }
    public void Quit()//退出
    {
        Application.Quit();
    }
}
