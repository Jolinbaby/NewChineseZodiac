using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    private Rigidbody rb;
    private void Update()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void beThrowed()
    {
        rb.AddForce(transform.up * 2f);
    }
}
