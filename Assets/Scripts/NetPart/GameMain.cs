using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMain : MonoBehaviour
{
    //���繦����������
    public static string id = "";
    public static bool isOnline;


    void Start()
    {
        //���繦����������
        //�������
        NetManager.AddEventListener(NetManager.NetEvent.Close, OnConnectClose);
        NetManager.AddMsgListener("MsgKick", OnMsgKick);
        //��ʼ��
        PanelManager.Init();
        BattleManager.Init();
        //�򿪵�½���
        PanelManager.Open<OnlineorNotPanel>();
    }

    void Update()
    {
        NetManager.Update();
    }

    //�ر�����
    void OnConnectClose(string err)
    {
        Debug.Log("�Ͽ�����");
    }

    //��������
    void OnMsgKick(MsgBase msgBase)
    {
        PanelManager.Open<TipPanel>("��������");
    }
}
