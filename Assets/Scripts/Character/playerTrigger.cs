using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("TreasureBox"))
        {
            Debug.Log("碰到了宝箱");
            Destroy(other); // 销毁物体
            GameManager.GameOver(true);
        }

        if (other.CompareTag("Key"))
        {
            Debug.Log("拿到了钥匙");
            CtrlAnimal ctrlAnimal = gameObject.GetComponent<CtrlAnimal>();
            ctrlAnimal.isGetKey = true;
            Destroy(other);
            // 钥匙UI显示，放在GameManager还是新建一个UI捏？
        }
    }
}
