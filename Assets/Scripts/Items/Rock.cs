using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    private Rigidbody rb;

    [Header("基本设置")]
    public float force = 8.0f;

    public float distoryTime = 2.0f;
    private GameObject[] players;
    private GameObject player;
    public enum RockStates { HitOtherPlayer, HitNothing };
    private RockStates rockStates;

    //public float angle = 30;//仰角
    //public float totalTime = 1.5f;//投掷时间
    //private float volocityY = 0;
    //private float volocityX = 0;
    //private float accumulateTime = 0;
    //public GameObject target;

    private Vector3 direction;
    // Start is called before the first frame update

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // 生成时，状态为攻击其他玩家
        rockStates = RockStates.HitOtherPlayer;
        players = GameObject.FindGameObjectsWithTag("Player"); //查找Player
        player = players[0];
        BeThrowed();
        // 如果不砸到其他玩家，生成一段时间后消失
        Destroy(gameObject,distoryTime);
    }

    /// <summary>
    /// 不锁定目标，直接朝前方丢
    /// </summary>
    public void BeThrowed()
    {
        SoundManager.Instance.OnThrowAudio();
        // 前方：角色前方
        direction = player.transform.forward.normalized;
        Debug.Log("投掷方向：" + direction);
        rb.AddForce(direction * force, ForceMode.Impulse);
    }

    //public void FlyToTarget()
    //{
    //    direction = (target.transform.position - transform.position).normalized;
    //    //未写：根据距离来调整力的大小
    //    rb.AddForce(direction * force, ForceMode.Impulse);
    //}

    /// <summary>
    /// 已知仰角和总时间，水平方向是匀速运动，抛物到指定位置的运动
    /// </summary>
    //public void FlyToTarget()
    //{

    //}
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("石头砸到其他玩家！");
        switch (rockStates)
        {
            case RockStates.HitOtherPlayer:
                if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Character"))
                {
                    Debug.Log("其他玩家眩晕！");
                    // 砸到角色后消失
                    Destroy(gameObject, distoryTime);
                    other.gameObject.GetComponent<Animator>().SetTrigger("Dizzy");
                    rockStates = RockStates.HitNothing;
                }
                break;
        }
    }

}
