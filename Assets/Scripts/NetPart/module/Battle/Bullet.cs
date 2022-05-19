using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //移动速度
    public float speed = 50f;
    //发射者
    public BaseAnimal animal;
    //炮弹模型
    private GameObject skin;
    //物理
    Rigidbody rigidBody;

    //初始化
    public void Init()
    {
        //皮肤
        GameObject skinRes = ResManager.LoadPrefab("RockToAttack");
        skin = (GameObject)Instantiate(skinRes);
        Vector3 Firepos = new Vector3(0,1.75f,2.34f);
        skin.transform.parent = this.transform;
        skin.transform.localPosition = Firepos;
        skin.transform.localEulerAngles = Vector3.zero;
        //物理
        rigidBody = gameObject.AddComponent<Rigidbody>();
        rigidBody.useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {
        //向前移动
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    //碰撞
    void OnCollisionEnter(Collision collisionInfo)
    {
        //打到的动物
        GameObject collObj = collisionInfo.gameObject;
        BaseAnimal hitanimal = collObj.GetComponent<BaseAnimal>();
        
        //不能打自己
        if (hitanimal == animal)
        {
            return;
        }
        //打到其他动物
        if (hitanimal != null)
        {
            SendMsgHit(animal, hitanimal);
            collisionInfo.gameObject.GetComponent<Animator>().SetTrigger("Dizzy");//
        }
        //显示爆炸效果
        GameObject explode = ResManager.LoadPrefab("WFX_Explosion");
        //if (hitanimal != null) 
        //collisionInfo.gameObject.GetComponent<Animator>().SetTrigger("Dizzy");//

        Vector3 Firepos = new Vector3(0, 1.75f, 2.34f);
        Instantiate(explode, transform.position+Firepos, transform.rotation);

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

        msg.Fireid = "QAttack";
        NetManager.Send(msg);
    }
}
