using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpBuff : MonoBehaviour
{
    //使用者
    public BaseAnimal animal;
    //加速buff模型
    private GameObject speedupObj;
    //加速时长
    public float speedUpTime;
    //判断是否加速
    public bool isSpeedUp;
    //加速大小
    public float addSpeed;


    private void DestorySelf()
    {
        Debug.Log("回到原速");
        isSpeedUp = false;
        animal.GetComponent<PlayerControl.ThirdPersonController>().MoveSpeed -= addSpeed;
        animal.GetComponent<PlayerControl.ThirdPersonController>().SprintSpeed -= addSpeed;
        //摧毁自身
        Destroy(speedupObj);
    }

    public void Init()
    {
        speedUpTime = 3.0f;
        addSpeed = 5.0f;
        isSpeedUp = true;

        GameObject skinRes = ResManager.LoadPrefab("BullBuffPos");
        speedupObj = (GameObject)Instantiate(skinRes);
        speedupObj.transform.Find("BuffSpeedUP").gameObject.GetComponent<ParticleSystem>().Play();
        speedupObj.transform.parent = this.transform;
        speedupObj.transform.parent = this.transform;
        speedupObj.transform.localPosition = Vector3.zero;

        animal.GetComponent<PlayerControl.ThirdPersonController>().MoveSpeed += addSpeed;
        animal.GetComponent<PlayerControl.ThirdPersonController>().SprintSpeed += addSpeed;
        Debug.Log("Init加速");

        Invoke("DestorySelf", speedUpTime);
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.X))
    //    {
    //        Init();
    //    }
    //}
}
