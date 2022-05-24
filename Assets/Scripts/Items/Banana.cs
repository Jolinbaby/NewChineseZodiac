using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana : MonoBehaviour
{

    //ʹ����
    public BaseAnimal animal;
    //�㽶Ƥģ��
    private GameObject bananaObj;
    //Ч��ʱ��
    public float effectTime;
    //�ж��Ƿ�ȵ�
    //public bool isSpeedUp;
    //���ٴ�С
    public float cutSpeed;
    //Ч���Ƿ���
    public bool flag;
    //�Ƿ���ڵ���
    public bool isGrounded;
    //ֻҪ�ж�һ�ξͺ�
    public int time;

    public void Init()
    {
        flag = false;
        time = 0;
        GameObject skinRes = ResManager.LoadPrefab("Banana");
        bananaObj = (GameObject)Instantiate(skinRes);
        bananaObj.transform.parent = this.transform;
        bananaObj.transform.localPosition = Vector3.zero;

        Debug.Log("Init�㽶Ƥ");
        Invoke("StartEffect", 3f);
    }
    public void StartEffect()
    {
        flag = true;
    }

    void Update()
    {
        if (time == 0)
        {
            isGrounded = IsOnGround();
            // ����
            if (!IsOnGround())
            {
                Debug.Log("�㽶Ƥ�����䣡!!!!!!!!!!!!!!!!!!!!!!!!!");
                transform.position -= transform.up * 2f * Time.deltaTime;
            }
        }
    }

    private bool IsOnGround()
    {
        LayerMask groundLayer = LayerMask.GetMask("Default") | LayerMask.GetMask("Ground");
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), 0.47f, groundLayer))
        {
            time++;
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// �����Ƿ�����㽶Ƥ�ı��˻���������ң�ֻҪ�ȵ��ͻỬ��
    /// Ϊ�˲�������ڷŵ�ʱ��ͱ�����������һ����ʼʱ��
    /// Ҳ�����㽶Ƥ�շ���ȥ��3s�ڲ��ᴥ��
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (!flag)
        {
            return;
        }
        BaseAnimal baseAnimal = other.gameObject.GetComponent<BaseAnimal>();

        if (baseAnimal != null)
        {
            SendMsgHit(animal, baseAnimal);
            Debug.Log("����Ҳȵ��㽶Ƥ");
            if (GameMain.isOnline == false)
                other.gameObject.GetComponent<Animator>().SetTrigger("BeBanana");
        }
        Destroy(gameObject);
    }

    //�����˺�Э��
    void SendMsgHit(BaseAnimal animal, BaseAnimal hitanimal)
    {

        if (hitanimal == null || animal == null)
        {
            return;
        }
        ////�����Լ��������ڵ�/ʯͷ
        //if (animal.id != GameMain.id)
        //{
        //    return;
        //}
        MsgHit msg = new MsgHit();
        msg.targetId = hitanimal.id;
        msg.id = animal.id;
        msg.x = transform.position.x;
        msg.y = transform.position.y;
        msg.z = transform.position.z;

        msg.Fireid = "BananaAttack";
        NetManager.Send(msg);
    }
}
