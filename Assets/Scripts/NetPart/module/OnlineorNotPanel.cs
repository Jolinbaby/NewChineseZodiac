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
    //展示角色按钮
    private Button CharacterShowButton;
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
        CharacterShowButton = skin.transform.Find("CharacterShowButton").GetComponent<Button>();
        SingleButton.onClick.AddListener(OnSingleClick);
        OnlineButton.onClick.AddListener(OnOnlineClick);
        CharacterShowButton.onClick.AddListener(OnCharacterShowButtonClick);
    }
    //点击按钮之后W
    public void OnSingleClick()
    {
        //显示场景中UI
        Transform SceneUI = GameObject.Find("UI").transform.GetChild(0) ;
        SceneUI.gameObject.SetActive(true);

        GameMain.isOnline = false;
        GameMain.id = "y";
        ////单机功能启动这里
        AnimalInfo animalInfo = new AnimalInfo();
        animalInfo.id = "y";
        animalInfo.camp =3;//不同camp不同动物
        animalInfo.x = -4.636f;
        animalInfo.y = 20.241f;
        animalInfo.z = 10.2672f;
        BattleManager.GenerateAnimal(animalInfo);


        //用于测试 生成几个其他玩家
        //AnimalInfo animalInfo2 = new AnimalInfo();
        //animalInfo2.id = "y2";
        //animalInfo2.camp = 5;//不同camp不同动物
        //                     //animalInfo2.x = -28.37626f;
        //                     //animalInfo2.y = 1.264288f;
        //                     //animalInfo2.z = 15.03f;
        //animalInfo2.x = -4.636f;
        //animalInfo2.y = 20.241f;
        //animalInfo2.z = 10.2672f;
        //BattleManager.GenerateAnimal(animalInfo2);


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
    public void OnCharacterShowButtonClick()
    {
        PanelManager.Open<ShowCharacterPanel>();
    }
}