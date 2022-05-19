using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomListPanel : BasePanel
{
    //�˺��ı�
    private Text idText;
    //ս���ı�
    private Text scoreText;
    //�������䰴ť
    private Button createButton;
    //ˢ���б�ť
    private Button reflashButton;
    //���뷿�䰴ť(��Ҫ������뷿���)
    private Button enterButton;
    //�б�����
    private Transform content;
    //��������
    private GameObject roomObj;

    //2022.5.8 ���뷿��ż��뷿�䰴ť
    private Button joinRoomButton;


    //��ʼ��
    public override void OnInit()
    {
        skinPath = "RoomListPanel";
        layer = PanelManager.Layer.Panel;
    }

    //��ʾ
    public override void OnShow(params object[] args)
    {
        //Ѱ�����
        idText = skin.transform.Find("Panel/RoomList/InfoPanel/IdText").GetComponent<Text>();
        scoreText = skin.transform.Find("Panel/RoomList/InfoPanel/ScoreText").GetComponent<Text>();
        createButton = skin.transform.Find("Panel/RoomList/CtrlPanel/CreateButton").GetComponent<Button>();
        reflashButton = skin.transform.Find("Panel/RoomList/CtrlPanel/RefleshButton").GetComponent<Button>();
        enterButton = skin.transform.Find("Panel/RoomList/CtrlPanel/GetInButton").GetComponent<Button>();
        content = skin.transform.Find("Panel/RoomList/ListPanel/Scroll View/Viewport/Content");
        roomObj = skin.transform.Find("Room").gameObject;
        
        
        //�ս����ʱ�򲻼����
        roomObj.SetActive(false);
        //��ʾid
        idText.text = GameMain.id;
        //��ť�¼�
        createButton.onClick.AddListener(OnCreateClick);
        reflashButton.onClick.AddListener(OnReflashClick);
        enterButton.onClick.AddListener(OnEnterClick);
        //Э�����
        NetManager.AddMsgListener("MsgGetAchieve", OnMsgGetAchieve);
        NetManager.AddMsgListener("MsgGetRoomList", OnMsgGetRoomList);
        NetManager.AddMsgListener("MsgCreateRoom", OnMsgCreateRoom);
        NetManager.AddMsgListener("MsgEnterRoom", OnMsgEnterRoom);
        //���Ͳ�ѯ
        MsgGetAchieve msgGetAchieve = new MsgGetAchieve();
        NetManager.Send(msgGetAchieve);
        MsgGetRoomList msgGetRoomList = new MsgGetRoomList();
        NetManager.Send(msgGetRoomList);
    }


    //�ر�
    public override void OnClose()
    {
        //Э�����
        NetManager.RemoveMsgListener("MsgGetAchieve", OnMsgGetAchieve);
        NetManager.RemoveMsgListener("MsgGetRoomList", OnMsgGetRoomList);
        NetManager.RemoveMsgListener("MsgCreateRoom", OnMsgCreateRoom);
        NetManager.RemoveMsgListener("MsgEnterRoom", OnMsgEnterRoom);
    }

    //�յ��ɼ���ѯЭ��
    public void OnMsgGetAchieve(MsgBase msgBase)
    {
        MsgGetAchieve msg = (MsgGetAchieve)msgBase;
        scoreText.text = msg.win + "ʤ " + msg.lost + "��";
    }

    //�յ������б�Э��
    public void OnMsgGetRoomList(MsgBase msgBase)
    {
        MsgGetRoomList msg = (MsgGetRoomList)msgBase;
        //��������б�
        for (int i = content.childCount - 1; i >= 0; i--)
        {
            GameObject o = content.GetChild(i).gameObject;
            Destroy(o);
        }
        //���������б�
        if (msg.rooms == null)
        {
            Debug.Log("no room!");
            return;
        }
        for (int i = 0; i < msg.rooms.Length; i++)
        {
            GenerateRoom(msg.rooms[i]); //�������䵥Ԫ
        }
    }

    //����һ�����䵥Ԫ
    public void GenerateRoom(RoomInfo roomInfo)
    {
        //��������
        GameObject o = Instantiate(roomObj);
        o.transform.SetParent(content,false);
        o.SetActive(true);
        //��ȡ���
        Transform trans = o.transform;
        Text idText = trans.Find("IdText").GetComponent<Text>();
        Text countText = trans.Find("CountText").GetComponent<Text>();
        Text statusText = trans.Find("StatusText").GetComponent<Text>();
        Button btn = trans.Find("JoinButton").GetComponent<Button>();
        //�����Ϣ
        idText.text = roomInfo.id.ToString();
        countText.text = roomInfo.count.ToString();
        if (roomInfo.status == 0)
        {
            statusText.text = "׼����";
        }
        else
        {
            statusText.text = "ս����";
        }
        //��ť�¼�
        btn.name = idText.text;
        btn.onClick.AddListener(delegate () {
            OnJoinClick(btn.name);
        });
    }
    
    //��������뷿�䰴ť,���뷿��ż��뷿��
    public void OnEnterClick()
    {
        PanelManager.Open<JoinRoomPanel>();
    }

    //���ˢ�°�ť
    public void OnReflashClick()
    {
        MsgGetRoomList msg = new MsgGetRoomList();
        NetManager.Send(msg);
    }

    //������뷿�䰴ť
    public void OnJoinClick(string idString)
    {
        MsgEnterRoom msg = new MsgEnterRoom();
        msg.id = int.Parse(idString);
        NetManager.Send(msg);
    }

    //�յ����뷿��Э��
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

    //����½����䰴ť
    public void OnCreateClick()
    {
        MsgCreateRoom msg = new MsgCreateRoom();
        NetManager.Send(msg);
    }

    //�յ��½�����Э��
    public void OnMsgCreateRoom(MsgBase msgBase)
    {
        MsgCreateRoom msg = (MsgCreateRoom)msgBase;
        //�ɹ���������
        if (msg.result == 0)
        {
            PanelManager.Open<TipPanel>("�����ɹ�");
            PanelManager.Open<RoomPanel>();
            Close();
        }
        //��������ʧ��
        else
        {
            PanelManager.Open<TipPanel>("��������ʧ��");
        }
    }
}
