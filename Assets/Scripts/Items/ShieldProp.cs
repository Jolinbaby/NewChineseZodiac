using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldProp : MonoBehaviour
{
    [SerializeField]
    private float curDissolve;//0-1
    [SerializeField]
    private float DissolveSpeed = 10f;//0-1

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

    public void Init()
    {
        isExist = true;
        // ��ײ
        collider = GetComponent<Collider>();
        // �ܽ�Ч����ʼ��
        curDissolve = 0;
        // shader
        shieldMaterial = gameObject.GetComponent<MeshRenderer>().material;
        shieldMaterial.SetFloat("_Disolve", curDissolve);//dissolve�治����д��ģ��Ҳ��������������ø��ˣ�
        // ����λ��
        nodePosition = animal.GetComponent<PlayerNodePosition>();
        nodePosition.bubblePos.SetActive(true);
        Debug.Log("���ɻ��ܣ�������������������������������������������");
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
            startShield();//��������
        }
        if (!isExist)
        {
            endShield();//����Ч����ʧ
        }
    }

    private void startShield()
    {
        if (curDissolve <= 1)
        {
            curDissolve += DissolveSpeed * Time.deltaTime;
            shieldMaterial.SetFloat("_Disolve", curDissolve);
        }
    }

    private void endShield()
    {
        if (curDissolve >= 0)
        {
            curDissolve -= DissolveSpeed * Time.deltaTime;
            shieldMaterial.SetFloat("_Disolve", curDissolve);
        }
    }

    //���ͻ���Э��
    void SendMsgProtect(BaseAnimal animal)
    {

    }
}