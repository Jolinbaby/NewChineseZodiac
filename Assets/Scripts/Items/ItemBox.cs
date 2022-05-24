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
        // ����ʱ�����ȷ���Լ������
        //int typeNumber = Random.Range(1, 4); //ȡ1-3
        int typeNumber = kind;
        //typeNumber = 8;//���Լ���buff
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
    /// <summary>
    /// ͨ�����߼����棬ȷ�������ڵ�����
    /// ͬʱ��ֹ�ص�
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
                    Debug.Log("������������: " + nearby.name);
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
                // ��������

                Random.InitState(10);//ָ��seed
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
    /// ��ֹ�����������ص�
    /// </summary>
    public void FindPos()
    {
        

    }
    public void DestorySelf()
    {
        Debug.Log("��������Ϊ" + itemType);
        Debug.Log("����idΪ" + itemId);
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
    }

    // ���������Ƿ�������ѡ����һ��panel
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
