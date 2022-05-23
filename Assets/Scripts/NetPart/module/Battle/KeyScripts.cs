using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScripts : MonoBehaviour
{
    //移动速度
    public float speed = 5f;
    public float upspeed = 15f;

    private Rigidbody rb;

    [Header("发射设置")]
    public float force = 8.0f;
    //public Vector3 velocity;
    public float angle = 45;//仰角
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
    Vector3 vectorfor = new Vector3(1,0,0);
    Vector3 vectorup = new Vector3(0, 1, 0);
    //物理
    Rigidbody rigidBody;
    public bool iscol;
    public void Init()
    {
        Debug.Log("kEYSCRIPT INIT");
        radius = 10f;
        //皮肤
        GameObject skinRes = ResManager.LoadPrefab("Key");
        skin = (GameObject)Instantiate(skinRes);
        Vector3 Firepos = new Vector3(0, 1.75f, 2.34f);
        skin.transform.parent = this.transform;
        skin.transform.localPosition = Firepos;
        skin.transform.localEulerAngles = Vector3.zero;
        //物理
        rigidBody = gameObject.AddComponent<Rigidbody>();
        rigidBody.useGravity = false;

       // skin.gameObject.AddComponent<>

    }
    private void Start()
    {
        //物理
        rigidBody = gameObject.AddComponent<Rigidbody>();
        rigidBody.useGravity = false;
    }
    void Update()
    {
        //向前移动
        if (iscol == false)
        {
            //transform.position += transform.forward * speed * Time.deltaTime;
            //transform.position += transform.up * upspeed * Time.deltaTime;
            transform.position += vectorfor * speed * Time.deltaTime;
            transform.position += vectorup * upspeed * Time.deltaTime;
            upspeed -= 0.5f;
        }
    }

    //碰撞
    //void OnCollisionEnter(Collision collisionInfo)
    //{
    //    //打到的动物
    //    GameObject collObj = collisionInfo.gameObject;
    //    BaseAnimal hitanimal = collObj.GetComponent<BaseAnimal>();
    //    speed = 0f;
    //    upspeed = 0f;
    //    if (hitanimal == animal)
    //    {
    //        MsgKey msg = new MsgKey();
    //        NetManager.Send(msg);
    //        Destroy(gameObject);
    //    }


    //    return;
    //}
    private void OnTriggerEnter(Collider other)
    {
        rigidBody.constraints = RigidbodyConstraints.FreezePositionX;
        rigidBody.constraints = RigidbodyConstraints.FreezePositionY;
        rigidBody.constraints = RigidbodyConstraints.FreezePositionZ;
        //打到的动物
        GameObject collObj = other.gameObject;
        BaseAnimal hitanimal = collObj.GetComponent<BaseAnimal>();
        CtrlAnimal ctrlanimal = collObj.gameObject.GetComponent<CtrlAnimal>();
        speed = 0f;
        upspeed = 0f;
        iscol = true;
        //animal 是原来的所属者
        if (hitanimal != null)
        {

            Debug.Log("GameMain.id" + GameMain.id);
            // 被撞击的时候，如果是被撞击者的话
            if (ctrlanimal != null)
            {
                if (ctrlanimal.id == GameMain.id)
                {
                    Debug.Log("爷爷撞到了");
                    MsgKey msg = new MsgKey();
                    NetManager.Send(msg);
                    Destroy(gameObject);
                    Debug.Log("ctrlanimal.id" + ctrlanimal.id);
                }
            }
            else if (hitanimal != null)//撞到动物了
            {
                //PanelManager.Open<TipPanel>("有人碰到了钥匙");
                Destroy(gameObject);
            }
        }
       
        return;
    }
    public void destroySelfKey()
    {
        Destroy(gameObject);
    }
    //private void OnCollisionEnter(Collision other)
    //{
    //    GameObject collObj = other.gameObject;
    //    BaseAnimal hitanimal = collObj.gameObject.GetComponent<BaseAnimal>();
    //    CtrlAnimal ctrlanimal = collObj.gameObject.GetComponent<CtrlAnimal>();
    //    speed = 0f;
    //    upspeed = 0f;
    //    iscol = true;
    //    //或者固定主机判断撞到没？
    //    Debug.Log("ppppppppppppppp");

    //    if (ctrlanimal != null)
    //    {

    //        Debug.Log("GameMain.id" + GameMain.id);
    //        // 被撞击的时候，如果是被撞击者的话
    //        if(ctrlanimal!=null)
    //        {
    //            if (ctrlanimal.id == GameMain.id)
    //            {
    //                Debug.Log("爷爷撞到了");
    //                MsgKey msg = new MsgKey();
    //                NetManager.Send(msg);
    //                Destroy(gameObject);
    //                Debug.Log("ctrlanimal.id" + ctrlanimal.id);
    //            }
    //        }
    //        if (hitanimal != null)//撞到动物了
    //        {
    //            PanelManager.Open<TipPanel>("有人碰到了钥匙");
    //            Destroy(gameObject);
    //        }
    //    }
    //}
    //private void OnControllerColliderHit(ControllerColliderHit hit)
    //{
    //    GameObject collObj = hit.gameObject;
    //    BaseAnimal hitanimal = collObj.gameObject.GetComponent<BaseAnimal>();
    //    CtrlAnimal ctrlanimal = collObj.gameObject.GetComponent<CtrlAnimal>();
    //    speed = 0f;
    //    upspeed = 0f;
    //    iscol = true;
    //    //或者固定主机判断撞到没？
    //    Debug.Log("ppppppppppppppp");

    //    if (ctrlanimal != null)
    //    {

    //        Debug.Log("GameMain.id" + GameMain.id);
    //        // 被撞击的时候，如果是被撞击者的话
    //        if (ctrlanimal != null)
    //        {
    //            if (ctrlanimal.id == GameMain.id)
    //            {
    //                Debug.Log("爷爷撞到了");
    //                MsgKey msg = new MsgKey();
    //                NetManager.Send(msg);
    //                Destroy(gameObject);
    //                Debug.Log("ctrlanimal.id" + ctrlanimal.id);
    //            }
    //        }
    //        if (hitanimal != null)//撞到动物了
    //        {
    //            PanelManager.Open<TipPanel>("有人碰到了钥匙");
    //            Destroy(gameObject);
    //        }
    //    }
    //}
    //void OnCollisionEnter(Collision collisionInfo)
    //{
    //    Debug.Log("ppppppppppppppp");

    //    //rigidBody.constraints = RigidbodyConstraints.FreezePositionX;
    //    //rigidBody.constraints = RigidbodyConstraints.FreezePositionY;
    //    //rigidBody.constraints = RigidbodyConstraints.FreezePositionZ;

    //    //Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
    //    //foreach (Collider nearby in colliders)
    //    //{
    //    //    BaseAnimal syncAnimal = nearby.gameObject.GetComponent<BaseAnimal>();

    //    //    if ( syncAnimal != null)
    //    //    {
    //    //        Debug.Log("r撞到了11");
    //    //        if (syncAnimal.id == GameMain.id)
    //    //        {
    //    //            Debug.Log("爷爷撞到了");
    //    //            MsgKey msg = new MsgKey();
    //    //            NetManager.Send(msg);

    //    //        }
    //    //        Destroy(gameObject);
    //    //        return;
    //    //    }
    //    //}
    //    //return;


    //    //打到的动物
    //    GameObject collObj = collisionInfo.gameObject;
    //    BaseAnimal hitanimal = collObj.GetComponent<BaseAnimal>();
    //    speed = 0f;
    //    upspeed = 0f;
    //    iscol = true;
    //    //或者固定主机判断撞到没？
    //    Debug.Log("ppppppppppppppp");
    //    if (hitanimal != null)
    //    {
    //        Debug.Log("hitanimal.id" + hitanimal.id);
    //        Debug.Log("GameMain.id" + GameMain.id);
    //        // 被撞击的时候，如果是被撞击者的话
    //        if (hitanimal != null && hitanimal.id == GameMain.id)
    //        {
    //            Debug.Log("爷爷撞到了");
    //            MsgKey msg = new MsgKey();
    //            NetManager.Send(msg);
    //            Destroy(gameObject);
    //        }
    //        else if (hitanimal != null)//撞到动物了
    //        {
    //            PanelManager.Open<TipPanel>("有人碰到了钥匙");
    //            Destroy(gameObject);
    //        }
    //    }

    //    return;
    //}
    /*
    void OnCollisionEnter(Collision collisionInfo)
    {
        Debug.Log("ppppppppppppppp");

        rigidBody.constraints = RigidbodyConstraints.FreezePositionX;
        rigidBody.constraints = RigidbodyConstraints.FreezePositionY;
        rigidBody.constraints = RigidbodyConstraints.FreezePositionZ;

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider nearby in colliders)
        {
            BaseAnimal syncAnimal = nearby.gameObject.GetComponent<BaseAnimal>();

            if (syncAnimal != null)
            {
                Debug.Log("r撞到了11");
                if (syncAnimal.id == GameMain.id)
                {
                    Debug.Log("爷爷撞到了");
                    MsgKey msg = new MsgKey();
                    NetManager.Send(msg);

                }
                Destroy(gameObject);
                return;
            }
        }
        return;


        //打到的动物
        //GameObject collObj = collisionInfo.gameObject;
        //BaseAnimal hitanimal = collObj.GetComponent<BaseAnimal>();
        //speed = 0f;
        //upspeed = 0f;
        //iscol = true;
        ////或者固定主机判断撞到没？
        //Debug.Log("ppppppppppppppp");
        //if (hitanimal != null)
        //{
        //    Debug.Log("hitanimal.id" + hitanimal.id);
        //    Debug.Log("GameMain.id" + GameMain.id);
        //    // 被撞击的时候，如果是被撞击者的话
        //    if (hitanimal != null && hitanimal.id == GameMain.id)
        //    {
        //        Debug.Log("爷爷撞到了");
        //        MsgKey msg = new MsgKey();
        //        NetManager.Send(msg);
        //        Destroy(gameObject);
        //    }
        //    else if (hitanimal != null)//撞到动物了
        //    {
        //        PanelManager.Open<TipPanel>("有人碰到了钥匙");
        //        Destroy(gameObject);
        //    }
        //}

        //return;
    }
    */
}
