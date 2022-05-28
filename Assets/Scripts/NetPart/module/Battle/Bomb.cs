using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    //�ƶ��ٶ�
    public float speed = 10f;
    public float upspeed = 13f;

    private Rigidbody rb;

    [Header("��������")]
    public float force = 10.0f;
    //public Vector3 velocity;
    public float angle=45;//����
    [Header("ʹ��Ч��")]
    public GameObject expPrefab;
    //public float expForce;
    //������Χ
    public float radius;
    [Header("Ч��ʱ��")]
    public float effectTime = 3.0f;
    //private GameObject[] players;
    //private GameObject player;
    public enum BombStates { HitOtherPlayer, HitNothing };
    private BombStates bombStates;

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
        GameObject skinRes = ResManager.LoadPrefab("Bomb");
        skin = (GameObject)Instantiate(skinRes);
        Vector3 Firepos = new Vector3(0, 1.75f, 2.34f);
        skin.transform.parent = this.transform;
        skin.transform.localPosition = Firepos;
        skin.transform.localEulerAngles = Vector3.zero;
        //����
        rigidBody = gameObject.AddComponent<Rigidbody>();
        //rigidBody.useGravity = false;

        Debug.Log("enterBomb");
        // ����ʱ��״̬Ϊ����������� ??
        bombStates = BombStates.HitOtherPlayer;
        //players = GameObject.FindGameObjectsWithTag("Player"); //����Player
        //player = players[0];

        // ���ǣ�45��
        //direction = animal.transform.forward.normalized + Mathf.Tan(angle) * Vector3.up;
        //Debug.Log("Ͷ������" + direction);
        //rb.AddForce(direction * force, ForceMode.Impulse);
        //BeThrowed();
    }
    void Update()
    {
        //��ǰ�ƶ�
        transform.position += transform.forward * speed * Time.deltaTime;
        transform.position += transform.up * upspeed * Time.deltaTime;
        upspeed -= 0.05f;
    }
    //}

    /// <summary>
    /// ������֪���Ǻͳ�ʼ�ٶȷ���
    /// </summary>
    public void BeThrowed()
    {
        // ���ǣ�45��
        direction = animal.transform.forward.normalized + Mathf.Tan(angle) * Vector3.up;
        Debug.Log("Ͷ������" + direction);
        rb.AddForce(direction * force, ForceMode.Impulse);
    }
    //��ײ
    void OnCollisionEnter(Collision collisionInfo)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
         foreach (Collider nearby in colliders)
         {
            BaseAnimal syncAnimal = nearby.gameObject.GetComponent<BaseAnimal>();

            if (syncAnimal != animal&&syncAnimal != null)
             {
                
                SendMsgHit(animal, syncAnimal);
                Debug.Log("�������ѣ�Σ�");
                if(GameMain.isOnline == false)
                    nearby.gameObject.GetComponent<Animator>().SetTrigger("BeBomb");
                
             }
         }
        //��ʾ��ըЧ��
        GameObject explode = ResManager.LoadPrefab("Explosion");
        //if (hitanimal != null) 
        //collisionInfo.gameObject.GetComponent<Animator>().SetTrigger("BeBomb");//

        Vector3 Firepos = new Vector3(0, 1.5f, 0f);
        Instantiate(explode, transform.position + Firepos, explode.transform.rotation);

        //�ݻ�����
        Destroy(gameObject);

    }

    ////��ײ
    //void OnCollisionEnter(Collision collisionInfo)
    //{
    //    //�򵽵Ķ���
    //    GameObject collObj = collisionInfo.gameObject;
    //    BaseAnimal hitanimal = collObj.GetComponent<BaseAnimal>();

    //    //���ܴ��Լ�
    //    if (hitanimal == animal)
    //    {
    //        return;
    //    }
    //    //����������
    //    if (hitanimal != null)
    //    {
    //        SendMsgHit(animal, hitanimal);
    //        collisionInfo.gameObject.GetComponent<Animator>().SetTrigger("BeBomb");//
    //    }
    //    //��ʾ��ըЧ��
    //    GameObject explode = ResManager.LoadPrefab("WFX_ExplosiveSmoke Big Alt");
    //    //if (hitanimal != null) 
    //    //collisionInfo.gameObject.GetComponent<Animator>().SetTrigger("BeBomb");//

    //    Vector3 Firepos = new Vector3(0, 1.75f, 2.34f);
    //    Instantiate(explode, transform.position + Firepos, transform.rotation);

    //    //�ݻ�����
    //    Destroy(gameObject);
    //}

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

        msg.Fireid = "BombAttack";
        NetManager.Send(msg);
    }
    //private void OnCollisionEnter(Collision other)
    //{


    //    switch (bombStates)
    //    {
    //        case BombStates.HitOtherPlayer:
    //            Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
    //            foreach (Collider nearby in colliders)
    //            {
    //                SyncAnimal syncAnimal = nearby.gameObject.GetComponent<SyncAnimal>();
    //                //if (nearby.gameObject.CompareTag("Player"))
    //                if (syncAnimal!=null)
    //                {
    //                    Debug.Log("�������ѣ�Σ�");
    //                    nearby.gameObject.GetComponent<Animator>().SetTrigger("BeBomb");
    //                    bombStates = BombStates.HitNothing;
    //                }
    //            }
    //            break;
    //    }

    //    //��ʾ��ըЧ��
    //    GameObject explode = ResManager.LoadPrefab("WFX_Explosion");
    //    Vector3 Firepos = new Vector3(0, 1.75f, 2.34f);
    //    GameObject exp = Instantiate(explode, transform.position + Firepos, transform.rotation);
    //    Destroy(exp, effectTime);

    //    // ��ըЧ��
    //    //GameObject exp = Instantiate(expPrefab, transform.position, Quaternion.identity);
    //    //Destroy(exp, effectTime);

    //    // ��ը����ʧ
    //    Destroy(gameObject);
    //}

    ///// <summary>
    ///// ���ܱ߸����������
    ///// </summary>
    //void KnockBack()
    //{
    //    Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

    //    int num = 0;
    //    foreach(Collider nearby in colliders)
    //    {
    //        num += 1;
    //        Debug.Log("��⵽��ײ��" + num);
    //        Rigidbody rigg = nearby.GetComponent<Rigidbody>();
    //        if (rigg != null)
    //        {
    //            rigg.AddExplosionForce(expForce, transform.position, radius);
    //        }
    //    }
    //}
}
