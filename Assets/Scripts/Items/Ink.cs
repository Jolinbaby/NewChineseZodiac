using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ink : MonoBehaviour
{

    //移动速度
    public float speed = 5f;
    public float upspeed = 7f;

    public float radius;

    private Vector3 direction;
    //发射者
    public BaseAnimal animal;
    //炮弹模型
    private GameObject skin;

    //物理
    Rigidbody rigidBody;

    public void Init()
    {

        radius = 10f;
        //皮肤
        Debug.Log("load prefab");
        GameObject skinRes = ResManager.LoadPrefab("Ink");
        Debug.Log("prefab loaded");
        skin = (GameObject)Instantiate(skinRes);
        Vector3 Firepos = new Vector3(0, 1.75f, 2.34f);
        skin.transform.parent = this.transform;
        skin.transform.localPosition = Firepos;
        skin.transform.localEulerAngles = Vector3.zero;
        //物理
        rigidBody = gameObject.AddComponent<Rigidbody>();

    }
    void Update()
    {
        //向前移动
        transform.position += transform.forward * speed * Time.deltaTime;
        transform.position += transform.up * upspeed * Time.deltaTime;
        upspeed -= 0.05f;
    }

    //碰撞
    void OnCollisionEnter(Collision collisionInfo)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider nearby in colliders)
        {
            BaseAnimal syncAnimal = nearby.gameObject.GetComponent<BaseAnimal>();

            if (syncAnimal != animal && syncAnimal != null)
            {

                SendMsgHit(animal, syncAnimal);
                Debug.Log("其他玩家被泼墨！");
                if (GameMain.isOnline == false)
                    GameObject.Find("UI_Ink/Image").GetComponent<Animator>().SetTrigger("BeInk");
                
                //nearby.gameObject.GetComponent<Animator>().SetTrigger("BeInk");

            }
        }
        //显示爆炸效果
        GameObject explode = ResManager.LoadPrefab("Ink_Particle");
        //if (hitanimal != null) 
        //collisionInfo.gameObject.GetComponent<Animator>().SetTrigger("BeBomb");//

        Vector3 Firepos = new Vector3(0, 1.75f, 2.34f);
        Instantiate(explode, transform.position + Firepos, transform.rotation);

        //摧毁自身
        Destroy(gameObject);

    }

    //发送伤害协议
    void SendMsgHit(BaseAnimal animal, BaseAnimal hitanimal)
    {

        if (hitanimal == null || animal == null)
        {
            return;
        }
        //不是自己发出的炮弹/石头
        if (animal.id != GameMain.id)
        {
            return;
        }
        MsgHit msg = new MsgHit();
        msg.targetId = hitanimal.id;
        msg.id = animal.id;
        msg.x = transform.position.x;
        msg.y = transform.position.y;
        msg.z = transform.position.z;

        msg.Fireid = "InkAttack";
        NetManager.Send(msg);
    }
}
