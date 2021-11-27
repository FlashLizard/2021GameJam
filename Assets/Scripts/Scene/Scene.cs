using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Scene
{
    public abstract bool Check();//进行切换判断
    public abstract IEnumerator Load();//进入时，一些初始化：获取设定界面什么的
    public virtual void Exit()
    {
        Debug.Log("LeaveScene");
    }
}
