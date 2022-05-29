using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Կ��")]
    public static bool takeKey = false;
    public GameObject keyPos;
    public GameObject keyObjectPrefab;
    private GameObject keyObject;

    [Header("����")]
    public Transform throwPos;
    //public float attackRadius;//������Χ����ʱ�ȷ���
    //public GameObject attackTarget;//��������

    private Animator anim;

    [HideInInspector]
    public bool isDizzy = false;

    void Start()
    {
        throwPos = gameObject.transform;
        anim = GetComponent<Animator>();
        Debug.Log("throwpos:" + throwPos.position);
    }

    /// <summary>
    /// ����ɫ��ѣ��ʱ���޷��ж����һᶪ��Կ��
    /// </summary>
    public void GetCurrentState()
    {
        AnimatorStateInfo stateinfo = anim.GetCurrentAnimatorStateInfo(0);
        if (stateinfo.IsName("Death")|| stateinfo.IsName("Death Long")|| stateinfo.IsName("Spin"))
        {
            // ֻ��ѣ��״̬�Ǵ�����״̬�л�������ʱ�򣬲ŻᲥ��
            if (isDizzy == false)
            {
                SoundManager.Instance.OnDizzyAudio();
            }
            isDizzy = true;
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
    /// ��ȡ�������
    /// </summary>
    void Update()
    {
        GetCurrentState();
        //Vector3 dir = transform.forward;
        //Debug.Log("ǰ������" + dir);

        if (!isDizzy)
        {
            // Q����ͨ����
            //if (Input.GetKeyDown(KeyCode.Q))
            //{
            //    ItemManager.ThrowRock(throwPos);
            //}
            // 1��ʹ�õ���1
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ItemManager.useItem1(throwPos);
            }
            // 2��ʹ�õ���2
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ItemManager.useItem2(throwPos);
            }
            // 3��ʹ�õ���3
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                ItemManager.useItem3(throwPos);
            }
            // 4��ʹ������buff
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                ItemManager.useBuff(throwPos);
            }
        }
        
    }

    //public void FindAttackTarget()
    //{

    //}

    

    private void OnTriggerEnter(Collider other)
    {
        // �������ߣ�ʰȡ����ʾ��UI��
        if (other.gameObject.CompareTag("ItemBox"))
        {
            Debug.Log("ʰȡ����!!!!!!!!!!!!!!!!!");
            // ����񵽵���Կ��
            ItemBox itemBox = other.gameObject.GetComponent<ItemBox>();
            if (itemBox.isKey())
            {
                Debug.Log("��Կ����!!!!!!!!!!!!!!!!!");

                //-------------------------------------------
                Debug.Log("�õ���Կ��");
                //CtrlAnimal ctrlAnimal = gameObject.GetComponent<CtrlAnimal>();
                //ctrlAnimal.isGetKey = true;
                BaseAnimal baseAnimal = gameObject.GetComponent<BaseAnimal>();
                baseAnimal.isGetKey = true;
                ItemManager.isAnimalGetKey= true;
                MsgKey msg = new MsgKey();
                //msg.id = this.id;
                NetManager.Send(msg);
                

                ItemManager.hasPlayerTakeKey = true;
                takeKey = true;
                //keyObject = Instantiate(keyObjectPrefab, keyPos.position, Quaternion.identity);
                //keyPos.SetActive(true);
                //itemBox.DisplayInKeyPanel();
                //-------------------------------------------
                //Destroy(itemBox);
            }
            // �����е�����-1
            GameManager.curBoxNum -= 1;
            itemBox.DestorySelf();
        }
    }

}
