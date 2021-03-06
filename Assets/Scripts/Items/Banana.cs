using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana : MonoBehaviour
{

    //使用者
    public BaseAnimal animal;
    //香蕉皮模型
    private GameObject bananaObj;
    //效果时长
    public float effectTime;
    //判断是否踩到
    //public bool isSpeedUp;
    //减速大小
    public float speed;
    public float upspeed;
    public float cutSpeed;
    //效果是否开启
    public bool flag;
    //是否掉在地上
    public bool isGrounded;
    //只要判断一次就好
    public int time;

    public void Init()
    {
        flag = false;
        time = 0;
        upspeed = 2f;
        speed = 5f;
        cutSpeed = 0.1f;
        GameObject skinRes = ResManager.LoadPrefab("Banana");
        bananaObj = (GameObject)Instantiate(skinRes);
        bananaObj.transform.parent = this.transform;
        Vector3 initPos = new Vector3(0,0f,0);
        //bananaObj.transform.localPosition = Vector3.zero;
        bananaObj.transform.localPosition = initPos;

        Debug.Log("Init香蕉皮");
    }

    void Update()
    {
        if (time == 0)
        {
            isGrounded = IsOnGround();
            // 下落
            if (!IsOnGround())
            {
                //Debug.Log("香蕉皮在下落！!!!!!!!!!!!!!!!!!!!!!!!!!");
                //transform.position -= transform.up * 2f * Time.deltaTime;
                //向前移动
                transform.position += transform.forward * speed * Time.deltaTime;
                transform.position += transform.up * upspeed * Time.deltaTime;
                upspeed -= cutSpeed;
            }
            else
            {
                //香蕉落在地上后才开始有效果
                flag = true;
            }
        }
    }

    private bool IsOnGround()
    {
        LayerMask groundLayer = LayerMask.GetMask("Default") | LayerMask.GetMask("Ground");
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), 0.47f, groundLayer))
        {
            time++;
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 不管是放这个香蕉皮的本人还是其他玩家，只要踩到就会滑倒
    /// 为了不让玩家在放的时候就被滑倒，设置一个初始时长
    /// 也就是香蕉皮刚放下去的3s内不会触发
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter");
        if (!flag)
        {
            return;
        }
        BaseAnimal baseAnimal = other.gameObject.GetComponent<BaseAnimal>();

        if (baseAnimal != null)
        {
            SendMsgHit(animal, baseAnimal);
            Debug.Log("有玩家踩到香蕉皮");
            if (GameMain.isOnline == false)
                other.gameObject.GetComponent<Animator>().SetTrigger("BeBanana");
        }
        Destroy(gameObject);
    }

    //发送伤害协议
    void SendMsgHit(BaseAnimal animal, BaseAnimal hitanimal)
    {

        if (hitanimal == null || animal == null)
        {
            return;
        }
        ////不是自己发出的炮弹/石头
        //if (animal.id != GameMain.id)
        //{
        //    return;
        //}
        MsgHit msg = new MsgHit();
        msg.targetId = hitanimal.id;
        msg.id = animal.id;
        msg.x = transform.position.x;
        msg.y = transform.position.y;
        msg.z = transform.position.z;

        msg.Fireid = "BananaAttack";
        NetManager.Send(msg);
    }
}
