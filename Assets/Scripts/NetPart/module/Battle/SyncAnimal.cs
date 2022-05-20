using PlayerControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncAnimal : BaseAnimal
{
    //预测信息，哪个时间到达哪个位置
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
    //重写Init
    public override void Init(string skinPath)
    {
        base.Init(skinPath);

        //不受物理运动影响
        rigidBody.constraints = RigidbodyConstraints.FreezeAll; //这个可能要改 因为rigibody不是我们加上去的
        rigidBody.useGravity = false;
        //初始化预测信息
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
        //更新位置
        ForecastUpdate();
        //UpdateAnim();
    }

    public void UpdateAnim()
    {

    }

    //移动同步
    public void SyncPos(MsgSyncAnimal msg)
    {
        //预测位置
        Vector3 pos = new Vector3(msg.x, msg.y, msg.z);
        Vector3 rot = new Vector3(msg.ex, msg.ey, msg.ez);
        //forecastPos = pos + 2*(pos - lastPos);
        //forecastRot = rot + 2*(rot - lastRot);
        forecastPos = pos;  //跟随不预测
        forecastRot = rot;
        //forecastTurretY = msg.turretY;
        //forecastGunX = msg.gunX;
        //更新
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

    //更新位置
    public void ForecastUpdate()
    {
        //时间
        float t = (Time.time - forecastTime) / CtrlAnimal.syncInterval;
        t = Mathf.Clamp(t, 0f, 1f);
        //位置
        Vector3 pos = transform.position;
        pos = Vector3.Lerp(pos, forecastPos, t);
        transform.position = pos;
        //旋转
        Quaternion quat = transform.rotation;
        Quaternion forcastQuat = Quaternion.Euler(forecastRot);
        quat = Quaternion.Lerp(quat, forcastQuat, t);
        transform.rotation = quat;
        //轮子旋转，履带滚动
        //float axis = transform.InverseTransformPoint(forecastPos).z;
        //axis = Mathf.Clamp(axis * 1024, -1f, 1f);
       // WheelUpdate(axis);
        //炮管
        //Vector3 le = turret.localEulerAngles;
        //le.y = Mathf.LerpAngle(le.y, forecastTurretY, t);
        //turret.localEulerAngles = le;
        ////炮塔
        //le = gun.localEulerAngles;
        //le.x = Mathf.LerpAngle(le.x, forecastGunX, t);
        //gun.localEulerAngles = le;
    }

    //开火
    public void SyncFire(MsgFire msg)
    {
        //Debug.Log(" SyncFire---------------------"+msg);
        string mFireid = msg.Fireid;
        if(mFireid== "QAttack")
        {
            Bullet bullet = Fire();
            //更新坐标
            Vector3 pos = new Vector3(msg.x, msg.y, msg.z);
            Vector3 rot = new Vector3(msg.ex, msg.ey, msg.ez);
            bullet.transform.position = pos;
            bullet.transform.eulerAngles = rot;

        }
        else if (mFireid == "BombAttack")
        {
            Bomb bomb = FireBomb();
            //更新坐标
            Vector3 pos = new Vector3(msg.x, msg.y, msg.z);
            Vector3 rot = new Vector3(msg.ex, msg.ey, msg.ez);
            bomb.transform.position = pos;
            bomb.transform.eulerAngles = rot;
        }

    }
}
