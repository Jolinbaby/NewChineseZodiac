using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipPanelSmall : BasePanel
{
    //提示文本
    private Text text;
    
    //初始化
    public override void OnInit()
    {
        skinPath = "TipPanelSmall";
        layer = PanelManager.Layer.Tip;
    }
    //显示
    public override void OnShow(params object[] args)
    {
        //寻找组件
        text = skin.transform.Find("Text").GetComponent<Text>();
        
        //提示语
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

    //关闭
    public override void OnClose()
    {

    }

}
