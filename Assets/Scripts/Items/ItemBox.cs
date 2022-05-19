using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemBox : MonoBehaviour
{

    public enum ItemType { Key, Bomb, Ink, Landmine };

    public ItemType itemType;

    private GameObject itemUI;

    private GameObject keyUI;

    void Start()
    {
        // 生成时，随机确定自己的类别
        int typeNumber = Random.Range(1, 4);  //取1-3
        switch (typeNumber)
        {
            case 1:
                itemType = ItemType.Bomb;
                break;
            case 2:
                itemType = ItemType.Ink;
                break;
            case 3:
                itemType = ItemType.Landmine;
                break;
        }
        // 如果还没有生成钥匙过，那么有10%的几率是钥匙
        if (!ItemManager.hasKey)
        {
            // 有10%的概率生成钥匙
            int r = Random.Range(0, 10);
            if (r == 5)
            {
                addKey();
                Debug.Log("生成钥匙！位置在" + transform.position);
            }
        }
        FindLand();
    }
    public void addKey()
    {
        itemType = ItemType.Key;
        ItemManager.hasKey = true;
        //DisplayInKeyPanel();
    }

    public bool isKey()
    {
        if(itemType == ItemType.Key)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// 通过射线检测地面，确保生成在地面上
    /// </summary>
    public void FindLand()
    {
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hitInfo;
        float heightAboveGround = 1.0f;
        if(Physics.Raycast(ray, out hitInfo))
        {
            transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y + heightAboveGround, hitInfo.point.z);
            gameObject.SetActive(true);
        }
    }

    public void DestorySelf()
    {
        Debug.Log("道具类型为"+itemType);
        // 若道具栏未满
        if (FindItemUI())
        {
            SoundManager.Instance.OnPickUpAudio();
            DisplayInItemBar();
            Destroy(gameObject);
        }
    }

    // 检查道具栏是否已满，选择下一个panel
    private bool FindItemUI()
    {
        if(GameObject.Find("ItemIcon_1").GetComponent<Image>().color.a == 0.0f)
        {
            itemUI = GameObject.Find("ItemIcon_1");
            return true;
        }
        else if (GameObject.Find("ItemIcon_2").GetComponent<Image>().color.a == 0.0f)
        {
            itemUI = GameObject.Find("ItemIcon_2");
            return true;
        }
        else if (GameObject.Find("ItemIcon_3").GetComponent<Image>().color.a == 0.0f)
        {
            itemUI = GameObject.Find("ItemIcon_3");
            return true;
        }
        return false;
    }
    private void DisplayInItemBar()
    {
        Debug.Log("道具显示在UI里！！！！！！！！！！！！！！！！！！！！！！！！！！！！");
        string imgPath;
        switch (itemType)
        {
            case ItemType.Bomb:
                // 找到文件路径，赋予Panel
                //string imgPath = Application.dataPath + "Images/ItemsIcon/" + "bomb.png
                imgPath = "Images/ItemsIcon/" + "bomb";
                itemUI.GetComponent<Image>().color = new Color(255, 255, 255, 1.0f);
                itemUI.GetComponent<Image>().sprite = Resources.Load(imgPath, typeof(Sprite)) as Sprite;
                Debug.Log("是炸弹！！！！！！！！！！！！！！！！！！！！！！！！！！！！！");
                Debug.Log(imgPath);
                break;
            case ItemType.Ink:
                // 找到文件路径，赋予Panel
                //string imgPath = Application.dataPath + "Images/ItemsIcon/" + "bomb.png
                imgPath = "Images/ItemsIcon/" + "ink";
                itemUI.GetComponent<Image>().color = new Color(255, 255, 255, 1.0f);
                itemUI.GetComponent<Image>().sprite = Resources.Load(imgPath, typeof(Sprite)) as Sprite;
                Debug.Log("是墨汁！！！！！！！！！！！！！！！！！！！！！！！！！！！！！");
                Debug.Log(imgPath);
                break;
            case ItemType.Landmine:
                // 找到文件路径，赋予Panel
                //string imgPath = Application.dataPath + "Images/ItemsIcon/" + "bomb.png
                imgPath = "Images/ItemsIcon/" + "landmine";
                itemUI.GetComponent<Image>().color = new Color(255, 255, 255, 1.0f);
                itemUI.GetComponent<Image>().sprite = Resources.Load(imgPath, typeof(Sprite)) as Sprite;
                Debug.Log("是地雷！！！！！！！！！！！！！！！！！！！！！！！！！！！！！");
                Debug.Log(imgPath);
                break;
        }
    }

    public void DisplayInKeyPanel()
    {
        keyUI = GameObject.Find("KeyPanel");
        keyUI.GetComponent<Image>().color = new Color(255, 255, 255, 1.0f);
    }
}
