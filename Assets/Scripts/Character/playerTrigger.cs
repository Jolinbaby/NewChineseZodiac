using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("TreasureBox"))
        {
            Debug.Log("�����˱���");
            Destroy(other); // ��������
            GameManager.GameOver(true);
        }

        if (other.CompareTag("Key"))
        {
            Debug.Log("�õ���Կ��");
            CtrlAnimal ctrlAnimal = gameObject.GetComponent<CtrlAnimal>();
            ctrlAnimal.isGetKey = true;
            Destroy(other);
            // Կ��UI��ʾ������GameManager�����½�һ��UI��
        }
    }
}
