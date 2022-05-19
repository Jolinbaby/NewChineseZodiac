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
    public bool isGetKey;
    private void Awake()
    {
        //getting reference for components on the Player
        controller = GetComponent<CharacterController>();
        cam = Camera.main;
        anim = gameObject.GetComponent<Animator>();
        
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

        //�Ƿ��õ�Կ��
        GetKeyUpdate();
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
