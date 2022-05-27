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
    //չʾ��ɫ��ť
    private Button CharacterShowButton;
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
        CharacterShowButton = skin.transform.Find("CharacterShowButton").GetComponent<Button>();
        SingleButton.onClick.AddListener(OnSingleClick);
        OnlineButton.onClick.AddListener(OnOnlineClick);
        CharacterShowButton.onClick.AddListener(OnCharacterShowButtonClick);
    }
    //�����ť֮��W
    public void OnSingleClick()
    {
        //��ʾ������UI
        Transform SceneUI = GameObject.Find("UI").transform.GetChild(0) ;
        SceneUI.gameObject.SetActive(true);

        GameMain.isOnline = false;
        GameMain.id = "y";
        ////����������������
        AnimalInfo animalInfo = new AnimalInfo();
        animalInfo.id = "y";
        animalInfo.camp =3;//��ͬcamp��ͬ����
        animalInfo.x = -4.636f;
        animalInfo.y = 20.241f;
        animalInfo.z = 10.2672f;
        BattleManager.GenerateAnimal(animalInfo);


        //���ڲ��� ���ɼ����������
        //AnimalInfo animalInfo2 = new AnimalInfo();
        //animalInfo2.id = "y2";
        //animalInfo2.camp = 5;//��ͬcamp��ͬ����
        //                     //animalInfo2.x = -28.37626f;
        //                     //animalInfo2.y = 1.264288f;
        //                     //animalInfo2.z = 15.03f;
        //animalInfo2.x = -4.636f;
        //animalInfo2.y = 20.241f;
        //animalInfo2.z = 10.2672f;
        //BattleManager.GenerateAnimal(animalInfo2);


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
    public void OnCharacterShowButtonClick()
    {
        PanelManager.Open<ShowCharacterPanel>();
    }
}