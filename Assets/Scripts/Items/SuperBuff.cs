using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperBuff : MonoBehaviour
{
    public BaseAnimal animal;
    private GameObject superObj;
    public float superTime;
    public bool isSuper;

    public GameObject buffpos;

    private void Start()
    {

    }

    private void DestorySelf()
    {
        Debug.Log("结束无敌状态");
        isSuper = false;
        animal.isSuperState = false;
        //摧毁自身
        Destroy(superObj);
    }

    public void Init()
    {
        Debug.Log("进入无敌状态");
        superTime = 5.0f;
        isSuper = true;
        animal.isSuperState = true;

        GameObject skinRes = ResManager.LoadPrefab("BullBuffPos");
        superObj = (GameObject)Instantiate(skinRes);
        GameObject buffsuper = superObj.transform.Find("BuffSuper").gameObject;
        //buffsuper.transform.Find("BuffSupperBottom").gameObject.GetComponent<ParticleSystem>().Play();
        //buffsuper.transform.Find("BuffSuperAround").gameObject.GetComponent<ParticleSystem>().Play();
        buffsuper.transform.Find("BuffSupperBottom").gameObject.GetComponent<ParticleSystem>().Play();
        //buffsuper.transform.Find("BuffSuperAround").gameObject.GetComponent<ParticleSystem>().Play();
        superObj.transform.parent = this.transform;
        superObj.transform.parent = this.transform;
        superObj.transform.localPosition = Vector3.zero;
        
        Invoke("DestorySelf", superTime);
    }

    // Update is called once per frame
    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.X))
    //    {
    //        Init();
    //    }
    //}
}
