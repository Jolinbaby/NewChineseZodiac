using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OnlineorNotPanel : BasePanel
{
    //������ť
    private Button SingleButton;
    //������ť
    private Button OnlineButton;
    //��ʼ��
    public override void OnInit()
    {
        skinPath = "OnlineorNotPanel";
        layer = PanelManager.Layer.Panel;
    }
    //��ʾ
    public override void OnShow(params object[] args)
    {
        //Ѱ�����
        SingleButton = skin.transform.Find("Single").GetComponent<Button>();
        OnlineButton = skin.transform.Find("Online").GetComponent<Button>();
        SingleButton.onClick.AddListener(OnSingleClick);
        OnlineButton.onClick.AddListener(OnOnlineClick);
    }
    //�����ť֮��W
    public void OnSingleClick()
    {
        GameMain.isOnline = false;
        GameMain.id = "y";
        ////����������������
        AnimalInfo animalInfo = new AnimalInfo();
        animalInfo.id = "y";
        animalInfo.camp = 2;//��ͬcamp��ͬ����
        animalInfo.x = -4.636f;
        animalInfo.y = 20.241f;
        animalInfo.z = 10.2672f;
        BattleManager.GenerateAnimal(animalInfo);
        Close();
    }
    //�����ť֮��
    public void OnOnlineClick()
    {
        GameMain.isOnline = true;
        //�򿪵�½���
        PanelManager.Open<LoginPanel>();
        Close();
    }
}