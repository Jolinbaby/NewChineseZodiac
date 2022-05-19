using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegisterPanel : BasePanel
{
    //�˺������
    private InputField idInput;
    //���������
    private InputField pwInput;
    //�ظ������
    private InputField repInput;
    //ע�ᰴť
    private Button regBtn;
    //�رհ�ť
    private Button closeBtn;


    //��ʼ��
    public override void OnInit()
    {
        skinPath = "RegisterPanel";
        layer = PanelManager.Layer.Panel;
    }

    //��ʾ
    public override void OnShow(params object[] args)
    {
        //Ѱ�����
        idInput = skin.transform.Find("IdInput").GetComponent<InputField>();
        pwInput = skin.transform.Find("PwInput").GetComponent<InputField>();
        repInput = skin.transform.Find("RepInput").GetComponent<InputField>();
        regBtn = skin.transform.Find("RegisterBtn").GetComponent<Button>();
        closeBtn = skin.transform.Find("CloseBtn").GetComponent<Button>();
        //����
        regBtn.onClick.AddListener(OnRegClick);
        closeBtn.onClick.AddListener(OnCloseClick);
        //����Э�����
        NetManager.AddMsgListener("MsgRegister", OnMsgRegister);
    }

    //�ر�
    public override void OnClose()
    {
        //����Э�����
        NetManager.RemoveMsgListener("MsgRegister", OnMsgRegister);
    }

    //������ע�ᰴť
    public void OnRegClick()
    {
        //�û�������Ϊ��
        if (idInput.text == "" || pwInput.text == "")
        {
            PanelManager.Open<TipPanel>("�û��������벻��Ϊ��");
            return;
        }
        //�������벻ͬ
        if (repInput.text != pwInput.text)
        {
            PanelManager.Open<TipPanel>("������������벻ͬ");
            return;
        }
        //����
        MsgRegister msgReg = new MsgRegister();
        msgReg.id = idInput.text;
        msgReg.pw = pwInput.text;
        NetManager.Send(msgReg);
    }

    //�յ�ע��Э��
    public void OnMsgRegister(MsgBase msgBase)
    {
        MsgRegister msg = (MsgRegister)msgBase;
        if (msg.result == 0)
        {
            Debug.Log("ע��ɹ�");
            //��ʾ
            PanelManager.Open<TipPanel>("ע��ɹ�");
            //�رս���
            Close();
        }
        else
        {
            //������ʱ�������,���п��������ڷǷ��ַ�������
            PanelManager.Open<TipPanel>("ע��ʧ��,�û����ظ�������Ƿ��ַ�");
        }
    }

    //�����¹رհ�ť
    public void OnCloseClick()
    {
        Close();
    }
}
