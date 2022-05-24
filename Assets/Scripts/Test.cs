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
        // œ¬¬‰
        if (!IsOnGround())
        {
            Debug.Log("œ„Ω∂∆§‘⁄œ¬¬‰£°!!!!!!!!!!!!!!!!!!!!!!!!!");
            transform.position -= transform.up * 2f * Time.deltaTime;
        }
    }

    private bool IsOnGround()
    {
        LayerMask groundLayer = LayerMask.GetMask("Default") | LayerMask.GetMask("Ground");
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
