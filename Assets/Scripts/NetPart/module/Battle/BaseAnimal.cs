using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAnimal : MonoBehaviour
{
    //����ģ��
    private GameObject skin;

    //�ƶ��ٶ�
    public float speed = 3f;
    //ת���ٶ�
    public float steer = 30;
    ////����
    //public Transform turret;
    ////�ڹ�
    //public Transform gun;
    ////�����
    //public Transform firePoint;
    //�ڵ�Cdʱ��
    public float fireCd = 0.5f;
    //��һ�η����ڵ���ʱ��
    public float lastFireTime = 0;

    //����
    public Rigidbody rigidBody;
    //����ֵ
    public float hp = 100;
    //������һ�����
    public string id = "";
    //��Ӫ
    public int camp = 0;

    // Use this for initialization
    public void Start()
    {

    }

    //��ʼ��
    public virtual void Init(string skinPath)
    {
        Debug.Log("skinPath="+skinPath);
        //Ƥ��
        GameObject skinRes = ResManager.LoadPrefab(skinPath);
        skin = (GameObject)Instantiate(skinRes);
        skin.transform.parent = this.transform;
        // skin.transform.localPosition = Vector3.zero;
        //����
        //rigidBody = gameObject.AddComponent<Rigidbody>();
        //BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
        //boxCollider.center = new Vector3(0, 2.5f, 1.47f);
        //boxCollider.size = new Vector3(7, 5, 12);
        //�����ڹ�
        //turret = skin.transform.Find("Turret");
        //gun = turret.transform.Find("Gun");
        //firePoint = gun.transform.Find("FirePoint");
    }

    //�����ڵ�
    public Bullet Fire()
    {
        //�Ѿ�����
        //if (isdie())
        //{
        //    return;
        //}
        //�Ѿ�dizzy
        //if (isdizzy())
        //{
        //    return null;
        //}
        //�����ڵ�
        GameObject bulletObj = new GameObject("bullet");
        //bulletObj.layer = LayerMask.NameToLayer("Bullet");
        Bullet bullet = bulletObj.AddComponent<Bullet>();
        bullet.Init();
        bullet.animal = this;
        //λ��
        bullet.transform.position = transform.position;
        bullet.transform.rotation = transform.rotation;
        //����ʱ��
        lastFireTime = Time.time;
        return bullet;
    }
    public Bomb FireBomb()
    {
        //�Ѿ�����
        //if (isdie())
        //{
        //    return;
        //}
        //�Ѿ�dizzy
        //if (isdizzy())
        //{
        //    return null;
        //}
        //�����ڵ�
        GameObject bombObj = new GameObject("bomb");
        //bulletObj.layer = LayerMask.NameToLayer("Bullet");
        Bomb bomb  = bombObj.AddComponent<Bomb>();
        bomb.Init();
        bomb.animal = this;
        //λ��
        bomb.transform.position = transform.position;
        bomb.transform.rotation = transform.rotation;
        //����ʱ��
        lastFireTime = Time.time;
        return bomb;
    }

    ////�Ƿ�����
    //public bool IsDie()
    //{
    //    return hp <= 0;
    //}

    //public bool isdizzy()
    //{
    //    return false;
    //}
    //������
    public void Attacked(float att,string Fireid)
    {
        //�Ѿ�����
        //if (IsDie())
        //{
        //    return;
        //}
        //��Ѫ
        // hp -= att;
        //����
        //if (IsDie())
        //{
        //    //��ʾ����Ч��
        //    GameObject obj = ResManager.LoadPrefab("AttackEffect");
        //    GameObject explosion = Instantiate(obj, transform.position, transform.rotation);
        //    explosion.transform.SetParent(transform);
        //}

        /* QAttack    BombAttack */
        //ѣ��
        //��ʾ����Ч��
        if (Fireid== "QAttack")
        {
            //GameObject obj = ResManager.LoadPrefab("WFX_Explosion");
            //GameObject explosion = Instantiate(obj, transform.position, transform.rotation);
            //explosion.transform.SetParent(transform);
            gameObject.GetComponent<Animator>().SetTrigger("Dizzy");
        }
        else if (Fireid == "BombAttack")
        {
            //GameObject obj = ResManager.LoadPrefab("WFX_ExplosiveSmoke Big Alt");
            //GameObject explosion = Instantiate(obj, transform.position, transform.rotation);
            //explosion.transform.SetParent(transform);
            gameObject.GetComponent<Animator>().SetTrigger("BeBomb");
        }

    }


    // Update is called once per frame
    public void Update()
    {

    }
}
