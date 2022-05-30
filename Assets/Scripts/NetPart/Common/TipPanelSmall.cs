using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipPanelSmall : BasePanel
{
    //��ʾ�ı�
    private Text text;
    
    //��ʼ��
    public override void OnInit()
    {
        skinPath = "TipPanelSmall";
        layer = PanelManager.Layer.Tip;
    }
    //��ʾ
    public override void OnShow(params object[] args)
    {
        //Ѱ�����
        text = skin.transform.Find("Text").GetComponent<Text>();
        
        //��ʾ��
        if (args.Length == 1)
        {
            text.text = (string)args[0];
        }
        Invoke("hide", 1.5f);
    }

    void hide()
    {
        Close();
    }

    //�ر�
    public override void OnClose()
    {

    }

}
