using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

static class ItemManager
{
    private static Image itemIcon;
    public static bool isThrowBomb;
    public static bool isAnimalGetKey;
    // 是否已经有钥匙生成
    public static bool hasKey;

    //[Header("道具Prefab")]
    //public static GameObject bombPrefab;

    public static GameObject playeranimal;///


    // 是否已经有角色拿到钥匙，要用网络传
    public static bool hasPlayerTakeKey;

   public static void useItem1(Transform throwPos)
    {
        itemIcon = GameObject.Find("ItemIcon_1").GetComponent<Image>();
        //如果有道具才能使用
        if (itemIcon.color.a == 1.0f)
        {
            ThrowItem(throwPos);
        }
        Image nextItemIcon = GameObject.Find("ItemIcon_2").GetComponent<Image>();
        Image thirdItemIcon = GameObject.Find("ItemIcon_3").GetComponent<Image>();
        // 如果下一个panel里有道具，移动：下一个panel的透明度为0
        if (nextItemIcon.color.a == 1.0f)
        {
            //如果第三个道具栏也有道具
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
        //如果有道具才能使用
        if (itemIcon.color.a == 1.0f)
        {
            ThrowItem(throwPos);
        }
        Image nextItemIcon = GameObject.Find("ItemIcon_3").GetComponent<Image>();
        // 如果下一个panel里有道具，移动：下一个panel的透明度为0
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
        //如果有道具才能使用
        if (itemIcon.color.a == 1.0f)
        {
            ThrowItem(throwPos);
        }
        itemIcon.color = new Color(255, 255, 255, 0.0f);
    }

    public static void ThrowRock(Transform throwPos)
    {
        Debug.Log("丢石头");
        // 生成rock
        GameObject rockPrefab = Resources.Load("Prefabs/Items/Rock", typeof(GameObject)) as GameObject;
        var rock = MonoBehaviour.Instantiate(rockPrefab, throwPos.position, Quaternion.identity);
        // 设置攻击目标：距离玩家某个范围内的角色？可能需要一个flag区分是否为角色。
        //FindAttackTarget();
        //rock.GetComponent<Rock>().target = attackTarget;
        //float distance = Vector3.Distance(attackTarget.transform.position, transform.position);
        //Debug.Log("与攻击对象间的距离：" + distance);
    }

    public static void ThrowItem(Transform throwPos)
    {
        switch (itemIcon.sprite.name)
        {
            case "bomb":
                Debug.Log("丢炸弹");
                // 生成bomb
                //GameObject bombPrefab = Resources.Load("Prefabs/Items/Bomb", typeof(GameObject)) as GameObject;
                //var bomb = MonoBehaviour.Instantiate(bombPrefab, throwPos.position, Quaternion.identity);
                isThrowBomb = true;
                break;
        }
    }
}
