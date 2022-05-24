using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ItemBox : MonoBehaviour
{

    public enum ItemType { Key, Bomb, Ink, Shield, SpeedUp, JumpUp, Super, Banana };

    public ItemType itemType;

    private GameObject itemUI;

    private GameObject keyUI;

    public int itemId;

    public bool hasOverlap;

    void Start()
    {
        //InitInfo(4, 1);
        hasOverlap = true;
    }

    public void InitInfo(int kind,int id)
    {
        // 生成时，随机确定自己的类别
        //int typeNumber = Random.Range(1, 4); //取1-3
        int typeNumber = kind;
        //typeNumber = 8;//测试加速buff
        itemId = id;
        switch (typeNumber)
        {
            case 1:
                itemType = ItemType.Bomb;
                break;
            case 2:
                itemType = ItemType.Ink;
                break;
            case 3:
                itemType = ItemType.Shield;
                break;
            case 4:
                ItemManager.hasKey = true;
                itemType = ItemType.Key;
                break;
            case 5:
                itemType = ItemType.SpeedUp;
                break;
            case 6:
                itemType = ItemType.JumpUp;
                break;
            case 7:
                itemType = ItemType.Super;
                break;
            case 8:
                itemType = ItemType.Banana;
                break;

        }
        // 如果还没有生成钥匙过，那么有10%的几率是钥匙
        //if (!ItemManager.hasKey)
        //{
        // // 有10%的概率生成钥匙
        // int r = Random.Range(0, 10);
        // if (r == 5)
        // {
        // addKey();
        // Debug.Log("生成钥匙！位置在" + transform.position);
        // }
        //}
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
    /// 同时防止重叠
    /// </summary>
    public void FindLand()
    {
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hitInfo;
        float heightAboveGround = 1.0f;
        float radius = 5.0f;
        bool flag = true;
        if (Physics.Raycast(ray, out hitInfo))
        {
            transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y + heightAboveGround, hitInfo.point.z);
        }
        while (flag)
        {
            hasOverlap = false;
            Collider[] colliders = Physics.OverlapBox(gameObject.transform.position, new Vector3(transform.localScale.x, 0.01f, transform.localScale.y));
            if (colliders.Length > 0)
            {
                foreach(Collider nearby in colliders)
                {
                    Debug.Log("道具箱碰到了: " + nearby.name);
                }
                hasOverlap = true;
            }
            //foreach (Collider nearby in colliders)
            //{
            //    if (nearby.gameObject.)
            //    {
            //        hasOverlap = true;
            //    }
            //}
            if (!hasOverlap)
            {
                flag = false;
            }
            else
            {
                // 重新生成

                Random.InitState(10);//指定seed
                int x = Random.Range((int)GameManager.Instance.minX,(int)GameManager.Instance.maxX);
                int z = Random.Range((int)GameManager.Instance.minZ, (int)GameManager.Instance.maxZ);
                transform.position = new Vector3((float)x, 24f, (float)z);
                ray = new Ray(transform.position, -transform.up);

                if (Physics.Raycast(ray, out hitInfo))
                {
                    
                    transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y + heightAboveGround, hitInfo.point.z);
                    
                }
            }
        }
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 防止道具箱生成重叠
    /// </summary>
    public void FindPos()
    {
        

    }
    public void DestorySelf()
    {
        Debug.Log("道具类型为" + itemType);
        Debug.Log("道具id为" + itemId);
        // 若道具栏未满
        if (FindItemUI())
        {
            SoundManager.Instance.OnPickUpAudio();
            //if(itemType!= ItemType.Bomb)//-----------------------------
                DisplayInItemBar();
            Destroy(gameObject);
            //向服务端发送报文,表示一下当前捡到了哪个物品
            MsgPickup msg = new MsgPickup();
            msg.itemid = itemId;
            NetManager.Send(msg);
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
            case ItemType.Shield:
                // 找到文件路径，赋予Panel
                //string imgPath = Application.dataPath + "Images/ItemsIcon/" + "bomb.png
                imgPath = "Images/ItemsIcon/" + "shield";
                itemUI.GetComponent<Image>().color = new Color(255, 255, 255, 1.0f);
                itemUI.GetComponent<Image>().sprite = Resources.Load(imgPath, typeof(Sprite)) as Sprite;
                Debug.Log("是护盾！！！！！！！！！！！！！！！！！！！！！！！！！！！！！");
                Debug.Log(imgPath);
                break;
            case ItemType.SpeedUp:
                // 找到文件路径，赋予Panel
                //string imgPath = Application.dataPath + "Images/ItemsIcon/" + "bomb.png
                imgPath = "Images/ItemsIcon/" + "baozi";
                itemUI.GetComponent<Image>().color = new Color(255, 255, 255, 1.0f);
                itemUI.GetComponent<Image>().sprite = Resources.Load(imgPath, typeof(Sprite)) as Sprite;
                Debug.Log("是包子（移动速度++）！！！！！！！！！！！！！！！！！！！！！！！！！！！！！");
                Debug.Log(imgPath);
                break;
            case ItemType.JumpUp:
                // 找到文件路径，赋予Panel
                //string imgPath = Application.dataPath + "Images/ItemsIcon/" + "bomb.png
                imgPath = "Images/ItemsIcon/" + "mooncake";
                itemUI.GetComponent<Image>().color = new Color(255, 255, 255, 1.0f);
                itemUI.GetComponent<Image>().sprite = Resources.Load(imgPath, typeof(Sprite)) as Sprite;
                Debug.Log("是月饼（跳跃高度++）！！！！！！！！！！！！！！！！！！！！！！！！！！！！！");
                Debug.Log(imgPath);
                break;
            case ItemType.Super:
                // 找到文件路径，赋予Panel
                //string imgPath = Application.dataPath + "Images/ItemsIcon/" + "bomb.png
                imgPath = "Images/ItemsIcon/" + "star";
                itemUI.GetComponent<Image>().color = new Color(255, 255, 255, 1.0f);
                itemUI.GetComponent<Image>().sprite = Resources.Load(imgPath, typeof(Sprite)) as Sprite;
                Debug.Log("是无敌！！！！！！！！！！！！！！！！！！！！！！！！！！！！！");
                Debug.Log(imgPath);
                break;
            case ItemType.Banana:
                // 找到文件路径，赋予Panel
                //string imgPath = Application.dataPath + "Images/ItemsIcon/" + "bomb.png
                imgPath = "Images/ItemsIcon/" + "banana";
                itemUI.GetComponent<Image>().color = new Color(255, 255, 255, 1.0f);
                itemUI.GetComponent<Image>().sprite = Resources.Load(imgPath, typeof(Sprite)) as Sprite;
                Debug.Log("是香蕉皮！！！！！！！！！！！！！！！！！！！！！！！！！！！！！");
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
