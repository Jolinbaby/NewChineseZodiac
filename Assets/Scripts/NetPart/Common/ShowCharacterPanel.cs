using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowCharacterPanel : BasePanel
{
    private List<Button> btnList;
    private List<string> nameList;
    //�رհ�ť
    private Button closeBtn;
    private GameObject currentAnimal;
    //��ʼ��
    public override void OnInit()
    {
        //����Ƥ����ַskinPath�����Ĳ㼶layer
        skinPath = "ShowCharacterPanel";
        layer = PanelManager.Layer.Panel;
    }

    //��ʾ
    public override void OnShow(params object[] args)
    {
        nameList = new List<string>();
        nameList.Add("Snake");
        nameList.Add("Dog");
        nameList.Add("Tiger");
        nameList.Add("Rooster");
        nameList.Add("Bull");

        btnList = new List<Button>();
        for (int i = 1; i < 6; i++)
        {
            var btn = skin.transform.Find("Btn_" + i).GetComponent<Button>();
            btnList.Add(btn);

            btn.onClick.AddListener(() => {
                LoadAnimal(btn);
            });
        }

        closeBtn = skin.transform.Find("CloseBtn").GetComponent<Button>();
        closeBtn.onClick.AddListener(OnCloseClick);

        var container = GameObject.Find("AnimalModelContainter");
        var animal = container.transform.Find("SnakeCharacter");
        animal.gameObject.SetActive(true);
        currentAnimal = animal.gameObject;

    }

    private void LoadAnimal(Button btn)
    {
        foreach (Button btn2 in btnList)
        {
            var textRoot2 = btn2.gameObject.transform.Find("TextRoot");
            textRoot2.gameObject.SetActive(false);
        }

        if (currentAnimal != null && currentAnimal.activeSelf)
        {
            currentAnimal.gameObject.SetActive(false);
        }
        int index = btnList.IndexOf(btn);
        string name = nameList[index];
        //�ڳ����м��ض�Ӧ����
        //Debug.Log("AnimalModelContainter/" + name + "Character");
        var container = GameObject.Find("AnimalModelContainter");
        var animal = container.transform.Find(name + "Character");
        animal.gameObject.SetActive(true);

        //���Ҳ�����ֲ�����ʾ����
        var textRoot = btn.transform.Find("TextRoot");
        textRoot.gameObject.SetActive(true);
        currentAnimal = animal.gameObject;
    }

    //�ر�,��ʱӦ�ðѶ��ﶼsetActive(false)
    public override void OnClose()
    {
        var container = GameObject.Find("AnimalModelContainter");
        foreach (string name in nameList)
        {
            var animal = container.transform.Find(name + "Character");
            animal.gameObject.SetActive(false);
        }

        foreach (Button btn in btnList)
        {
            var textRoot = btn.transform.Find("TextRoot");
            textRoot.gameObject.SetActive(false);
        }
        currentAnimal = container.transform.Find("SnakeCharacter").gameObject;
    }
    public void OnCloseClick()
    {
        
        Close();
    }
}