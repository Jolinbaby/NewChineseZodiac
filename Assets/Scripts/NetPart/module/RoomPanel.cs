using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomPanel : BasePanel
{
    //开战按钮
    private Button startButton;
    //退出按钮
    private Button closeButton;
    //列表容器
    private Transform content;
    //玩家信息物体
    private GameObject playerObj;

    //显示房间
    private Text roomId;

    //初始化
    public override void OnInit()
    {
        skinPath = "RoomPanel";
        layer = PanelManager.Layer.Panel;
    }

    //显示
    public override void OnShow(params object[] args)
    {
        //寻找组件
        startButton = skin.transform.Find("CtrlPanel/FightReadyBtn").GetComponent<Button>();
        closeButton = skin.transform.Find("CtrlPanel/ExitBtn").GetComponent<Button>();
        content = skin.transform.Find("ListPanel/Scroll View/Viewport/Content");
        playerObj = skin.transform.Find("Player").gameObject;

        roomId = skin.transform.Find("roomName").GetComponent<Text>();
        //不激活玩家信息
        playerObj.SetActive(false);
        //按钮事件
        startButton.onClick.AddListener(OnStartClick);
        closeButton.onClick.AddListener(OnCloseClick);
        //协议监听
        NetManager.AddMsgListener("MsgGetRoomInfo", OnMsgGetRoomInfo);
        NetManager.AddMsgListener("MsgLeaveRoom", OnMsgLeaveRoom);
        NetManager.AddMsgListener("MsgStartBattle", OnMsgStartBattle);
        //发送查询
        MsgGetRoomInfo msg = new MsgGetRoomInfo();
        NetManager.Send(msg);
    }

    //关闭
    public override void OnClose()
    {
        //协议监听
        NetManager.RemoveMsgListener("MsgGetRoomInfo", OnMsgGetRoomInfo);
        NetManager.RemoveMsgListener("MsgLeaveRoom", OnMsgLeaveRoom);
        NetManager.RemoveMsgListener("MsgStartBattle", OnMsgStartBattle);
    }

    //收到玩家列表协议
    public void OnMsgGetRoomInfo(MsgBase msgBase)
    {
        MsgGetRoomInfo msg = (MsgGetRoomInfo)msgBase;
        roomId.text = "房间号: "+msg.roomId.ToString();
        //清除玩家列表
        for (int i = content.childCount - 1; i >= 0; i--)
        {
            GameObject o = content.GetChild(i).gameObject;
            Destroy(o);
        }
        //重新生成列表
        if (msg.players == null)
        {
            return;
        }
        for (int i = 0; i < msg.players.Length; i++)
        {
            GeneratePlayerInfo(msg.players[i]);
        }
    }

    //创建一个玩家信息单元
    public void GeneratePlayerInfo(PlayerInfo playerInfo)
    {
        //创建物体
        GameObject o = Instantiate(playerObj);
        o.transform.SetParent(content,false); //这里务必设置成false否则会有bug
        o.SetActive(true);
        //获取组件
        Transform trans = o.transform;
        Text idText = trans.Find("Name").GetComponent<Text>();
        Text campText = trans.Find("Camp").GetComponent<Text>();
        Text scoreText = trans.Find("Status").GetComponent<Text>();
        //填充信息
        idText.text = playerInfo.id;
        switch(playerInfo.camp)
        {
            case 1:
                campText.text = "鸡";
                break;
            case 2:
                campText.text = "牛";
                break;
            case 3:
                campText.text = "蛇";
                break;
            case 4:
                campText.text = "虎";
                break;
            case 5:
                campText.text = "狗";
                break;
        }
        
        if (playerInfo.isOwner == 1)
        {
            campText.text = campText.text;
        }
        scoreText.text = playerInfo.win + "胜 " + playerInfo.lost + "负";
    }

    //点击退出按钮
    public void OnCloseClick()
    {
        MsgLeaveRoom msg = new MsgLeaveRoom();
        NetManager.Send(msg);
    }

    //收到退出房间协议
    public void OnMsgLeaveRoom(MsgBase msgBase)
    {
        MsgLeaveRoom msg = (MsgLeaveRoom)msgBase;
        //成功退出房间
        if (msg.result == 0)
        {
            PanelManager.Open<TipPanel>("退出房间");
            PanelManager.Open<RoomListPanel>();
            Close();
        }
        //退出房间失败
        else
        {
            PanelManager.Open<TipPanel>("退出房间失败");
        }
    }

    //点击开战按钮
    public void OnStartClick()
    {
        MsgStartBattle msg = new MsgStartBattle();
        NetManager.Send(msg);
    }

    //收到开战返回
    public void OnMsgStartBattle(MsgBase msgBase)
    {
        Debug.Log("lululu");
        MsgStartBattle msg = (MsgStartBattle)msgBase;
        //开战
        if (msg.result == 0)
        {
            //关闭界面
            Close();
        }
        //开战失败
        else
        {
            PanelManager.Open<TipPanel>("开战失败！只有队长可以开始战斗!");
        }
    }
}