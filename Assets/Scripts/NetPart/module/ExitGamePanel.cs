using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ExitGamePanel: BasePanel
{
    //确定按钮
    private Button okBtn;

    private Button CancleBtn;

    //初始化
    public override void OnInit()
    {
        //设置皮肤地址skinPath和面板的层级layer
        skinPath = "ExitGamePanel";
        layer = PanelManager.Layer.Panel;
    }

    //显示
    public override void OnShow(params object[] args)
    {
        //寻找组件
        okBtn = skin.transform.Find("okBtn").GetComponent<Button>();
        CancleBtn  = skin.transform.Find("CancleBtn").GetComponent<Button>();

        //监听:通过这种方式绑定监听,可以很好地进行预制体的移植.
        okBtn.onClick.AddListener(OnOkEscClick);
        CancleBtn.onClick.AddListener(OnCancleClick);

    }

    //关闭
    public override void OnClose()
    {

    }
    public void OnCancleClick()
    {
        Close();
    }

    public void OnOkEscClick()
    {
        Debug.Log("点击退出-----------------");
        NetManager.Close();
        //        //退出游戏
        //        //预处理
#if UNITY_EDITOR    //在编辑器模式下
        EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif

    }






}
