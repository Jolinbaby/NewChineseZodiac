using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapPanel : BasePanel
{
    //��ʼ��
    public override void OnInit()
    {
        //����Ƥ����ַskinPath�����Ĳ㼶layer
        skinPath = "MinimapPanel";
        layer = PanelManager.Layer.Panel;
    }

}
