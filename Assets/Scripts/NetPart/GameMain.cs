using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMain : MonoBehaviour
{
    //���繦����������
    //public static string id = "";

    //����������������
    public static string id = "y";

    // Use this for initialization
    void Start()
    {
        //���繦����������
        //�������
        //NetManager.AddEventListener(NetManager.NetEvent.Close, OnConnectClose);
        //NetManager.AddMsgListener("MsgKick", OnMsgKick);
        ////��ʼ��
        //PanelManager.Init();
        //BattleManager.Init();
        ////�򿪵�½���
        //PanelManager.Open<LoginPanel>();

        //����������������
        AnimalInfo animalInfo = new AnimalInfo();
        animalInfo.id = "y";
        animalInfo.camp = 2;//��ͬcamp��ͬ����
        animalInfo.x = -4.636f;
        animalInfo.y =20.241f;
        animalInfo.z = 10.2672f;
        BattleManager.GenerateAnimal(animalInfo);
    }


    // Update is called once per frame
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
