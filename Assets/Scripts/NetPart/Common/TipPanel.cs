using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipPanel : BasePanel
{
    //��ʾ�ı�
    private Text text;
    //ȷ����ť
    private Button okBtn;

    //��ʼ��
    public override void OnInit()
    {
        skinPath = "TipPanel";
        layer = PanelManager.Layer.Tip;
    }
    //��ʾ
    public override void OnShow(params object[] args)
    {
        //Ѱ�����
        text = skin.transform.Find("Text").GetComponent<Text>();
        okBtn = skin.transform.Find("OkBtn").GetComponent<Button>();
        //����
        okBtn.onClick.AddListener(OnOkClick);
        //��ʾ��
        if (args.Length == 1)
        {
            text.text = (string)args[0];
        }
    }

    //�ر�
    public override void OnClose()
    {

    }

    //������ȷ����ť
    public void OnOkClick()
    {
        if (text.text == "��������")
        {
            NetManager.Close();
            PanelManager.Open<OnlineorNotPanel>();
        }
        Close();
    }
}
