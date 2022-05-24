using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // ����
        if (!IsOnGround())
        {
            Debug.Log("�㽶Ƥ�����䣡!!!!!!!!!!!!!!!!!!!!!!!!!");
            transform.position -= transform.up * 2f * Time.deltaTime;
        }
    }

    private bool IsOnGround()
    {
        LayerMask groundLayer = LayerMask.GetMask("Default") | LayerMask.GetMask("Ground");
        Debug.Log("�㽶Ƥ�ڵ��ϣ�!!!!!!!!!!!!!!!!!!!!!!!!!");
        //LayerMask groundLayer = 1 << 3;
        Debug.Log("Layer��" + groundLayer.ToString());
        CapsuleCollider capsuleCollider = gameObject.GetComponent<CapsuleCollider>();
        float height = capsuleCollider.height;
        Debug.Log("height:" + height);
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), 0.47f, groundLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
