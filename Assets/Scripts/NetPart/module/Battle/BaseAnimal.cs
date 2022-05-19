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

    //物理
    public Rigidbody rigidBody;
    //生命值
    public float hp = 100;
    //属于哪一名玩家
    public string id = "";
    //阵营
    public int camp = 0;

    // Use this for initialization
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
            GameObject obj = ResManager.LoadPrefab("WFX_Explosion");
            GameObject explosion = Instantiate(obj, transform.position, transform.rotation);
            explosion.transform.SetParent(transform);
            gameObject.GetComponent<Animator>().SetTrigger("Dizzy");
        }
        else if (Fireid == "BombAttack")
        {
            GameObject obj = ResManager.LoadPrefab("WFX_ExplosiveSmoke Big Alt");
            GameObject explosion = Instantiate(obj, transform.position, transform.rotation);
            explosion.transform.SetParent(transform);
            gameObject.GetComponent<Animator>().SetTrigger("BeBomb");
        }

    }


    // Update is called once per frame
    public void Update()
    {

    }
}
