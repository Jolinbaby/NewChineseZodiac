using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ItemBox : MonoBehaviour
{

    public enum ItemType { Key, Bomb, Ink, Shield, SpeedUp, JumpUp, Super, Banana };

    public ItemType itemType;

    private string itemTitle;
    private string itemHint;
    private string itemPath;

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
        //kind = Random.Range(7, 9);
        int typeNumber = kind;

        //typeNumber = 5;//测试无敌同步

        itemId = id;
        switch (typeNumber)
        {
            case 1:
                itemType = ItemType.Bomb;
                itemTitle = "炸弹";
                itemHint = "炸晕范围内玩家，使其1.5s内无法行动";
                itemPath = "Images/ItemsIcon/bomb";
                break;
            case 2:
                itemType = ItemType.Ink;
                itemTitle = "墨汁";
                itemHint = "溅开墨汁，使范围内玩家5s内无法看清前方";
                itemPath = "Images/ItemsIcon/ink";
                break;
            case 3:
                itemType = ItemType.Shield;
                itemTitle = "护盾";
                itemHint = "抵御所有石子及单次道具攻击，5s后消失";
                itemPath = "Images/ItemsIcon/shield";
                break;
            case 4:
                ItemManager.hasKey = true;
                itemType = ItemType.Key;
                itemTitle = "钥匙";
                itemHint = "钥匙可以打开宝箱";
                itemPath = "Images/ItemsIcon/key";
                break;
            case 5:
                itemType = ItemType.SpeedUp;
                itemTitle = "包子";
                itemHint = "增加玩家移动速度3s";
                itemPath = "Images/ItemsIcon/baozi";
                break;
            case 6:
                itemType = ItemType.JumpUp;
                itemTitle = "月饼";
                itemHint = "增加玩家跳跃高度5s";
                itemPath = "Images/ItemsIcon/mooncake";
                break;
            case 7:
                itemType = ItemType.Super;
                itemTitle = "星星";
                itemHint = "进入无敌状态，5s内不受任何道具干扰";
                itemPath = "Images/ItemsIcon/star";
                break;
            case 8:
                itemType = ItemType.Banana;
                itemTitle = "香蕉";
                itemHint = "绊倒他人，使其2s内无法行动";
                itemPath = "Images/ItemsIcon/banana";
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

    public void FindLand()
    {
        // 防止其生成在房顶or树上
        LayerMask groundLayer = 1 << 3;
        LayerMask waterLayer = 1 << 4;
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hitInfo;
        float heightAboveGround = 1.8f;
        float heightAboveWater = 1.5f;
        // 发出的射线只与groundLayer产生碰撞
        if (Physics.Raycast(ray, out hitInfo, 30f, groundLayer))
        {
            transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y + heightAboveGround, hitInfo.point.z);
        }
        else
        {
            // 如果与水面产生碰撞
            if (Physics.Raycast(ray, out hitInfo, 30f, waterLayer))
            {
                transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y + heightAboveWater, hitInfo.point.z);
            }
        }
        // 防止卡在房子或其他碰撞体上
        Collider[] colliders = Physics.OverlapBox(gameObject.transform.position, new Vector3(transform.localScale.x/2, 0.01f, transform.localScale.z/2));
        if (colliders.Length > 0)
        {
            foreach (Collider nearby in colliders)
            {
                Debug.Log("道具箱碰到了: " + nearby.name);
            }
            hasOverlap = true;
            Debug.Log("道具箱销毁！ ");
            Destroy(gameObject);

            //给服务器发送报文

        }

    }

    public void DestorySelf()
    {
        Debug.Log("道具类型为" + itemType);
        Debug.Log("道具id为" + itemId);

        PanelManager.Open<propTipPanel>(itemHint,itemTitle,itemPath);
        
        // 互动道具
        if (itemType == ItemType.Bomb || itemType == ItemType.Ink || itemType == ItemType.Banana)
        {
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
            else
            {
                return;
            }
        }
        else if (itemType == ItemType.Shield || itemType == ItemType.SpeedUp || itemType == ItemType.JumpUp || itemType == ItemType.Super)
        {
            if (FindBuffUI())
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
            else
            {
                return;
            }
        }
        
    }

    // 检查互动道具栏是否已满，选择下一个panel
    private bool FindItemUI()
    {
        
        if (GameObject.Find("ItemIcon_1").GetComponent<Image>().color.a == 0.0f)
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
        Debug.Log("互动道具栏已满！");
        return false;
    }

    // 检查buff道具栏是否已满，选择下一个panel
    private bool FindBuffUI()
    {
        // 自身Buff
        if (GameObject.Find("ItemIcon_4").GetComponent<Image>().color.a == 0.0f)
        {
            itemUI = GameObject.Find("ItemIcon_4");
            return true;
        }
        Debug.Log("Buff道具栏已满！");
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
