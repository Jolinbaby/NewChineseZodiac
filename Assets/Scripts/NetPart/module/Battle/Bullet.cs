using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class Bullet : MonoBehaviour
{
    //�ƶ��ٶ�
    public float speed = 50f;
    //������
    public BaseAnimal animal;
    //�ڵ�ģ��
    private GameObject skin;
    //����
    Rigidbody rigidBody;
    //�����
    private Transform aimCameraTransform;
    //�ܷ�ﵽ����
    public bool canHit;
    //Ŀ���
    public Vector3 target;
    //ֻ���һ��
    private bool flag;
    //��ʼ��
    public void Init()
    {
        //Ƥ��
        GameObject skinRes = ResManager.LoadPrefab("RockToAttack");
        skin = (GameObject)Instantiate(skinRes);
        Vector3 Firepos = new Vector3(0.7f,1.1f,1.6f);
        skin.transform.parent = this.transform;
        skin.transform.localPosition = Firepos;
        skin.transform.localEulerAngles = Vector3.zero;
        //����
        rigidBody = gameObject.AddComponent<Rigidbody>();
        rigidBody.useGravity = false;
        //aimCameraTransform = animal.gameObject.transform.Find("PlayerAimCamera").gameObject.transform;
        canHit = false;
        //Shoot();
        //target = new Vector3(19.2f, 18f, -13f);//debug
        //Debug.DrawLine(transform.position, target, Color.red);//����һ����ɫ������  ���-�յ�
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }

    //��ײ
    void OnCollisionEnter(Collision collisionInfo)
    {
        Debug.Log("ʯͷ������ײ��������������������������������������������������������");
        //�򵽵Ķ���
        GameObject collObj = collisionInfo.gameObject;
        BaseAnimal hitanimal = collObj.GetComponent<BaseAnimal>();
        
        //���ܴ��Լ�
        if (hitanimal == animal)
        {
            return;
        }
        //����������
        if (hitanimal != null)
        {
            SendMsgHit(animal, hitanimal);
            if(GameMain.isOnline == false)
                collisionInfo.gameObject.GetComponent<Animator>().SetTrigger("Dizzy");//
        }
        //��ʾ��ըЧ��
        GameObject explode = ResManager.LoadPrefab("BulletFatExplosionFire");
        //if (hitanimal != null) 
        //collisionInfo.gameObject.GetComponent<Animator>().SetTrigger("Dizzy");//

        Vector3 Firepos = new Vector3(0, 0f, 0f);
        GameObject exObj =  Instantiate(explode, transform.position+Firepos, explode.transform.rotation);

        //�ݻ�����
        Destroy(exObj, 1f);
        Destroy(gameObject);
    }

    //�����˺�Э��
    void SendMsgHit(BaseAnimal animal, BaseAnimal hitanimal)
    {
        
        if (hitanimal == null || animal == null)
        {
            return;
        }
        //�����Լ��������ڵ�/ʯͷ
        if (animal.id != GameMain.id)
        {
            return;
        }
        MsgHit msg = new MsgHit();
        msg.targetId = hitanimal.id;
        msg.id = animal.id;
        msg.x = transform.position.x;
        msg.y = transform.position.y;
        msg.z = transform.position.z;

        msg.Fireid = "QAttack";
        NetManager.Send(msg);
    }
}
