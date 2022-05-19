using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("钥匙")]
    public static bool takeKey = false;
    public GameObject keyPos;
    public GameObject keyObjectPrefab;
    private GameObject keyObject;

    [Header("道具")]
    public Transform throwPos;
    //public float attackRadius;//攻击范围，暂时先放着
    //public GameObject attackTarget;//攻击对象

    private Animator anim;

    [HideInInspector]
    public bool isDizzy = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        Debug.Log("throwpos:" + throwPos.position);
    }

    /// <summary>
    /// 当角色在眩晕时，无法行动，且会丢掉钥匙
    /// </summary>
    public void GetCurrentState()
    {
        AnimatorStateInfo stateinfo = anim.GetCurrentAnimatorStateInfo(0);
        if (stateinfo.IsName("Death"))
        {
            isDizzy = true;
            SoundManager.Instance.OnDizzyAudio();
            LoseKey();
        }
        else
        {
            isDizzy = false;
        }
    }

    public void LoseKey()
    {
        ItemManager.hasPlayerTakeKey = false;
        takeKey = false;
        if (keyObject != null)
        {
            keyObject.GetComponent<Key>().beThrowed();
        }
    }


    /// <summary>
    /// 获取玩家输入
    /// </summary>
    void Update()
    {
        GetCurrentState();
        //Vector3 dir = transform.forward;
        //Debug.Log("前进方向：" + dir);

        if (!isDizzy)
        {
            // Q：普通攻击
            //if (Input.GetKeyDown(KeyCode.Q))
            //{
            //    ItemManager.ThrowRock(throwPos);
            //}
            // 1：使用道具1
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ItemManager.useItem1(throwPos);
            }
            // 2：使用道具2
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ItemManager.useItem2(throwPos);
            }
            // 3：使用道具3
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                ItemManager.useItem3(throwPos);
            }
        }
        
    }

    //public void FindAttackTarget()
    //{

    //}

    

    private void OnTriggerEnter(Collider other)
    {
        // 碰到道具，拾取（显示在UI）
        if (other.gameObject.CompareTag("ItemBox"))
        {
            SoundManager.Instance.OnPickUpAudio();
            Debug.Log("拾取道具!!!!!!!!!!!!!!!!!");
            // 如果捡到的是钥匙
            ItemBox itemBox = other.gameObject.GetComponent<ItemBox>();
            if (itemBox.isKey())
            {
                Debug.Log("捡到钥匙啦!!!!!!!!!!!!!!!!!");
                ItemManager.hasPlayerTakeKey = true;
                takeKey = true;
                //keyObject = Instantiate(keyObjectPrefab, keyPos.position, Quaternion.identity);
                keyPos.SetActive(true);
                itemBox.DisplayInKeyPanel();
            }
            // 场景中道具数-1
            GameManager.curBoxNum -= 1;
            itemBox.DestorySelf();
        }
    }

}
