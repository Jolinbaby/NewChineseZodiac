using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpUpBuff : MonoBehaviour
{
    public BaseAnimal animal;
    private GameObject jumpUpObj;
    public float jumpUpTime;
    public bool isJumpUp;
    public float addHeight;

    private void DestorySelf()
    {
        Debug.Log("回到原速");
        isJumpUp = false;
        animal.GetComponent<PlayerControl.ThirdPersonController>().JumpHeight -= addHeight;
        //摧毁自身
        Destroy(jumpUpObj);
    }

    public void Init()
    {
        jumpUpTime = 5.0f;
        addHeight = 6.0f;
        isJumpUp = true;

        GameObject skinRes = ResManager.LoadPrefab("BullBuffPos");
        jumpUpObj = (GameObject)Instantiate(skinRes);
        jumpUpObj.transform.Find("BuffJumpUp").gameObject.GetComponent<ParticleSystem>().Play();
        jumpUpObj.transform.parent = this.transform;
        jumpUpObj.transform.parent = this.transform;
        jumpUpObj.transform.localPosition = Vector3.zero;

        animal.GetComponent<PlayerControl.ThirdPersonController>().JumpHeight += addHeight;

        Invoke("DestorySelf", jumpUpTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
