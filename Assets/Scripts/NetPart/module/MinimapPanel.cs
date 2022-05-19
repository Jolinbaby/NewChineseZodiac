using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapPanel : BasePanel
{
    //初始化
    public override void OnInit()
    {
        //设置皮肤地址skinPath和面板的层级layer
        skinPath = "MinimapPanel";
        layer = PanelManager.Layer.Panel;
    }

}
