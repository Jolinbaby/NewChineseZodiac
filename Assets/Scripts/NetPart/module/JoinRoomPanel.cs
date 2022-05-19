using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoinRoomPanel : BasePanel
{
    //���뷿�䰴ť
    private Button joinButton;
    //�˳���ť
    private Button closeButton;
    //�����,������ֶ�����,����÷���
    private InputField roomNum;

    //private GameObject playerObj;

    //��ʼ��
    public override void OnInit()
    {
        skinPath = "JoinRoomPanel";
        layer = PanelManager.Layer.Panel;
    }

    //��ʾ
    public override void OnShow(params object[] args)
    {
        //Ѱ�����
        joinButton = skin.transform.Find("CreateOrJumpIntoRoom/FightReady").GetComponent<Button>();
        closeButton = skin.transform.Find("CreateOrJumpIntoRoom/Exit").GetComponent<Button>();
        roomNum = skin.transform.Find("CreateOrJumpIntoRoom/InputRoomNumber").GetComponent<InputField>();

        joinButton.onClick.AddListener(OnJoinClick);
        closeButton.onClick.AddListener(OnCloseClick);
        //Э�����
        NetManager.AddMsgListener("MsgEnterRoom", OnMsgEnterRoom);
        //���Ͳ�ѯ
        MsgGetRoomInfo msg = new MsgGetRoomInfo();
        NetManager.Send(msg);
    }

    public void OnMsgEnterRoom(MsgBase msgBase)
    {
        MsgEnterRoom msg = (MsgEnterRoom)msgBase;
        //�ɹ����뷿��
        if (msg.result == 0)
        {
            PanelManager.Open<RoomPanel>();
            Close();
        }
        //���뷿��ʧ��
        else
        {
            PanelManager.Open<TipPanel>("���뷿��ʧ��");
        }
    }

    //�ر�
    public override void OnClose()
    {
        //Э�����
        NetManager.RemoveMsgListener("MsgEnterRoom", OnMsgEnterRoom);
    }

    //������뷿�䰴ť֮��
    public void OnJoinClick()
    {
        string roomN = roomNum.text;
        int result = 0;
        if (int.TryParse(roomN, out result))
        {
            //��ʱת����Ľ��Ϊresult,�൱���������result�ŷ���
            MsgEnterRoom msg = new MsgEnterRoom();
            msg.id = result;
            NetManager.Send(msg);
        }
        else
        {
            PanelManager.Open<TipPanel>("����ķ���Ų��Ϸ�!");
        }
    }

    
    //����˳���ť
    public void OnCloseClick()
    {
        Close();
    }

   
    
}
