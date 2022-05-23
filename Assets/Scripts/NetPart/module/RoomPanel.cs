using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomPanel : BasePanel
{
    //��ս��ť
    private Button startButton;
    //�˳���ť
    private Button closeButton;
    //�б�����
    private Transform content;
    //�����Ϣ����
    private GameObject playerObj;

    //��ʾ����
    private Text roomId;

    //��ʼ��
    public override void OnInit()
    {
        skinPath = "RoomPanel";
        layer = PanelManager.Layer.Panel;
    }

    //��ʾ
    public override void OnShow(params object[] args)
    {
        //Ѱ�����
        startButton = skin.transform.Find("CtrlPanel/FightReadyBtn").GetComponent<Button>();
        closeButton = skin.transform.Find("CtrlPanel/ExitBtn").GetComponent<Button>();
        content = skin.transform.Find("ListPanel/Scroll View/Viewport/Content");
        playerObj = skin.transform.Find("Player").gameObject;

        roomId = skin.transform.Find("roomName").GetComponent<Text>();
        //�����������Ϣ
        playerObj.SetActive(false);
        //��ť�¼�
        startButton.onClick.AddListener(OnStartClick);
        closeButton.onClick.AddListener(OnCloseClick);
        //Э�����
        NetManager.AddMsgListener("MsgGetRoomInfo", OnMsgGetRoomInfo);
        NetManager.AddMsgListener("MsgLeaveRoom", OnMsgLeaveRoom);
        NetManager.AddMsgListener("MsgStartBattle", OnMsgStartBattle);
        //���Ͳ�ѯ
        MsgGetRoomInfo msg = new MsgGetRoomInfo();
        NetManager.Send(msg);
    }

    //�ر�
    public override void OnClose()
    {
        //Э�����
        NetManager.RemoveMsgListener("MsgGetRoomInfo", OnMsgGetRoomInfo);
        NetManager.RemoveMsgListener("MsgLeaveRoom", OnMsgLeaveRoom);
        //NetManager.RemoveMsgListener("MsgStartBattle", OnMsgStartBattle);
    }

    //�յ�����б�Э��
    public void OnMsgGetRoomInfo(MsgBase msgBase)
    {
        MsgGetRoomInfo msg = (MsgGetRoomInfo)msgBase;
        roomId.text = "�����: "+msg.roomId.ToString();
        //�������б�
        for (int i = content.childCount - 1; i >= 0; i--)
        {
            GameObject o = content.GetChild(i).gameObject;
            Destroy(o);
        }
        //���������б�
        if (msg.players == null)
        {
            return;
        }
        for (int i = 0; i < msg.players.Length; i++)
        {
            GeneratePlayerInfo(msg.players[i]);
        }
    }

    //����һ�������Ϣ��Ԫ
    public void GeneratePlayerInfo(PlayerInfo playerInfo)
    {
        //��������
        GameObject o = Instantiate(playerObj);
        o.transform.SetParent(content,false); //����������ó�false�������bug
        o.SetActive(true);
        //��ȡ���
        Transform trans = o.transform;
        Text idText = trans.Find("Name").GetComponent<Text>();
        Text campText = trans.Find("Camp").GetComponent<Text>();
        Text scoreText = trans.Find("Status").GetComponent<Text>();
        //�����Ϣ
        idText.text = playerInfo.id;
        switch(playerInfo.camp)
        {
            case 1:
                campText.text = "��";
                break;
            case 2:
                campText.text = "ţ";
                break;
            case 3:
                campText.text = "��";
                break;
            case 4:
                campText.text = "��";
                break;
            case 5:
                campText.text = "��";
                break;
        }
        
        if (playerInfo.isOwner == 1)
        {
            campText.text = campText.text;
        }
        scoreText.text = playerInfo.win + "ʤ " + playerInfo.lost + "��";
    }

    //����˳���ť
    public void OnCloseClick()
    {
        MsgLeaveRoom msg = new MsgLeaveRoom();
        NetManager.Send(msg);
    }

    //�յ��˳�����Э��
    public void OnMsgLeaveRoom(MsgBase msgBase)
    {
        MsgLeaveRoom msg = (MsgLeaveRoom)msgBase;
        //�ɹ��˳�����
        if (msg.result == 0)
        {
            PanelManager.Open<TipPanel>("�˳�����");
            PanelManager.Open<RoomListPanel>();
            Close();
        }
        //�˳�����ʧ��
        else
        {
            PanelManager.Open<TipPanel>("�˳�����ʧ��");
        }
    }

    //�����ս��ť
    public void OnStartClick()
    {
        MsgStartBattle msg = new MsgStartBattle();
        NetManager.Send(msg);
    }

    //�յ���ս����
    public void OnMsgStartBattle(MsgBase msgBase)
    {
        Debug.Log("lululu");
        MsgStartBattle msg = (MsgStartBattle)msgBase;
        //��ս
        if (msg.result == 0)
        {
            //�رս���
            Close();
        }
        //��սʧ��
        else
        {
            PanelManager.Open<TipPanel>("��սʧ�ܣ����ٶ���Ҫ2����Ҳ��ܿ�ս�� ��ֻ�з������Կ�ʼս����");
        }
    }
}