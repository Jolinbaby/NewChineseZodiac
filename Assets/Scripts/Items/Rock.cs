using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    private Rigidbody rb;

    [Header("��������")]
    public float force = 8.0f;

    public float distoryTime = 2.0f;
    private GameObject[] players;
    private GameObject player;
    public enum RockStates { HitOtherPlayer, HitNothing };
    private RockStates rockStates;

    //public float angle = 30;//����
    //public float totalTime = 1.5f;//Ͷ��ʱ��
    //private float volocityY = 0;
    //private float volocityX = 0;
    //private float accumulateTime = 0;
    //public GameObject target;

    private Vector3 direction;
    // Start is called before the first frame update

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // ����ʱ��״̬Ϊ�����������
        rockStates = RockStates.HitOtherPlayer;
        players = GameObject.FindGameObjectsWithTag("Player"); //����Player
        player = players[0];
        BeThrowed();
        // ������ҵ�������ң�����һ��ʱ�����ʧ
        Destroy(gameObject,distoryTime);
    }

    /// <summary>
    /// ������Ŀ�ֱ꣬�ӳ�ǰ����
    /// </summary>
    public void BeThrowed()
    {
        SoundManager.Instance.OnThrowAudio();
        // ǰ������ɫǰ��
        direction = player.transform.forward.normalized;
        Debug.Log("Ͷ������" + direction);
        rb.AddForce(direction * force, ForceMode.Impulse);
    }

    //public void FlyToTarget()
    //{
    //    direction = (target.transform.position - transform.position).normalized;
    //    //δд�����ݾ������������Ĵ�С
    //    rb.AddForce(direction * force, ForceMode.Impulse);
    //}

    /// <summary>
    /// ��֪���Ǻ���ʱ�䣬ˮƽ�����������˶������ﵽָ��λ�õ��˶�
    /// </summary>
    //public void FlyToTarget()
    //{

    //}
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("ʯͷ�ҵ�������ң�");
        switch (rockStates)
        {
            case RockStates.HitOtherPlayer:
                if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Character"))
                {
                    Debug.Log("�������ѣ�Σ�");
                    // �ҵ���ɫ����ʧ
                    Destroy(gameObject, distoryTime);
                    other.gameObject.GetComponent<Animator>().SetTrigger("Dizzy");
                    rockStates = RockStates.HitNothing;
                }
                else
                {
                    Invoke("SelfDestroy", 5);
                }
                break;
        }
    }

    void SelfDestroy()
    {
        Debug.Log("5s����");
        Destroy(gameObject);
        Debug.Log("5s�Ѿ�����");
    }
}
