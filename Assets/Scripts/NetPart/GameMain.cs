using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMain : MonoBehaviour
{
    //网络功能启动这里
    public static string id = "";
    public static bool isOnline;


    void Start()
    {
        //网络功能启动这里
        //网络监听
        NetManager.AddEventListener(NetManager.NetEvent.Close, OnConnectClose);
        NetManager.AddMsgListener("MsgKick", OnMsgKick);
        //初始化
        PanelManager.Init();
        BattleManager.Init();
        //打开登陆面板
        PanelManager.Open<OnlineorNotPanel>();
    }

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
