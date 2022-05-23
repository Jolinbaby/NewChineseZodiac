using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAnimal : MonoBehaviour
{
    //动物模型
    private GameObject skin;

    //移动速度
    public float speed = 3f;
    //转向速度
    public float steer = 30;
    ////炮塔
    //public Transform turret;
    ////炮管
    //public Transform gun;
    ////发射点
    //public Transform firePoint;
    //炮弹Cd时间
    public float fireCd = 0.5f;
    //上一次发射炮弹的时间
    public float lastFireTime = 0;

    //保护罩Cd时间
    public float ShieldCd = 1f;
    //上一次保护罩的时间
    public float lastShieldTime = 0;

    //物理
    public Rigidbody rigidBody;
    //生命值
    public float hp = 100;
    //属于哪一名玩家
    public string id = "";
    //阵营
    public int camp = 0;

    //---------------------------------
    public bool isGetKey;
    public GameObject FireposObj;
    public GameObject GetKeyEff;

    public ShieldProp shieldProp;
    // Use this for initialization

    public bool isShieldProtect;
    public void Start()
    {

    }

    //初始化
    public virtual void Init(string skinPath)
    {
        Debug.Log("skinPath="+skinPath);
        //皮肤
        GameObject skinRes = ResManager.LoadPrefab(skinPath);
        skin = (GameObject)Instantiate(skinRes);
        skin.transform.parent = this.transform;
        // skin.transform.localPosition = Vector3.zero;
        //物理
        //rigidBody = gameObject.AddComponent<Rigidbody>();
        //BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
        //boxCollider.center = new Vector3(0, 2.5f, 1.47f);
        //boxCollider.size = new Vector3(7, 5, 12);
        //炮塔炮管
        //turret = skin.transform.Find("Turret");
        //gun = turret.transform.Find("Gun");
        //firePoint = gun.transform.Find("FirePoint");
    }

    //发射炮弹
    public Bullet Fire()
    {
        //已经死亡
        //if (isdie())
        //{
        //    return;
        //}
        //已经dizzy
        //if (isdizzy())
        //{
        //    return null;
        //}
        //产生炮弹
        GameObject bulletObj = new GameObject("bullet");
        //bulletObj.layer = LayerMask.NameToLayer("Bullet");
        Bullet bullet = bulletObj.AddComponent<Bullet>();
        bullet.Init();
        bullet.animal = this;
        //位置
        bullet.transform.position = transform.position;
        bullet.transform.rotation = transform.rotation;
        //更新时间
        lastFireTime = Time.time;
        return bullet;
    }
    public Bomb FireBomb()
    {
        //已经死亡
        //if (isdie())
        //{
        //    return;
        //}
        //已经dizzy
        //if (isdizzy())
        //{
        //    return null;
        //}
        //产生炮弹
        GameObject bombObj = new GameObject("bomb");
        //bulletObj.layer = LayerMask.NameToLayer("Bullet");
        Bomb bomb  = bombObj.AddComponent<Bomb>();
        bomb.Init();
        bomb.animal = this;
        //位置
        bomb.transform.position = transform.position;
        bomb.transform.rotation = transform.rotation;
        //更新时间
        lastFireTime = Time.time;
        return bomb;
    }

    //生成保护罩
    public ShieldProp SpawnShield()
    {
        Debug.Log("SpawnShield!!!!!!!!!!!!!!!!!!!!");
        GameObject shieldObj = new GameObject("shield");
        shieldProp = shieldObj.AddComponent<ShieldProp>();
        shieldProp.Init();
        shieldProp.animal = this;
        //位置
        shieldProp.transform.position = transform.position;
        shieldProp.transform.transform.parent = this.transform;

        isShieldProtect = true;
        shieldProp.transform.rotation = transform.rotation;
        //更新时间
        lastShieldTime = Time.time;
        Invoke("DestroyShield", 5f);
        return shieldProp;
    }

    public void DestroyShield()
    {
        isShieldProtect = false;
    }

    public Ink FireInk()
    {
        //已经死亡
        //if (isdie())
        //{
        //    return;
        //}
        //已经dizzy
        //if (isdizzy())
        //{
        //    return null;
        //}
        //产生炮弹
        GameObject inkObj = new GameObject("ink");
        //bulletObj.layer = LayerMask.NameToLayer("Bullet");
        Ink ink = inkObj.AddComponent<Ink>();
        ink.Init();
        ink.animal = this;
        //位置
        ink.transform.position = transform.position;
        ink.transform.rotation = transform.rotation;
        //更新时间
        lastFireTime = Time.time;
        Debug.Log("InkFIred");
        return ink;
    }

    public SpeedUpBuff StartSpeedUp()
    {
        Debug.Log("开始加速啦!!!!!!!!!!!!!!!!!!!!");
        GameObject speedupObj = new GameObject("speedup");
        SpeedUpBuff speedUpBuff = speedupObj.AddComponent<SpeedUpBuff>();
        
        speedUpBuff.animal = this;
        speedUpBuff.Init();
        //位置
        speedUpBuff.transform.position = transform.position;
        speedUpBuff.transform.transform.parent = this.transform;
        speedUpBuff.transform.rotation = transform.rotation;
        //更新时间
        lastShieldTime = Time.time;
        return speedUpBuff;
    }

    public JumpUpBuff StartJumpUp()
    {
        Debug.Log("开始跳跃高度增加啦!!!!!!!!!!!!!!!!!!!!");
        GameObject jumpUpObj = new GameObject("jumpup");
        JumpUpBuff jumpUpBuff = jumpUpObj.AddComponent<JumpUpBuff>();
        jumpUpBuff.animal = this;
        jumpUpBuff.Init();
        
        //位置
        jumpUpBuff.transform.position = transform.position;
        jumpUpBuff.transform.transform.parent = this.transform;
        jumpUpBuff.transform.rotation = transform.rotation;
        //更新时间
        lastShieldTime = Time.time;
        return jumpUpBuff;
    }

    public SuperBuff StartSuper()
    {
        Debug.Log("无敌啦!!!!!!!!!!!!!!!!!!!!");
        GameObject superObj = new GameObject("super");
        SuperBuff superBuff = superObj.AddComponent<SuperBuff>();
        superBuff.Init();
        superBuff.animal = this;
        //位置
        superBuff.transform.position = transform.position;
        superBuff.transform.transform.parent = this.transform;
        superBuff.transform.rotation = transform.rotation;
        //更新时间
        lastShieldTime = Time.time;
        return superBuff;
    }

    ////是否死亡
    //public bool IsDie()
    //{
    //    return hp <= 0;
    //}

    //public bool isdizzy()
    //{
    //    return false;
    //}
    //被攻击
    public void Attacked(float att,string Fireid)
    {
        //已经死亡
        //if (IsDie())
        //{
        //    return;
        //}
        //扣血
        // hp -= att;
        //死亡
        //if (IsDie())
        //{
        //    //显示焚烧效果
        //    GameObject obj = ResManager.LoadPrefab("AttackEffect");
        //    GameObject explosion = Instantiate(obj, transform.position, transform.rotation);
        //    explosion.transform.SetParent(transform);
        //}

        /* QAttack    BombAttack */
        //眩晕
        //显示焚烧效果
        if (Fireid== "QAttack")
        {
            //new!05231645
            if (isShieldProtect)
            {
                return;
            }
            //GameObject obj = ResManager.LoadPrefab("WFX_Explosion");
            //GameObject explosion = Instantiate(obj, transform.position, transform.rotation);
            //explosion.transform.SetParent(transform);
            gameObject.GetComponent<Animator>().SetTrigger("Dizzy");

            //假设击落,钥匙击飞--------------------------
            if(isGetKey==true)
            {
                isGetKey = false;
                KeyFall();
            }
        }
        else if (Fireid == "BombAttack")
        {
            //new!05231645
            if (isShieldProtect)
            {
                isShieldProtect = false;
                shieldProp.Breakshield();
                return;
            }
            //假设击落,钥匙击飞--------------------------
            if (isGetKey == true)
            {
                isGetKey = false;
                KeyFall();
            }
            //GameObject obj = ResManager.LoadPrefab("WFX_ExplosiveSmoke Big Alt");
            //GameObject explosion = Instantiate(obj, transform.position, transform.rotation);
            //explosion.transform.SetParent(transform);
            gameObject.GetComponent<Animator>().SetTrigger("BeBomb");
        }
        else if (Fireid == "InkAttack")
        {
            //new!05231645
            if (isShieldProtect)
            {
                isShieldProtect = false;
                shieldProp.Breakshield();
                return;
            }
            /*            GameObject obj = ResManager.LoadPrefab("WFX_ExplosiveSmoke Big Alt");
                        GameObject explosion = Instantiate(obj, transform.position, transform.rotation);
                        explosion.transform.SetParent(transform);*/
            Debug.Log("set trigger");
            GameObject.Find("UI_Ink/Image").GetComponent<Animator>().SetTrigger("BeInk");
            //gameObject.GetComponent<Animator>().SetTrigger("BeInk");
        }
    }

    public void ShowKey()
    {
        //-------------------------------
        //WFXMR_FlameThrower Big
        isGetKey = true;

        GameObject theKey = ResManager.LoadPrefab("Key");
        Vector3 Firepos = new Vector3(0, 1.75f, 2.34f);
        FireposObj = Instantiate(theKey, transform.position + Firepos, transform.rotation);
        FireposObj.transform.SetParent(transform);

        GameObject obj = ResManager.LoadPrefab("WFXMR_FlameThrower Big");
         GetKeyEff = Instantiate(obj, transform.position, transform.rotation);
        GetKeyEff.transform.SetParent(transform); 
    }


    public void KeyFall()
    {
        Destroy(FireposObj);
        Destroy(GetKeyEff);
        ToThrowKey();
    }
    public void  ToThrowKey()
    {
        //GameObject originKey=

        //产生钥匙
        GameObject theKey = ResManager.LoadPrefab("Key");
        Vector3 Firepos = new Vector3(0, 1.75f, 2.34f);
        GameObject chuxiankey= Instantiate(theKey, transform.position + Firepos, transform.rotation);
        KeyScripts keyScript = chuxiankey.AddComponent<KeyScripts>();
        //keyScript.Init();
        //BoxCollider boxCollider = theKey.gameObject.AddComponent<BoxCollider>();
        //GameObject keyObj = new GameObject("lostKey");
        //BoxCollider boxCollider = keyObj.AddComponent<BoxCollider>();
        //boxCollider.center = new Vector3(0.3f, 2.7f, -4.3f);
        //boxCollider.size = new Vector3(4f, 6f, 1f);

        ////keyObj.AddComponent<BoxCollider>();
        //KeyScripts thekey = keyObj.AddComponent<KeyScripts>();

        //thekey.Init();
        //thekey.animal = this;
        ////位置
        //thekey.transform.position = transform.position;
        //thekey.transform.rotation = transform.rotation;

    }
    // Update is called once per frame
    public void Update()
    {

    }
}
