using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMain : MonoBehaviour
{
    //网络功能启动这里
    //public static string id = "";

    //单机功能启动这里
    public static string id = "y";

    // Use this for initialization
    void Start()
    {
        //网络功能启动这里
        //网络监听
        //NetManager.AddEventListener(NetManager.NetEvent.Close, OnConnectClose);
        //NetManager.AddMsgListener("MsgKick", OnMsgKick);
        ////初始化
        //PanelManager.Init();
        //BattleManager.Init();
        ////打开登陆面板
        //PanelManager.Open<LoginPanel>();

        //单机功能启动这里
        AnimalInfo animalInfo = new AnimalInfo();
        animalInfo.id = "y";
        animalInfo.camp = 2;//不同camp不同动物
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

    //关闭连接
    void OnConnectClose(string err)
    {
        Debug.Log("断开连接");
    }

    //被踢下线
    void OnMsgKick(MsgBase msgBase)
    {
        PanelManager.Open<TipPanel>("被踢下线");
    }
}
