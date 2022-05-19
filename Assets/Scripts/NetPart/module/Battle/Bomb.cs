using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    //移动速度
    public float speed = 5f;
    public float upspeed = 7f;

    private Rigidbody rb;

    [Header("发射设置")]
    public float force = 8.0f;
    //public Vector3 velocity;
    public float angle=45;//仰角
    [Header("使用效果")]
    public GameObject expPrefab;
    //public float expForce;
    public float radius;
    [Header("效果时长")]
    public float effectTime = 3.0f;
    //private GameObject[] players;
    //private GameObject player;
    public enum BombStates { HitOtherPlayer, HitNothing };
    private BombStates bombStates;

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
        GameObject skinRes = ResManager.LoadPrefab("Bomb");
        skin = (GameObject)Instantiate(skinRes);
        Vector3 Firepos = new Vector3(0, 1.75f, 2.34f);
        skin.transform.parent = this.transform;
        skin.transform.localPosition = Firepos;
        skin.transform.localEulerAngles = Vector3.zero;
        //物理
        rigidBody = gameObject.AddComponent<Rigidbody>();
        //rigidBody.useGravity = false;

        Debug.Log("enterBomb");
        // 生成时，状态为攻击其他玩家 ??
        bombStates = BombStates.HitOtherPlayer;
        //players = GameObject.FindGameObjectsWithTag("Player"); //查找Player
        //player = players[0];

        // 仰角：45度
        //direction = animal.transform.forward.normalized + Mathf.Tan(angle) * Vector3.up;
        //Debug.Log("投掷方向：" + direction);
        //rb.AddForce(direction * force, ForceMode.Impulse);
        //BeThrowed();
    }
    void Update()
    {
        //向前移动
        transform.position += transform.forward * speed * Time.deltaTime;
        transform.position += transform.up * upspeed * Time.deltaTime;
        upspeed -= 0.05f;
    }
    //}

    /// <summary>
    /// 根据已知仰角和初始速度发射
    /// </summary>
    public void BeThrowed()
    {
        // 仰角：45度
        direction = animal.transform.forward.normalized + Mathf.Tan(angle) * Vector3.up;
        Debug.Log("投掷方向：" + direction);
        rb.AddForce(direction * force, ForceMode.Impulse);
    }
    //碰撞
    void OnCollisionEnter(Collision collisionInfo)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
         foreach (Collider nearby in colliders)
         {
            BaseAnimal syncAnimal = nearby.gameObject.GetComponent<BaseAnimal>();

            if (syncAnimal != animal&&syncAnimal != null)
             {
                
                SendMsgHit(animal, syncAnimal);
                Debug.Log("其他玩家眩晕！");
                 nearby.gameObject.GetComponent<Animator>().SetTrigger("BeBomb");
                
             }
         }
        //显示爆炸效果
        GameObject explode = ResManager.LoadPrefab("WFX_ExplosiveSmoke Big Alt");
        //if (hitanimal != null) 
        //collisionInfo.gameObject.GetComponent<Animator>().SetTrigger("BeBomb");//

        Vector3 Firepos = new Vector3(0, 1.75f, 2.34f);
        Instantiate(explode, transform.position + Firepos, transform.rotation);

        //摧毁自身
        Destroy(gameObject);

    }

    ////碰撞
    //void OnCollisionEnter(Collision collisionInfo)
    //{
    //    //打到的动物
    //    GameObject collObj = collisionInfo.gameObject;
    //    BaseAnimal hitanimal = collObj.GetComponent<BaseAnimal>();

    //    //不能打自己
    //    if (hitanimal == animal)
    //    {
    //        return;
    //    }
    //    //打到其他动物
    //    if (hitanimal != null)
    //    {
    //        SendMsgHit(animal, hitanimal);
    //        collisionInfo.gameObject.GetComponent<Animator>().SetTrigger("BeBomb");//
    //    }
    //    //显示爆炸效果
    //    GameObject explode = ResManager.LoadPrefab("WFX_ExplosiveSmoke Big Alt");
    //    //if (hitanimal != null) 
    //    //collisionInfo.gameObject.GetComponent<Animator>().SetTrigger("BeBomb");//

    //    Vector3 Firepos = new Vector3(0, 1.75f, 2.34f);
    //    Instantiate(explode, transform.position + Firepos, transform.rotation);

    //    //摧毁自身
    //    Destroy(gameObject);
    //}

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

        msg.Fireid = "BombAttack";
        NetManager.Send(msg);
    }
    //private void OnCollisionEnter(Collision other)
    //{


    //    switch (bombStates)
    //    {
    //        case BombStates.HitOtherPlayer:
    //            Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
    //            foreach (Collider nearby in colliders)
    //            {
    //                SyncAnimal syncAnimal = nearby.gameObject.GetComponent<SyncAnimal>();
    //                //if (nearby.gameObject.CompareTag("Player"))
    //                if (syncAnimal!=null)
    //                {
    //                    Debug.Log("其他玩家眩晕！");
    //                    nearby.gameObject.GetComponent<Animator>().SetTrigger("BeBomb");
    //                    bombStates = BombStates.HitNothing;
    //                }
    //            }
    //            break;
    //    }

    //    //显示爆炸效果
    //    GameObject explode = ResManager.LoadPrefab("WFX_Explosion");
    //    Vector3 Firepos = new Vector3(0, 1.75f, 2.34f);
    //    GameObject exp = Instantiate(explode, transform.position + Firepos, transform.rotation);
    //    Destroy(exp, effectTime);

    //    // 爆炸效果
    //    //GameObject exp = Instantiate(expPrefab, transform.position, Quaternion.identity);
    //    //Destroy(exp, effectTime);

    //    // 爆炸后消失
    //    Destroy(gameObject);
    //}

    ///// <summary>
    ///// 对周边刚体产生推力
    ///// </summary>
    //void KnockBack()
    //{
    //    Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

    //    int num = 0;
    //    foreach(Collider nearby in colliders)
    //    {
    //        num += 1;
    //        Debug.Log("检测到碰撞体" + num);
    //        Rigidbody rigg = nearby.GetComponent<Rigidbody>();
    //        if (rigg != null)
    //        {
    //            rigg.AddExplosionForce(expForce, transform.position, radius);
    //        }
    //    }
    //}
}
