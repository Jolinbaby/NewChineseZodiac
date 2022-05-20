using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OnlineorNotPanel : BasePanel
{
    //单机按钮
    private Button SingleButton;
    //联机按钮
    private Button OnlineButton;
    //初始化
    public override void OnInit()
    {
        skinPath = "OnlineorNotPanel";
        layer = PanelManager.Layer.Panel;
    }
    //显示
    public override void OnShow(params object[] args)
    {
        //寻找组件
        SingleButton = skin.transform.Find("Single").GetComponent<Button>();
        OnlineButton = skin.transform.Find("Online").GetComponent<Button>();
        SingleButton.onClick.AddListener(OnSingleClick);
        OnlineButton.onClick.AddListener(OnOnlineClick);
    }
    //点击按钮之后W
    public void OnSingleClick()
    {
        GameMain.isOnline = false;
        GameMain.id = "y";
        ////单机功能启动这里
        AnimalInfo animalInfo = new AnimalInfo();
        animalInfo.id = "y";
        animalInfo.camp = 2;//不同camp不同动物
        animalInfo.x = -4.636f;
        animalInfo.y = 20.241f;
        animalInfo.z = 10.2672f;
        BattleManager.GenerateAnimal(animalInfo);
        Close();
    }
    //点击按钮之后
    public void OnOnlineClick()
    {
        GameMain.isOnline = true;
        //打开登陆面板
        PanelManager.Open<LoginPanel>();
        Close();
    }
}