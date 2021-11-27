using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUI : MonoBehaviour//���ƶ�ui��ͷ��UI������
{
    protected float m_height=0.5f;//�������ƫ����
    public PlayerControl pc;//���
    public static GameObject Generate(Data.UIId id,GameObject player)//����һ��MoveUI
    {
        MoveUI newUI = Data.Generate("UIs/"+Data.uIIdToString[id],player).GetComponent<MoveUI>();
        newUI.pc = player.GetComponent<PlayerControl>();
        return newUI.gameObject;
    }
    public void Start()
    {
        transform.position = transform.position + new Vector3(0, m_height);//����λ��
    }
}
