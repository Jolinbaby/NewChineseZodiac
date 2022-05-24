using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CtrlAnimal : BaseAnimal
{
    //��һ�η���ͬ����Ϣ��ʱ��
    private float lastSendSyncTime = 0;
    //ͬ��֡��
    public static float syncInterval = 0.05f;

    [Header("Movement")]
    [SerializeField] float speednew=10;
    [SerializeField] float rotationSmoothTime=0.2f;

    [Header("Gravity")]
    [SerializeField] float gravity = 9.8f;
    [SerializeField] float gravityMultiplier = 2;
    [SerializeField] float groundedGravity = -0.5f;
    [SerializeField] float jumpHeight = 3f;
    float velocityY;

    CharacterController controller;
    Camera cam;

    float currentAngle;
    float currentAngleVelocity;

    float sendSpeed = 0f;
    int sendJump = 0;

    private Animator anim;
    //public bool isGetKey;

    private bool isKeyMDown = false;

    private bool isKeyCDown = false;

    private void Awake()
    {
        //getting reference for components on the Player
        controller = GetComponent<CharacterController>();
        cam = Camera.main;
        anim = gameObject.GetComponent<Animator>();
        
    }
    private void getoutofworldUpdate()
    {
        //{ 47.8f,61f,-94f,0f, 0f, 0f},//������1
        if(transform.position.y<-63f|| transform.position.z>128.9f || transform.position.z < -151.0f || transform.position.x<-116f|| transform.position.x > 126f)
        {
            Vector3 birpos = new Vector3(47.8f, 61f, -94f);
            gameObject.transform.position = birpos;
        }
        
    }
    //private void MoveUpdate()
    //{
    //    HandleMovement();
    //    HandleGravityAndJump();
    //    MsgAnimation msg = new MsgAnimation();
    //    msg.isJump = sendJump;
    //    msg.speed = sendSpeed;
    //    NetManager.Send(msg);
    //}

    private void HandleMovement()
    {
        //capturing Input from Player
        Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        anim.SetFloat("Speed", 0f);
        sendSpeed = 0f;
        if (movement.magnitude >= 0.1f)
        {
            //compute rotation
            anim.SetFloat("Speed", 15f);
            sendSpeed = 15f;
            float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
            currentAngle = Mathf.SmoothDampAngle(currentAngle, targetAngle, ref currentAngleVelocity, rotationSmoothTime);
            transform.rotation = Quaternion.Euler(0, currentAngle, 0);

            //move in direction of rotation
            Vector3 rotatedMovement = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            controller.Move(rotatedMovement * speednew * Time.deltaTime);
        }
    }

    void HandleGravityAndJump()
    {
        
        //apply groundedGravity when the Player is Grounded
        if (controller.isGrounded && velocityY < 0f)
            velocityY = groundedGravity;
        sendJump = 0;

        //When Grounded and Jump Button is Pressed, set veloctiyY with the formula below
        if (controller.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            velocityY = Mathf.Sqrt(jumpHeight * 2f * gravity);
            anim.SetTrigger("isJump");
            sendJump = 1;

        }

        //applying gravity when Player is not grounded
        velocityY -= gravity * gravityMultiplier * Time.deltaTime;
        controller.Move(Vector3.up * velocityY * Time.deltaTime);
    }

    new void Update()
    {
        base.Update();
        //����ͬ����Ϣ
        SyncUpdate();
        //�ƶ�����
        //MoveUpdate();
        //��������
        //TurretUpdate();
        //����
        FireUpdate();
        FireBombUpdate();
        FireInkUpdate();
        //����
        ShieldUpdate();
        //����buff
        SpeedUpUpdate();
        //����
        JumpUpUpdate();
        //�㽶Ƥ
        BananaUpdate();

        //��ʾ��ͼ
        MaxMapUpdate();
        //�Ƿ���C����ɫչʾ����
        ShowCharacterUpdate();
        //-------------------------------------------
        //�Ƿ��õ�Կ��
        //GetKeyUpdate();
        getoutofworldUpdate();
    }
    private void ShowCharacterUpdate()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (!isKeyCDown)
            {
                PanelManager.Open<ShowCharacterPanel>();
            }
            else
            {
                PanelManager.Close("ShowCharacterPanel");
            }
            isKeyCDown = !isKeyCDown;
        }


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PanelManager.Close("TipPanel");
        }
    }
    //private void ShowCharacterUpdate()
    //{
    //    if (Input.GetKeyDown(KeyCode.C))
    //    {
    //        if (!isKeyCDown)
    //        {
    //            PanelManager.Open<ShowCharacterPanel>();
    //        }
    //        else
    //        {
    //            PanelManager.Close("ShowCharacterPanel");
    //        }
    //        isKeyCDown = !isKeyCDown;
    //    }
    //}
    private void MaxMapUpdate()
    {
        GameObject UI_Map = GameObject.Find("UI_Map");
        GameObject maxmap = UI_Map.transform.Find("maxmap").gameObject;
        GameObject minmap = UI_Map.transform.Find("minimap").gameObject;
        GameObject UI_item = GameObject.Find("UI_ItemBar");
        GameObject itemBar = UI_item.transform.Find("ItemBar").gameObject;
        if (Input.GetKeyDown(KeyCode.M))
        {
            maxmap.SetActive(true);
            minmap.SetActive(false);
            itemBar.SetActive(false);
            isKeyMDown = true;
        }
        
        if (Input.GetKeyUp(KeyCode.M) && isKeyMDown)
        {
            maxmap.SetActive(false);
            minmap.SetActive(true);
            itemBar.SetActive(true);
            isKeyMDown = false;
        }
    }

    //�ƶ�����
    public void MoveUpdate()
    {
        
        ////�Ѿ�����
        //if (IsDie())
        //{
        //    return;
        //}
        // ��ת
        //float x = Input.GetAxis("Horizontal");
        //transform.Rotate(0, x * steer * Time.deltaTime, 0);
        ////ǰ������
        //float y = Input.GetAxis("Vertical");
        //Vector3 s = y * transform.forward * speed * Time.deltaTime;
        //transform.transform.position += s;
    }


    //����
    public void FireUpdate()
    {
        //�Ѿ�����
        //if (IsDie())
        //{
        //    return;
        //}
        //if (isdizzy())
        //{
        //    return;
        //}
        //�����ж�
        if (!Input.GetKey(KeyCode.Q))
        {
            return;
        }
        //cd�Ƿ��ж�
        if (Time.time - lastFireTime < fireCd)
        {
            return;
        }
       
        //����
        Bullet bullet = Fire();
        //����ͬ��Э��
        MsgFire msg = new MsgFire();
        msg.x = bullet.transform.position.x;
        msg.y = bullet.transform.position.y;
        msg.z = bullet.transform.position.z;
        msg.ex = bullet.transform.eulerAngles.x;
        msg.ey = bullet.transform.eulerAngles.y;
        msg.ez = bullet.transform.eulerAngles.z;

        msg.Fireid = "QAttack";

        NetManager.Send(msg);
    }

    public void FireBombUpdate()
    {
        //�Ѿ�����
        //if (IsDie())
        //{
        //    return;
        //}
        //if (isdizzy())
        //{
        //    return;
        //}
        //�����ж�
        if (ItemManager.isThrowBomb==false)
        {
            return;
        }
        //cd�Ƿ��ж�
        if (Time.time - lastFireTime < fireCd)
        {
            return;
        }
     
        Debug.Log(id + "************************************");
        Bomb bomb = FireBomb();

        //����ͬ��Э��
        MsgFire msg = new MsgFire();
        msg.x = bomb.transform.position.x;
        msg.y = bomb.transform.position.y;
        msg.z = bomb.transform.position.z;
        msg.ex = bomb.transform.eulerAngles.x;
        msg.ey = bomb.transform.eulerAngles.y;
        msg.ez = bomb.transform.eulerAngles.z;
        msg.Fireid = "BombAttack";
        NetManager.Send(msg);

        ItemManager.isThrowBomb = false;
    }

    public void ShieldUpdate()
    {
        //�����ж�
        if (ItemManager.isShield == false)
        {
            return;
        }
        //����
        //if (!Input.GetKey(KeyCode.E))
        //{
        //    return;
        //}
       
        if (Time.time - lastShieldTime < ShieldCd)
        {
            return;
        }
        ShieldProp shieldProp = SpawnShield();
        //����ͬ��Э��

        MsgFire msg = new MsgFire();
        msg.x = shieldProp.transform.position.x;
        msg.y = shieldProp.transform.position.y;
        msg.z = shieldProp.transform.position.z;
        msg.ex = shieldProp.transform.eulerAngles.x;
        msg.ey = shieldProp.transform.eulerAngles.y;
        msg.ez = shieldProp.transform.eulerAngles.z;
        msg.Fireid = "Shield";
        NetManager.Send(msg);

        ItemManager.isShield = false;
    }

    public void SpeedUpUpdate()
    {
        //�����ж�
        if (ItemManager.isSpeedUp == false)
        {
            return;
        }
        //cd�Ƿ��ж�
        //if (Time.time - lastFireTime < fireCd)
        //{
        //    return;
        //}
        SpeedUpBuff speedUpBuff = StartSpeedUp();
        //����ͬ��Э��

        MsgFire msg = new MsgFire();
        msg.x = speedUpBuff.transform.position.x;
        msg.y = speedUpBuff.transform.position.y;
        msg.z = speedUpBuff.transform.position.z;
        msg.ex = speedUpBuff.transform.eulerAngles.x;
        msg.ey = speedUpBuff.transform.eulerAngles.y;
        msg.ez = speedUpBuff.transform.eulerAngles.z;
        msg.Fireid = "SpeedUp";
        NetManager.Send(msg);

        ItemManager.isSpeedUp = false;

    }

    public void JumpUpUpdate()
    {
        //�����ж�
        if (ItemManager.isJumpUp == false)
        {
            return;
        }
        //cd�Ƿ��ж�
        //if (Time.time - lastFireTime < fireCd)
        //{
        //    return;
        //}
        JumpUpBuff jumpUpBuff = StartJumpUp();
        //����ͬ��Э��

        MsgFire msg = new MsgFire();
        msg.x = jumpUpBuff.transform.position.x;
        msg.y = jumpUpBuff.transform.position.y;
        msg.z = jumpUpBuff.transform.position.z;
        msg.ex = jumpUpBuff.transform.eulerAngles.x;
        msg.ey = jumpUpBuff.transform.eulerAngles.y;
        msg.ez = jumpUpBuff.transform.eulerAngles.z;
        msg.Fireid = "JumpUp";
        NetManager.Send(msg);

        ItemManager.isJumpUp = false;

    }


    public void SuperUpdate()
    {
        //�����ж�
        if (ItemManager.isSuper == false)
        {
            return;
        }
        //cd�Ƿ��ж�
        //if (Time.time - lastFireTime < fireCd)
        //{
        //    return;
        //}
        SuperBuff superBuff = StartSuper();
        //����ͬ��Э��

        MsgFire msg = new MsgFire();
        msg.x = superBuff.transform.position.x;
        msg.y = superBuff.transform.position.y;
        msg.z = superBuff.transform.position.z;
        msg.ex = superBuff.transform.eulerAngles.x;
        msg.ey = superBuff.transform.eulerAngles.y;
        msg.ez = superBuff.transform.eulerAngles.z;
        msg.Fireid = "Super";
        NetManager.Send(msg);

        ItemManager.isSuper = false;

    }

    public void FireInkUpdate()
    {
        //����
        //if (!Input.GetKey(KeyCode.E))
        //{
        //    return;
        //}
        //�Ѿ�����
        //if (IsDie())
        //{
        //    return;
        //}
        //if (isdizzy())
        //{
        //    return;
        //}
        //�����ж�
        if (ItemManager.isThrowInk == false)
        {
            return;
        }
        //cd�Ƿ��ж�
        if (Time.time - lastFireTime < fireCd)
        {
            return;
        }

        Debug.Log(id + "************************************");
        Ink ink = FireInk();

        //����ͬ��Э��
        MsgFire msg = new MsgFire();
        msg.x = ink.transform.position.x;
        msg.y = ink.transform.position.y;
        msg.z = ink.transform.position.z;
        msg.ex = ink.transform.eulerAngles.x;
        msg.ey = ink.transform.eulerAngles.y;
        msg.ez = ink.transform.eulerAngles.z;
        msg.Fireid = "InkAttack";
        NetManager.Send(msg);

        ItemManager.isThrowInk = false;
    }

    public void BananaUpdate()
    {
        //�����ж�
        if (ItemManager.isThrowBanana == false)
        {
            return;
        }
        //cd�Ƿ��ж�
        if (Time.time - lastFireTime < fireCd)
        {
            return;
        }

        Debug.Log(id + "************************************");
        Banana banana = SpawnBanana();

        //����ͬ��Э��
        MsgFire msg = new MsgFire();
        msg.x = banana.transform.position.x;
        msg.y = banana.transform.position.y;
        msg.z = banana.transform.position.z;
        msg.ex = banana.transform.eulerAngles.x;
        msg.ey = banana.transform.eulerAngles.y;
        msg.ez = banana.transform.eulerAngles.z;
        msg.Fireid = "BananaAttack";
        NetManager.Send(msg);

        ItemManager.isThrowBanana = false;
    }


    //����ͬ����Ϣ
    public void SyncUpdate()
    {
        //ʱ�����ж�
        if (Time.time - lastSendSyncTime < syncInterval)
        {
            return;
        }
        lastSendSyncTime = Time.time;
        //����ͬ��Э��
        MsgSyncAnimal msg = new MsgSyncAnimal();
        msg.x = transform.position.x;
        msg.y = transform.position.y;
        msg.z = transform.position.z;
        msg.ex = transform.eulerAngles.x;
        msg.ey = transform.eulerAngles.y;
        msg.ez = transform.eulerAngles.z;
        //msg.turretY = turret.localEulerAngles.y;
        //msg.gunX = gun.localEulerAngles.x;
        NetManager.Send(msg);
    }

    public void GetKeyUpdate()
    {
        if (isGetKey == false) return;

        Debug.Log(" GetKeyUpdate()");
        MsgKey msg = new MsgKey();
        //msg.id = this.id;
        NetManager.Send(msg);
        
    }
}
