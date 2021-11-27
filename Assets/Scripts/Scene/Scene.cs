using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Scene
{
    public abstract bool Check();//�����л��ж�
    public abstract IEnumerator Load();//����ʱ��һЩ��ʼ������ȡ�趨����ʲô��
    public virtual void Exit()
    {
        Debug.Log("LeaveScene");
    }
}
