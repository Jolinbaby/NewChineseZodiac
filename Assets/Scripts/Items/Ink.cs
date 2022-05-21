using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ink : MonoBehaviour
{

    //�ƶ��ٶ�
    public float speed = 5f;
    public float upspeed = 7f;

    public float radius;

    private Vector3 direction;
    //������
    public BaseAnimal animal;
    //�ڵ�ģ��
    private GameObject skin;

    //����
    Rigidbody rigidBody;

    public void Init()
    {

        radius = 10f;
        //Ƥ��
        Debug.Log("load prefab");
        GameObject skinRes = ResManager.LoadPrefab("Ink");
        Debug.Log("prefab loaded");
        skin = (GameObject)Instantiate(skinRes);
        Vector3 Firepos = new Vector3(0, 1.75f, 2.34f);
        skin.transform.parent = this.transform;
        skin.transform.localPosition = Firepos;
        skin.transform.localEulerAngles = Vector3.zero;
        //����
        rigidBody = gameObject.AddComponent<Rigidbody>();

    }
    void Update()
    {
        //��ǰ�ƶ�
        transform.position += transform.forward * speed * Time.deltaTime;
        transform.position += transform.up * upspeed * Time.deltaTime;
        upspeed -= 0.05f;
    }

    //��ײ
    void OnCollisionEnter(Collision collisionInfo)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider nearby in colliders)
        {
            BaseAnimal syncAnimal = nearby.gameObject.GetComponent<BaseAnimal>();

            if (syncAnimal != animal && syncAnimal != null)
            {

                SendMsgHit(animal, syncAnimal);
                Debug.Log("������ұ���ī��");
                if (GameMain.isOnline == false)
                    GameObject.Find("UI_Ink/Image").GetComponent<Animator>().SetTrigger("BeInk");
                
                //nearby.gameObject.GetComponent<Animator>().SetTrigger("BeInk");

            }
        }
        //��ʾ��ըЧ��
        GameObject explode = ResManager.LoadPrefab("Ink_Particle");
        //if (hitanimal != null) 
        //collisionInfo.gameObject.GetComponent<Animator>().SetTrigger("BeBomb");//

        Vector3 Firepos = new Vector3(0, 1.75f, 2.34f);
        Instantiate(explode, transform.position + Firepos, transform.rotation);

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

        msg.Fireid = "InkAttack";
        NetManager.Send(msg);
    }
}
