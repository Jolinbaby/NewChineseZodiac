using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deepground : MonoBehaviour
{
    Vector3 birpos;
    // Start is called before the first frame update
    void Start()
    {
        birpos = new Vector3(47.8f, 61f, -94f);   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //private void OnCollisionEnter(Collision collision)
    //{
    //    CtrlAnimal ctrlAnimal = collision.gameObject.GetComponent<CtrlAnimal>();
    //    if(ctrlAnimal!=null)
    //    {
    //        Debug.Log("deep in ground");
    //        collision.gameObject.transform.position = birpos;
    //    }
    //}
    private void OnTriggerEnter(Collider collision)
    {
        CtrlAnimal ctrlAnimal = collision.gameObject.GetComponent<CtrlAnimal>();
        if (ctrlAnimal != null)
        {
            //ctrlAnimal.Getupup();
            Debug.Log("deep in ground");
           // collision.gameObject.transform.position = birpos;
        }
    }
}
