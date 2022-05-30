using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
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
    //摄像机
    private Transform aimCameraTransform;
    //能否达到东西
    public bool canHit;
    //目标点
    public Vector3 target;
    //只检测一次
    private bool flag;
    //石头生成位置
    private Vector3 Firepos;
    private Vector3 initPos;
    //初始化
    public void Init()
    {
        //皮肤
        GameObject skinRes = ResManager.LoadPrefab("RockToAttack");
        skin = (GameObject)Instantiate(skinRes);
        //Firepos = new Vector3(0.7f,1.1f,1.6f);
        Firepos = Vector3.zero;
        skin.transform.parent = this.transform;
        skin.transform.localPosition = Firepos;
        skin.transform.localEulerAngles = Vector3.zero;
        //物理
        rigidBody = gameObject.AddComponent<Rigidbody>();
        rigidBody.useGravity = false;
        //aimCameraTransform = animal.gameObject.transform.Find("PlayerAimCamera").gameObject.transform;
        canHit = true;
        //Shoot();
        //target = new Vector3(19.2f, 18f, -13f);//debug
        //Debug.DrawLine(transform.position, target, Color.red);//绘制一条红色的射线  起点-终点
        //initPos = transform.position + Firepos;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target , speed * Time.deltaTime);
        //+ (target-transform.position-Firepos)*0.5f
    }

    //碰撞
    void OnCollisionEnter(Collision collisionInfo)
    {
        Debug.Log("石头碰到碰撞体啦！！！！！！！！！！！！！！！！！！！！！！！！！！");
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
            if(GameMain.isOnline == false)
                collisionInfo.gameObject.GetComponent<Animator>().SetTrigger("Dizzy");//
        }
        //显示爆炸效果
        GameObject explode = ResManager.LoadPrefab("BulletFatExplosionFire");
        //if (hitanimal != null) 
        //collisionInfo.gameObject.GetComponent<Animator>().SetTrigger("Dizzy");//

        Vector3 Firepos = new Vector3(0f, 0f, 0f);
        GameObject exObj =  Instantiate(explode, transform.position+Firepos, explode.transform.rotation);

        //摧毁自身
        Destroy(exObj, 1f);
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
