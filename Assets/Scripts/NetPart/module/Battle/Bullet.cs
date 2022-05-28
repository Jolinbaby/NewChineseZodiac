using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    //��ʼ��
    public void Init()
    {
        //Ƥ��
        GameObject skinRes = ResManager.LoadPrefab("RockToAttack");
        skin = (GameObject)Instantiate(skinRes);
        Vector3 Firepos = new Vector3(0,1.75f,2.34f);
        skin.transform.parent = this.transform;
        skin.transform.localPosition = Firepos;
        skin.transform.localEulerAngles = Vector3.zero;
        //����
        rigidBody = gameObject.AddComponent<Rigidbody>();
        rigidBody.useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {
        //��ǰ�ƶ�
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    //��ײ
    void OnCollisionEnter(Collision collisionInfo)
    {
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
        Instantiate(explode, transform.position+Firepos, explode.transform.rotation);

        //�ݻ�����
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
