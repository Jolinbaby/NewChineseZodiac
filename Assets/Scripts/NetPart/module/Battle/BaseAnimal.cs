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

    //������Cdʱ��
    public float ShieldCd = 1f;
    //��һ�α����ֵ�ʱ��
    public float lastShieldTime = 0;

    //����
    public Rigidbody rigidBody;
    //����ֵ
    public float hp = 100;
    //������һ�����
    public string id = "";
    //��Ӫ
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

    //���ɱ�����
    public ShieldProp SpawnShield()
    {
        Debug.Log("SpawnShield!!!!!!!!!!!!!!!!!!!!");
        GameObject shieldObj = new GameObject("shield");
        shieldProp = shieldObj.AddComponent<ShieldProp>();
        shieldProp.Init();
        shieldProp.animal = this;
        //λ��
        shieldProp.transform.position = transform.position;
        shieldProp.transform.transform.parent = this.transform;

        isShieldProtect = true;
        shieldProp.transform.rotation = transform.rotation;
        //����ʱ��
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
        GameObject inkObj = new GameObject("ink");
        //bulletObj.layer = LayerMask.NameToLayer("Bullet");
        Ink ink = inkObj.AddComponent<Ink>();
        ink.Init();
        ink.animal = this;
        //λ��
        ink.transform.position = transform.position;
        ink.transform.rotation = transform.rotation;
        //����ʱ��
        lastFireTime = Time.time;
        Debug.Log("InkFIred");
        return ink;
    }

    public SpeedUpBuff StartSpeedUp()
    {
        Debug.Log("��ʼ������!!!!!!!!!!!!!!!!!!!!");
        GameObject speedupObj = new GameObject("speedup");
        SpeedUpBuff speedUpBuff = speedupObj.AddComponent<SpeedUpBuff>();
        
        speedUpBuff.animal = this;
        speedUpBuff.Init();
        //λ��
        speedUpBuff.transform.position = transform.position;
        speedUpBuff.transform.transform.parent = this.transform;
        speedUpBuff.transform.rotation = transform.rotation;
        //����ʱ��
        lastShieldTime = Time.time;
        return speedUpBuff;
    }

    public JumpUpBuff StartJumpUp()
    {
        Debug.Log("��ʼ��Ծ�߶�������!!!!!!!!!!!!!!!!!!!!");
        GameObject jumpUpObj = new GameObject("jumpup");
        JumpUpBuff jumpUpBuff = jumpUpObj.AddComponent<JumpUpBuff>();
        jumpUpBuff.animal = this;
        jumpUpBuff.Init();
        
        //λ��
        jumpUpBuff.transform.position = transform.position;
        jumpUpBuff.transform.transform.parent = this.transform;
        jumpUpBuff.transform.rotation = transform.rotation;
        //����ʱ��
        lastShieldTime = Time.time;
        return jumpUpBuff;
    }

    public SuperBuff StartSuper()
    {
        Debug.Log("�޵���!!!!!!!!!!!!!!!!!!!!");
        GameObject superObj = new GameObject("super");
        SuperBuff superBuff = superObj.AddComponent<SuperBuff>();
        superBuff.Init();
        superBuff.animal = this;
        //λ��
        superBuff.transform.position = transform.position;
        superBuff.transform.transform.parent = this.transform;
        superBuff.transform.rotation = transform.rotation;
        //����ʱ��
        lastShieldTime = Time.time;
        return superBuff;
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
            //new!05231645
            if (isShieldProtect)
            {
                return;
            }
            //GameObject obj = ResManager.LoadPrefab("WFX_Explosion");
            //GameObject explosion = Instantiate(obj, transform.position, transform.rotation);
            //explosion.transform.SetParent(transform);
            gameObject.GetComponent<Animator>().SetTrigger("Dizzy");

            //�������,Կ�׻���--------------------------
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
            //�������,Կ�׻���--------------------------
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

        //����Կ��
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
        ////λ��
        //thekey.transform.position = transform.position;
        //thekey.transform.rotation = transform.rotation;

    }
    // Update is called once per frame
    public void Update()
    {

    }
}
