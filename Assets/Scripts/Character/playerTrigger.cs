using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("TreasureBox"))
        {

            CtrlAnimal ctrlAnimal = gameObject.GetComponent<CtrlAnimal>();
            if(ctrlAnimal.isGetKey==true)
            {
                Debug.Log("�����˱���");
                Destroy(other); // ��������
                MsgWin msg = new MsgWin();
                NetManager.Send(msg);
                GameManager.GameOver(true);
            }
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
