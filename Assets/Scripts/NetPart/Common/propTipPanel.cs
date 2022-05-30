using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class propTipPanel : BasePanel
{
    //道具Title
    private Text title;
    //提示文本
    private Text text;
    //图片
    private Image icon;

    //初始化
    public override void OnInit()
    {
        skinPath = "PropsTipPanel";
        layer = PanelManager.Layer.Tip;
    }
    //显示
    public override void OnShow(params object[] args)
    {
        //寻找组件
        text = skin.transform.Find("use-text").GetComponent<Text>();
        title = skin.transform.Find("title-text").GetComponent<Text>();
        icon = skin.transform.Find("icon-image").GetComponent<Image>();

        //提示语
        if (args.Length == 3)
        {
            Debug.Log("替换文字");
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

    //关闭
    public override void OnClose()
    {

    }

}
