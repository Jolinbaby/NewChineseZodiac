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
        // ����ʱ�����ȷ���Լ������
        //int typeNumber = Random.Range(1, 4); //ȡ1-3
        //kind = Random.Range(7, 9);
        int typeNumber = kind;

        //typeNumber = 5;//�����޵�ͬ��

        itemId = id;
        switch (typeNumber)
        {
            case 1:
                itemType = ItemType.Bomb;
                itemTitle = "ը��";
                itemHint = "ը�η�Χ����ң�ʹ��1.5s���޷��ж�";
                itemPath = "Images/ItemsIcon/bomb";
                break;
            case 2:
                itemType = ItemType.Ink;
                itemTitle = "ī֭";
                itemHint = "����ī֭��ʹ��Χ�����5s���޷�����ǰ��";
                itemPath = "Images/ItemsIcon/ink";
                break;
            case 3:
                itemType = ItemType.Shield;
                itemTitle = "����";
                itemHint = "��������ʯ�Ӽ����ε��߹�����5s����ʧ";
                itemPath = "Images/ItemsIcon/shield";
                break;
            case 4:
                ItemManager.hasKey = true;
                itemType = ItemType.Key;
                itemTitle = "Կ��";
                itemHint = "Կ�׿��Դ򿪱���";
                itemPath = "Images/ItemsIcon/key";
                break;
            case 5:
                itemType = ItemType.SpeedUp;
                itemTitle = "����";
                itemHint = "��������ƶ��ٶ�3s";
                itemPath = "Images/ItemsIcon/baozi";
                break;
            case 6:
                itemType = ItemType.JumpUp;
                itemTitle = "�±�";
                itemHint = "���������Ծ�߶�5s";
                itemPath = "Images/ItemsIcon/mooncake";
                break;
            case 7:
                itemType = ItemType.Super;
                itemTitle = "����";
                itemHint = "�����޵�״̬��5s�ڲ����κε��߸���";
                itemPath = "Images/ItemsIcon/star";
                break;
            case 8:
                itemType = ItemType.Banana;
                itemTitle = "�㽶";
                itemHint = "�����ˣ�ʹ��2s���޷��ж�";
                itemPath = "Images/ItemsIcon/banana";
                break;

        }
        // �����û������Կ�׹�����ô��10%�ļ�����Կ��
        //if (!ItemManager.hasKey)
        //{
        // // ��10%�ĸ�������Կ��
        // int r = Random.Range(0, 10);
        // if (r == 5)
        // {
        // addKey();
        // Debug.Log("����Կ�ף�λ����" + transform.position);
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
        // ��ֹ�������ڷ���or����
        LayerMask groundLayer = 1 << 3;
        LayerMask waterLayer = 1 << 4;
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hitInfo;
        float heightAboveGround = 1.8f;
        float heightAboveWater = 1.5f;
        // ����������ֻ��groundLayer������ײ
        if (Physics.Raycast(ray, out hitInfo, 30f, groundLayer))
        {
            transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y + heightAboveGround, hitInfo.point.z);
        }
        else
        {
            // �����ˮ�������ײ
            if (Physics.Raycast(ray, out hitInfo, 30f, waterLayer))
            {
                transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y + heightAboveWater, hitInfo.point.z);
            }
        }
        // ��ֹ���ڷ��ӻ�������ײ����
        Collider[] colliders = Physics.OverlapBox(gameObject.transform.position, new Vector3(transform.localScale.x/2, 0.01f, transform.localScale.z/2));
        if (colliders.Length > 0)
        {
            foreach (Collider nearby in colliders)
            {
                Debug.Log("������������: " + nearby.name);
            }
            hasOverlap = true;
            Debug.Log("���������٣� ");
            Destroy(gameObject);

            //�����������ͱ���

        }

    }

    public void DestorySelf()
    {
        Debug.Log("��������Ϊ" + itemType);
        Debug.Log("����idΪ" + itemId);

        PanelManager.Open<propTipPanel>(itemHint,itemTitle,itemPath);
        
        // ��������
        if (itemType == ItemType.Bomb || itemType == ItemType.Ink || itemType == ItemType.Banana)
        {
            // ��������δ��
            if (FindItemUI())
            {
                SoundManager.Instance.OnPickUpAudio();
                //if(itemType!= ItemType.Bomb)//-----------------------------
                DisplayInItemBar();
                Destroy(gameObject);
                //�����˷��ͱ���,��ʾһ�µ�ǰ�����ĸ���Ʒ
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
                //�����˷��ͱ���,��ʾһ�µ�ǰ�����ĸ���Ʒ
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

    // ��黥���������Ƿ�������ѡ����һ��panel
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
        Debug.Log("����������������");
        return false;
    }

    // ���buff�������Ƿ�������ѡ����һ��panel
    private bool FindBuffUI()
    {
        // ����Buff
        if (GameObject.Find("ItemIcon_4").GetComponent<Image>().color.a == 0.0f)
        {
            itemUI = GameObject.Find("ItemIcon_4");
            return true;
        }
        Debug.Log("Buff������������");
        return false;
    }
    private void DisplayInItemBar()
    {
        Debug.Log("������ʾ��UI�������������������������������������������������������");
        string imgPath;
        switch (itemType)
        {
            case ItemType.Bomb:
                // �ҵ��ļ�·��������Panel
                //string imgPath = Application.dataPath + "Images/ItemsIcon/" + "bomb.png
                imgPath = "Images/ItemsIcon/" + "bomb";
                itemUI.GetComponent<Image>().color = new Color(255, 255, 255, 1.0f);
                itemUI.GetComponent<Image>().sprite = Resources.Load(imgPath, typeof(Sprite)) as Sprite;
                Debug.Log("��ը������������������������������������������������������������");
                Debug.Log(imgPath);
                break;
            case ItemType.Ink:
                // �ҵ��ļ�·��������Panel
                //string imgPath = Application.dataPath + "Images/ItemsIcon/" + "bomb.png
                imgPath = "Images/ItemsIcon/" + "ink";
                itemUI.GetComponent<Image>().color = new Color(255, 255, 255, 1.0f);
                itemUI.GetComponent<Image>().sprite = Resources.Load(imgPath, typeof(Sprite)) as Sprite;
                Debug.Log("��ī֭����������������������������������������������������������");
                Debug.Log(imgPath);
                break;
            case ItemType.Shield:
                // �ҵ��ļ�·��������Panel
                //string imgPath = Application.dataPath + "Images/ItemsIcon/" + "bomb.png
                imgPath = "Images/ItemsIcon/" + "shield";
                itemUI.GetComponent<Image>().color = new Color(255, 255, 255, 1.0f);
                itemUI.GetComponent<Image>().sprite = Resources.Load(imgPath, typeof(Sprite)) as Sprite;
                Debug.Log("�ǻ��ܣ���������������������������������������������������������");
                Debug.Log(imgPath);
                break;
            case ItemType.SpeedUp:
                // �ҵ��ļ�·��������Panel
                //string imgPath = Application.dataPath + "Images/ItemsIcon/" + "bomb.png
                imgPath = "Images/ItemsIcon/" + "baozi";
                itemUI.GetComponent<Image>().color = new Color(255, 255, 255, 1.0f);
                itemUI.GetComponent<Image>().sprite = Resources.Load(imgPath, typeof(Sprite)) as Sprite;
                Debug.Log("�ǰ��ӣ��ƶ��ٶ�++������������������������������������������������������������");
                Debug.Log(imgPath);
                break;
            case ItemType.JumpUp:
                // �ҵ��ļ�·��������Panel
                //string imgPath = Application.dataPath + "Images/ItemsIcon/" + "bomb.png
                imgPath = "Images/ItemsIcon/" + "mooncake";
                itemUI.GetComponent<Image>().color = new Color(255, 255, 255, 1.0f);
                itemUI.GetComponent<Image>().sprite = Resources.Load(imgPath, typeof(Sprite)) as Sprite;
                Debug.Log("���±�����Ծ�߶�++������������������������������������������������������������");
                Debug.Log(imgPath);
                break;
            case ItemType.Super:
                // �ҵ��ļ�·��������Panel
                //string imgPath = Application.dataPath + "Images/ItemsIcon/" + "bomb.png
                imgPath = "Images/ItemsIcon/" + "star";
                itemUI.GetComponent<Image>().color = new Color(255, 255, 255, 1.0f);
                itemUI.GetComponent<Image>().sprite = Resources.Load(imgPath, typeof(Sprite)) as Sprite;
                Debug.Log("���޵У���������������������������������������������������������");
                Debug.Log(imgPath);
                break;
            case ItemType.Banana:
                // �ҵ��ļ�·��������Panel
                //string imgPath = Application.dataPath + "Images/ItemsIcon/" + "bomb.png
                imgPath = "Images/ItemsIcon/" + "banana";
                itemUI.GetComponent<Image>().color = new Color(255, 255, 255, 1.0f);
                itemUI.GetComponent<Image>().sprite = Resources.Load(imgPath, typeof(Sprite)) as Sprite;
                Debug.Log("���㽶Ƥ����������������������������������������������������������");
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
