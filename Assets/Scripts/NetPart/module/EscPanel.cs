using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EscPanel : BasePanel
{
    //确定按钮
    private Button EscBtn;
    
    //初始化
    public override void OnInit()
    {
        //设置皮肤地址skinPath和面板的层级layer
        skinPath = "EscPanel";
        layer = PanelManager.Layer.Panel;
    }

    //显示
    public override void OnShow(params object[] args)
    {
        //寻找组件
        EscBtn = skin.transform.Find("ESC_ExitGame").GetComponent<Button>();

        //监听:通过这种方式绑定监听,可以很好地进行预制体的移植.
        EscBtn.onClick.AddListener(OnEscClick);

        
    }

    //关闭
    public override void OnClose()
    {
        
    }
    //当按下注册按钮
    public void OnEscClick()
    {
        PanelManager.Open<ExitGamePanel>();
        
        //Close();
    }

   
}
