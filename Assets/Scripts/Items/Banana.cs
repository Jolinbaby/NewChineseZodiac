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
    public bool isSpeedUp;
    //减速大小
    public float cutSpeed;
    //效果是否开启
    public bool flag;

    public void Init()
    {
        flag = false;

        GameObject skinRes = ResManager.LoadPrefab("Banana");
        bananaObj = (GameObject)Instantiate(skinRes);
        bananaObj.transform.Find("BuffSpeedUP").gameObject.GetComponent<ParticleSystem>().Play();
        bananaObj.transform.parent = this.transform;
        bananaObj.transform.localPosition = Vector3.zero;

        Debug.Log("Init加速");
        Invoke("StartEffect", 3f);
    }
    public void StartEffect()
    {
        flag = true;
    }

    void Update()
    {
        
    }

    /// <summary>
    /// 不管是放这个香蕉皮的本人还是其他玩家，只要踩到就会滑倒
    /// 为了不让玩家在放的时候就被滑倒，设置一个初始时长
    /// 也就是香蕉皮刚放下去的3s内不会触发
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionEnter(Collision other)
    {
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
