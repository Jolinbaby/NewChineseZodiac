using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoinRoomPanel : BasePanel
{
    //加入房间按钮
    private Button joinButton;
    //退出按钮
    private Button closeButton;
    //房间号,由玩家手动输入,进入该房间
    private InputField roomNum;

    //private GameObject playerObj;

    //初始化
    public override void OnInit()
    {
        skinPath = "JoinRoomPanel";
        layer = PanelManager.Layer.Panel;
    }

    //显示
    public override void OnShow(params object[] args)
    {
        //寻找组件
        joinButton = skin.transform.Find("CreateOrJumpIntoRoom/FightReady").GetComponent<Button>();
        closeButton = skin.transform.Find("CreateOrJumpIntoRoom/Exit").GetComponent<Button>();
        roomNum = skin.transform.Find("CreateOrJumpIntoRoom/InputRoomNumber").GetComponent<InputField>();

        joinButton.onClick.AddListener(OnJoinClick);
        closeButton.onClick.AddListener(OnCloseClick);
        //协议监听
        NetManager.AddMsgListener("MsgEnterRoom", OnMsgEnterRoom);
        //发送查询
        MsgGetRoomInfo msg = new MsgGetRoomInfo();
        NetManager.Send(msg);
    }

    public void OnMsgEnterRoom(MsgBase msgBase)
    {
        MsgEnterRoom msg = (MsgEnterRoom)msgBase;
        //成功进入房间
        if (msg.result == 0)
        {
            PanelManager.Open<RoomPanel>();
            Close();
        }
        //进入房间失败
        else
        {
            PanelManager.Open<TipPanel>("进入房间失败");
        }
    }

    //关闭
    public override void OnClose()
    {
        //协议监听
        NetManager.RemoveMsgListener("MsgEnterRoom", OnMsgEnterRoom);
    }

    //点击加入房间按钮之后
    public void OnJoinClick()
    {
        string roomN = roomNum.text;
        int result = 0;
        if (int.TryParse(roomN, out result))
        {
            //此时转换后的结果为result,相当于申请加入result号房间
            MsgEnterRoom msg = new MsgEnterRoom();
            msg.id = result;
            NetManager.Send(msg);
        }
        else
        {
            PanelManager.Open<TipPanel>("输入的房间号不合法!");
        }
    }

    
    //点击退出按钮
    public void OnCloseClick()
    {
        Close();
    }

   
    
}
