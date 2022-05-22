using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpBuff : MonoBehaviour
{
    //ʹ����
    public BaseAnimal animal;
    //����buffģ��
    private GameObject speedupObj;
    //����ʱ��
    public float speedUpTime;
    //�ж��Ƿ����
    public bool isSpeedUp;
    //���ٴ�С
    public float addSpeed;


    private void DestorySelf()
    {
        Debug.Log("�ص�ԭ��");
        isSpeedUp = false;
        animal.GetComponent<PlayerControl.ThirdPersonController>().MoveSpeed -= addSpeed;
        animal.GetComponent<PlayerControl.ThirdPersonController>().SprintSpeed -= addSpeed;
        //�ݻ�����
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
        Debug.Log("Init����");

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
