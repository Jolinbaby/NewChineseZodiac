using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

static class ItemManager
{
    private static Image itemIcon;

    public static bool isThrowBomb;
    public static bool isThrowInk;
    public static bool isShield;
    public static bool isSpeedUp;
    public static bool isJumpUp;
    public static bool isSuper;

    public static bool isAnimalGetKey;
    // �Ƿ��Ѿ���Կ������
    public static bool hasKey;

    //[Header("����Prefab")]
    //public static GameObject bombPrefab;

    public static GameObject playeranimal;///


    // �Ƿ��Ѿ��н�ɫ�õ�Կ�ף�Ҫ�����紫
    public static bool hasPlayerTakeKey;

   public static void useItem1(Transform throwPos)
    {
        itemIcon = GameObject.Find("ItemIcon_1").GetComponent<Image>();
        //����е��߲���ʹ��
        if (itemIcon.color.a == 1.0f)
        {
            ThrowItem(throwPos);
        }
        Image nextItemIcon = GameObject.Find("ItemIcon_2").GetComponent<Image>();
        Image thirdItemIcon = GameObject.Find("ItemIcon_3").GetComponent<Image>();
        // �����һ��panel���е��ߣ��ƶ�����һ��panel��͸����Ϊ0
        if (nextItemIcon.color.a == 1.0f)
        {
            //���������������Ҳ�е���
            if (thirdItemIcon.color.a == 1.0f)
            {
                thirdItemIcon.color = new Color(255, 255, 255, 0.0f);
                itemIcon.color = new Color(255, 255, 255, 1.0f);
                itemIcon.sprite = nextItemIcon.sprite;
                nextItemIcon.color = new Color(255, 255, 255, 1.0f);
                nextItemIcon.sprite = thirdItemIcon.sprite;

            }
            else
            {
                nextItemIcon.color = new Color(255, 255, 255, 0.0f);
                itemIcon.color = new Color(255, 255, 255, 1.0f);
                itemIcon.sprite = nextItemIcon.sprite;
            }
        }
        else
        {
            itemIcon.color = new Color(255, 255, 255, 0.0f);
        }
    }

    public static void useItem2(Transform throwPos)
    {
        itemIcon = GameObject.Find("ItemIcon_2").GetComponent<Image>();
        //����е��߲���ʹ��
        if (itemIcon.color.a == 1.0f)
        {
            ThrowItem(throwPos);
        }
        Image nextItemIcon = GameObject.Find("ItemIcon_3").GetComponent<Image>();
        // �����һ��panel���е��ߣ��ƶ�����һ��panel��͸����Ϊ0
        if (nextItemIcon.color.a == 1.0f)
        {
            nextItemIcon.color = new Color(255, 255, 255, 0.0f);
            itemIcon.color = new Color(255, 255, 255, 1.0f);
            itemIcon.sprite = nextItemIcon.sprite;
        }
        else
        {
            itemIcon.color = new Color(255, 255, 255, 0.0f);
        }
    }

    public static void useItem3(Transform throwPos)
    {
        itemIcon = GameObject.Find("ItemIcon_3").GetComponent<Image>();
        //����е��߲���ʹ��
        if (itemIcon.color.a == 1.0f)
        {
            ThrowItem(throwPos);
        }
        itemIcon.color = new Color(255, 255, 255, 0.0f);
    }

    public static void ThrowRock(Transform throwPos)
    {
        Debug.Log("��ʯͷ");
        // ����rock
        GameObject rockPrefab = Resources.Load("Prefabs/Items/Rock", typeof(GameObject)) as GameObject;
        var rock = MonoBehaviour.Instantiate(rockPrefab, throwPos.position, Quaternion.identity);
        // ���ù���Ŀ�꣺�������ĳ����Χ�ڵĽ�ɫ��������Ҫһ��flag�����Ƿ�Ϊ��ɫ��
        //FindAttackTarget();
        //rock.GetComponent<Rock>().target = attackTarget;
        //float distance = Vector3.Distance(attackTarget.transform.position, transform.position);
        //Debug.Log("�빥�������ľ��룺" + distance);
    }

    public static void ThrowItem(Transform throwPos)
    {
        switch (itemIcon.sprite.name)
        {
            case "bomb":
                Debug.Log("��ը��");
                // ����bomb
                //GameObject bombPrefab = Resources.Load("Prefabs/Items/Bomb", typeof(GameObject)) as GameObject;
                //var bomb = MonoBehaviour.Instantiate(bombPrefab, throwPos.position, Quaternion.identity);
                isThrowBomb = true;
                break;
            case "ink":
                Debug.Log("��ī֭");
                isThrowInk = true;
                break;
            case "shield":
                Debug.Log("����");
                isShield = true;
                break;
            case "baozi":
                Debug.Log("����");
                isSpeedUp = true;
                break;
            case "mooncake":
                Debug.Log("����");
                isJumpUp = true;
                break;
            case "star":
                Debug.Log("�޵�");
                isSuper = true;
                break;
        }
    }
}
