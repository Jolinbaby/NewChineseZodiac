using PlayerControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncAnimal : BaseAnimal
{
    //Ԥ����Ϣ���ĸ�ʱ�䵽���ĸ�λ��
    private Vector3 lastPos;
    private Vector3 lastRot;
    private Vector3 forecastPos;
    private Vector3 forecastRot;
    private float lastTurretY;
    private float forecastTurretY;
    private float lastGunX;
    private float forecastGunX;
    private float forecastTime;

    private Animator anim;
    private int jump = 0;
    //��дInit
    public override void Init(string skinPath)
    {
        base.Init(skinPath);

        //���������˶�Ӱ��
        rigidBody.constraints = RigidbodyConstraints.FreezeAll; //�������Ҫ�� ��Ϊrigibody�������Ǽ���ȥ��
        rigidBody.useGravity = false;
        //��ʼ��Ԥ����Ϣ
        lastPos = transform.position;
        lastRot = transform.eulerAngles;
        forecastPos = transform.position;
        forecastRot = transform.eulerAngles;
        //lastTurretY = turret.localEulerAngles.y;
        //forecastTurretY = turret.localEulerAngles.y;
        //lastGunX = gun.localEulerAngles.x;
        //forecastGunX = gun.localEulerAngles.x;
        forecastTime = Time.time;
        this.gameObject.GetComponent<ThirdPersonController>().enabled = false;
        //Destroy(this.gameObject.GetComponent<PlayerInputs>());
        //Destroy(this.gameObject.GetComponent<BasicRigidBodyPush>());
        anim = this.gameObject.GetComponent<Animator>();
        anim.SetFloat("Speed", 16f);
    }

    new void Update()
    {
        base.Update();
        //����λ��
        ForecastUpdate();
        //UpdateAnim();
    }

    public void UpdateAnim()
    {

    }

    //�ƶ�ͬ��
    public void SyncPos(MsgSyncAnimal msg)
    {
        //Ԥ��λ��
        Vector3 pos = new Vector3(msg.x, msg.y, msg.z);
        Vector3 rot = new Vector3(msg.ex, msg.ey, msg.ez);
        //forecastPos = pos + 2*(pos - lastPos);
        //forecastRot = rot + 2*(rot - lastRot);
        forecastPos = pos;  //���治Ԥ��
        forecastRot = rot;
        //forecastTurretY = msg.turretY;
        //forecastGunX = msg.gunX;
        //����
        lastPos = pos;
        lastRot = rot;
        //lastTurretY = turret.localEulerAngles.y;
        //lastGunX = turret.localEulerAngles.x;
        forecastTime = Time.time;
    }

    public void SyncAnim(MsgAnimation msg)
    {
        jump = msg.isJump;
        if (jump == 1)
        {
            anim.SetTrigger("Jump");
            jump = 0;
        }
           
        //float speed = msg.speed;
        //anim.SetFloat("Speed", speed);
    }

    //����λ��
    public void ForecastUpdate()
    {
        //ʱ��
        float t = (Time.time - forecastTime) / CtrlAnimal.syncInterval;
        t = Mathf.Clamp(t, 0f, 1f);
        //λ��
        Vector3 pos = transform.position;
        pos = Vector3.Lerp(pos, forecastPos, t);
        transform.position = pos;
        //��ת
        Quaternion quat = transform.rotation;
        Quaternion forcastQuat = Quaternion.Euler(forecastRot);
        quat = Quaternion.Lerp(quat, forcastQuat, t);
        transform.rotation = quat;
        //������ת���Ĵ�����
        //float axis = transform.InverseTransformPoint(forecastPos).z;
        //axis = Mathf.Clamp(axis * 1024, -1f, 1f);
       // WheelUpdate(axis);
        //�ڹ�
        //Vector3 le = turret.localEulerAngles;
        //le.y = Mathf.LerpAngle(le.y, forecastTurretY, t);
        //turret.localEulerAngles = le;
        ////����
        //le = gun.localEulerAngles;
        //le.x = Mathf.LerpAngle(le.x, forecastGunX, t);
        //gun.localEulerAngles = le;
    }

    //����
    public void SyncFire(MsgFire msg)
    {
        //Debug.Log(" SyncFire---------------------"+msg);
        string mFireid = msg.Fireid;
        if(mFireid== "QAttack")
        {
            myTAR.x = msg.ex;
            myTAR.y = msg.ey;
            myTAR.z = msg.ez;
            Bullet bullet = Fire();
            //��������
            Vector3 pos = new Vector3(msg.x, msg.y, msg.z);
            Vector3 rot = new Vector3(msg.ex, msg.ey, msg.ez);
            bullet.transform.position = pos;
            bullet.transform.eulerAngles = rot;

        }
        else if (mFireid == "BombAttack")
        {
            Bomb bomb = FireBomb();
            //��������
            Vector3 pos = new Vector3(msg.x, msg.y, msg.z);
            Vector3 rot = new Vector3(msg.ex, msg.ey, msg.ez);
            bomb.transform.position = pos;
            bomb.transform.eulerAngles = rot;
        }
        else if (mFireid == "Shield")
        {
            ShieldProp shieldProp = SpawnShield();
            //��������
            Vector3 pos = new Vector3(msg.x, msg.y, msg.z);
            Vector3 rot = new Vector3(msg.ex, msg.ey, msg.ez);
            shieldProp.transform.position = pos;
            shieldProp.transform.eulerAngles = rot;
        }
        else if (mFireid == "InkAttack")
        {
            Ink ink = FireInk();
            //��������
            Vector3 pos = new Vector3(msg.x, msg.y, msg.z);
            Vector3 rot = new Vector3(msg.ex, msg.ey, msg.ez);
            ink.transform.position = pos;
            ink.transform.eulerAngles = rot;
        }
        else if (mFireid == "SpeedUp")
        {
            SpeedUpBuff speedUpBuff = StartSpeedUp();
            //��������
            Vector3 pos = new Vector3(msg.x, msg.y, msg.z);
            Vector3 rot = new Vector3(msg.ex, msg.ey, msg.ez);
            speedUpBuff.transform.position = pos;
            speedUpBuff.transform.eulerAngles = rot;
        }
        else if (mFireid == "JumpUp")
        {
            JumpUpBuff jumpUpBuff = StartJumpUp();
            //��������
            Vector3 pos = new Vector3(msg.x, msg.y, msg.z);
            Vector3 rot = new Vector3(msg.ex, msg.ey, msg.ez);
            jumpUpBuff.transform.position = pos;
            jumpUpBuff.transform.eulerAngles = rot;
        }
        else if (mFireid == "Super")
        {
            SuperBuff superBuff = StartSuper();
            //��������
            Vector3 pos = new Vector3(msg.x, msg.y, msg.z);
            Vector3 rot = new Vector3(msg.ex, msg.ey, msg.ez);
            superBuff.transform.position = pos;
            superBuff.transform.eulerAngles = rot;
        }

        else if (mFireid == "BananaAttack")
        {
            Banana banana = SpawnBanana();
            //��������
            Vector3 pos = new Vector3(msg.x, msg.y, msg.z);
            Vector3 rot = new Vector3(msg.ex, msg.ey, msg.ez);
            banana.transform.position = pos;
            banana.transform.eulerAngles = rot;
        }
    }
}
