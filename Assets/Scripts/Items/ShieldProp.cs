using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldProp : MonoBehaviour
{
    [SerializeField]
    private float curDissolve;//0-1
    [SerializeField]
    private float DissolveSpeed = 0.003f;//0-1

    //ʹ����
    public BaseAnimal animal;
    //����ģ��
    private GameObject shield;
    //����material
    private Material shieldMaterial;
    //�����Ƿ����
    [HideInInspector]
    public bool isExist = false;
    //��ײ��trigger
    private Collider collider;//Ч��δд
    //����λ��
    private PlayerNodePosition nodePosition;

    private bool isBreaking=false;
   
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
        //�����Ҳ�֪���᲻��Ҳ��bug  
        //shieldMaterial = this.transform.Find("Bubble Shield").GetComponent<MeshRenderer>().material;


        //GameObject Bubbleshield = GameObject.Find("Bubble Shield");
        //Transform Bubbleshieldtrans = this.transform; 
        //GameObject.FindChild("Bubble Shield");
        //shieldMaterial = Bubbleshield.GetComponent<MeshRenderer>().material;
        curDissolve = 1;
        shieldMaterial.SetFloat("_Disolve", curDissolve);
        DissolveSpeed = 1f;
        isExist = true;
        animal.isShieldProtect = true;
        //Destroy(this.gameObject, 3f);

        //startShield();//��������
        Invoke("Breakshield", 5f);

        //isExist = true;
        //// ��ײ
        //collider = GetComponent<Collider>();
        //// �ܽ�Ч����ʼ��
        //curDissolve = 0;
        //// shader
        //shieldMaterial = gameObject.GetComponent<MeshRenderer>().material;
        //shieldMaterial.SetFloat("_Disolve", curDissolve);//dissolve�治����д��ģ��Ҳ��������������ø��ˣ�
        //// ����λ��
        //nodePosition = animal.GetComponent<PlayerNodePosition>();
        //nodePosition.bubblePos.SetActive(true);
        //Debug.Log("���ɻ��ܣ�������������������������������������������");
    }

    //public void getAnimalType()
    //{
    //    switch (animal.camp)
    //    {
    //        case 1:
    //            shield.transform.localScale = new Vector3(1.8f, 1.8f, 1.8f);
    //            break;
    //    }
    //}


    public void Breakshield()
    {
        isExist = false;
        Debug.Log("breakShield");
        isBreaking = true;
        curDissolve = -0.1f;
    }

    void Update()
    {
        if (isExist)
        {
            startShield();//��������
        }
        if(isBreaking)
        {
            endShield();
        }
        //if (!isExist)
        //{
        //    endShield();//����Ч����ʧ
        //}
    }

    private void startShield()
    {
        if(curDissolve >= 0)
        {
            curDissolve -= DissolveSpeed * Time.deltaTime;
            shieldMaterial.SetFloat("_Disolve", curDissolve);
        }
        else
        {
            isExist = false;
        }
        //isExist = false;
    }

    private void endShield()
    {
        if (curDissolve <= 1)
        {
            curDissolve += DissolveSpeed * Time.deltaTime;
            shieldMaterial.SetFloat("_Disolve", curDissolve);
        }
        else
        {
            isBreaking = false;
            animal.isShieldProtect = false;
            Destroy(this.gameObject);
        }
    }

    ////���ͻ���Э��
    //void SendMsgProtect(BaseAnimal animal)
    //{

    //}
}