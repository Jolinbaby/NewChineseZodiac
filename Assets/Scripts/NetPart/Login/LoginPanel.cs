using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : BasePanel
{
    //�˺������
    private InputField idInput;
    //���������
    private InputField pwInput;
    //��½��ť
    private Button loginBtn;
    //ע�ᰴť
    private Button regBtn;

    //��ʼ��
    public override void OnInit()
    {
        //����Ƥ����ַskinPath�����Ĳ㼶layer
        skinPath = "LoginPanel";
        layer = PanelManager.Layer.Panel;
    }

    //��ʾ
    public override void OnShow(params object[] args)
    {
        //Ѱ�����
        idInput = skin.transform.Find("IdInput").GetComponent<InputField>();
        pwInput = skin.transform.Find("PwInput").GetComponent<InputField>();
        loginBtn = skin.transform.Find("LoginBtn").GetComponent<Button>();
        regBtn = skin.transform.Find("RegisterBtn").GetComponent<Button>();
        //����:ͨ�����ַ�ʽ�󶨼���,���Ժܺõؽ���Ԥ�������ֲ.
        loginBtn.onClick.AddListener(OnLoginClick);
        regBtn.onClick.AddListener(OnRegClick);
        //����Э�����
        NetManager.AddMsgListener("MsgLogin", OnMsgLogin);
        //�����¼�����
        NetManager.AddEventListener(NetManager.NetEvent.ConnectSucc, OnConnectSucc);
        NetManager.AddEventListener(NetManager.NetEvent.ConnectFail, OnConnectFail);
        //���ӷ�����
        NetManager.Connect("127.0.0.1", 8888); //����Ƿ������Ĺ�����ַ�Լ���Ӧ�Ķ˿ں�

        //172.20.10.2
        //NetManager.Connect("172.20.10.2", 8888); //����Ƿ������Ĺ�����ַ�Լ���Ӧ�Ķ˿ں�

        //NetManager.Connect("47.111.176.71", 8888); //����Ƿ������Ĺ�����ַ�Լ���Ӧ�Ķ˿ں�
    }

    //�ر�
    public override void OnClose()
    {
        //����Э�����
        NetManager.RemoveMsgListener("MsgLogin", OnMsgLogin);
        //�����¼�����,NetEvent���Լ������ö����,�����Ҫ�����¼��Ļ�Ҳ���Լӽ�ȥ
        NetManager.RemoveEventListener(NetManager.NetEvent.ConnectSucc, OnConnectSucc);
        NetManager.RemoveEventListener(NetManager.NetEvent.ConnectFail, OnConnectFail);
    }


    //���ӳɹ��ص�
    void OnConnectSucc(string err)
    {
        Debug.Log("OnConnectSucc");
    }

    //����ʧ�ܻص�
    void OnConnectFail(string err)
    {
        PanelManager.Open<TipPanel>(err);
    }



    //������ע�ᰴť
    public void OnRegClick()
    {
        PanelManager.Open<RegisterPanel>();
        //Close();
    }



    //�����µ�½��ť
    public void OnLoginClick()
    {
        //�û�������Ϊ��
        if (idInput.text == "" || pwInput.text == "")
        {
            PanelManager.Open<TipPanel>("�û��������벻��Ϊ��");
            return;
        }
        //����
        MsgLogin msgLogin = new MsgLogin(); //���﷢�͵�¼Э��,Э�鱾��Ҫд��Э������ļ��е���
        msgLogin.id = idInput.text;
        msgLogin.pw = pwInput.text;
        NetManager.Send(msgLogin);
    }

    //�յ���½Э��(����Ƿ���˻ش������ĵ�¼Э��,�ͻ��˽������е�����)
    public void OnMsgLogin(MsgBase msgBase)
    {
        MsgLogin msg = (MsgLogin)msgBase;
        if (msg.result == 0)
        {
            Debug.Log("��½�ɹ�");
            //����id
            GameMain.id = msg.id;
            //�򿪷����б����
            PanelManager.Open<RoomListPanel>();
            //�رս���
            Close();
        }
        else
        {
            PanelManager.Open<TipPanel>("��½ʧ��,�û������������");
        }
    }
}
