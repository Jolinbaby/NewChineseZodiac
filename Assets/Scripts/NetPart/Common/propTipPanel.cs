using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class propTipPanel : BasePanel
{
    //����Title
    private Text title;
    //��ʾ�ı�
    private Text text;
    //ͼƬ
    private Image icon;

    //��ʼ��
    public override void OnInit()
    {
        skinPath = "PropsTipPanel";
        layer = PanelManager.Layer.Tip;
    }
    //��ʾ
    public override void OnShow(params object[] args)
    {
        //Ѱ�����
        text = skin.transform.Find("use-text").GetComponent<Text>();
        title = skin.transform.Find("title-text").GetComponent<Text>();
        icon = skin.transform.Find("icon-image").GetComponent<Image>();

        //��ʾ��
        if (args.Length == 3)
        {
            Debug.Log("�滻����");
            text.text = (string)args[0];
            title.text = (string)args[1];
            icon.sprite = Resources.Load((string)args[2], typeof(Sprite)) as Sprite;
        }
        Invoke("hide", 2f);
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
