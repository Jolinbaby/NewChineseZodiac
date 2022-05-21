using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldProp : MonoBehaviour
{
    [SerializeField]
    private float curDissolve;//0-1
    [SerializeField]
    private float DissolveSpeed = 0.001f;//0-1

    //使用者
    public BaseAnimal animal;
    //护盾模型
    private GameObject shield;
    //护盾material
    private Material shieldMaterial;
    //护盾是否存在
    [HideInInspector]
    public bool isExist = false;
    //碰撞体trigger
    private Collider collider;//效果未写
    //生成位置
    private PlayerNodePosition nodePosition;

   
    public void Init()
    {
        GameObject skinRes = ResManager.LoadPrefab("BullBubblePos");
        shield = (GameObject)Instantiate(skinRes);
        shield.transform.parent = this.transform;
        shield.transform.parent = this.transform;
        shield.transform.localPosition = Vector3.zero;

        //GameObject Bubbleshield = GameObject.Find("Bubble Shield");
        //shieldMaterial = Bubbleshield.GetComponent<MeshRenderer>().material;

        MeshRenderer meshRenderer = gameObject.GetComponentInChildren<MeshRenderer>();
        shieldMaterial = meshRenderer.material;
        //这样找不知道会不会也有bug  
        //shieldMaterial = this.transform.Find("Bubble Shield").GetComponent<MeshRenderer>().material;


        //GameObject Bubbleshield = GameObject.Find("Bubble Shield");
        //Transform Bubbleshieldtrans = this.transform; 
        //GameObject.FindChild("Bubble Shield");
        //shieldMaterial = Bubbleshield.GetComponent<MeshRenderer>().material;
        curDissolve = 1;
        shieldMaterial.SetFloat("_Disolve", curDissolve);
        DissolveSpeed = 1f;
        isExist = true;

        //startShield();//产生护盾
        

        //isExist = true;
        //// 碰撞
        //collider = GetComponent<Collider>();
        //// 溶解效果初始化
        //curDissolve = 0;
        //// shader
        //shieldMaterial = gameObject.GetComponent<MeshRenderer>().material;
        //shieldMaterial.SetFloat("_Disolve", curDissolve);//dissolve真不是我写错的！我不背锅！不过懒得改了！
        //// 生成位置
        //nodePosition = animal.GetComponent<PlayerNodePosition>();
        //nodePosition.bubblePos.SetActive(true);
        //Debug.Log("生成护盾！！！！！！！！！！！！！！！！！！！！！！");
    }

    public void getAnimalType()
    {
        switch (animal.camp)
        {
            case 1:
                shield.transform.localScale = new Vector3(1.8f, 1.8f, 1.8f);
                break;
        }
    }

    void Update()
    {
        if (isExist)
        {
            startShield();//产生护盾
        }
        //if (!isExist)
        //{
        //    endShield();//护盾效果消失
        //}
    }

    private void startShield()
    {
        if(curDissolve >= 0)
        {
            curDissolve -= DissolveSpeed * Time.deltaTime;
            shieldMaterial.SetFloat("_Disolve", curDissolve);
        }
    }

    //private void endShield()
    //{
    //    if (curDissolve >= 0)
    //    {
    //        curDissolve -= DissolveSpeed * Time.deltaTime;
    //        shieldMaterial.SetFloat("_Disolve", curDissolve);
    //    }
    //}

    //发送护盾协议
    void SendMsgProtect(BaseAnimal animal)
    {

    }
}