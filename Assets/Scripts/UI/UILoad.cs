using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILoad : MonoBehaviour//��������а�ť����¼����ø��ֺ�����һ����
{
    public void LoadUI(string name)//����ui
    {
        Controller.current.LoadUI(name);
    }
    public void ReturnFromUI()//��ui����
    {
        Controller.current.ReturnFromUI();
    }
    public void SwitchSence(string name)//�л�����
    {
        Controller.current.SwitchScene(name);
    }
    public void Quit()//�˳�
    {
        Application.Quit();
    }
}
