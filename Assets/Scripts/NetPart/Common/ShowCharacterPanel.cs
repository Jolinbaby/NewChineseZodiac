using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowCharacterPanel : BasePanel
{
    private List<Button> btnList;
    private List<string> nameList;
    //关闭按钮
    private Button closeBtn;
    private GameObject currentAnimal;
    //初始化
    public override void OnInit()
    {
        //设置皮肤地址skinPath和面板的层级layer
        skinPath = "ShowCharacterPanel";
        layer = PanelManager.Layer.Panel;
    }

    //显示
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
        //在场景中加载对应动物
        //Debug.Log("AnimalModelContainter/" + name + "Character");
        var container = GameObject.Find("AnimalModelContainter");
        var animal = container.transform.Find(name + "Character");
        animal.gameObject.SetActive(true);

        //把右侧的文字部分显示出来
        var textRoot = btn.transform.Find("TextRoot");
        textRoot.gameObject.SetActive(true);
        currentAnimal = animal.gameObject;
    }

    //关闭,此时应该把动物都setActive(false)
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