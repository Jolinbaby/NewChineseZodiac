using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldProp : MonoBehaviour
{
    [SerializeField]
    private float curDissolve;//0-1
    [SerializeField]
    private float DissolveSpeed = 10f;//0-1

    //使用者
    public BaseAnimal animal;
    //护盾模型
    private GameObject shield;

    public void Init()
    {
        // 加载预制体
        GameObject shieldRes = ResManager.LoadPrefab("Shield");
        shield = (GameObject)Instantiate(shieldRes);
        Vector3 Spawnpos = new Vector3(0, 0, 0);
        shield.transform.parent = this.transform;
        shield.transform.localPosition = Spawnpos;
        shield.transform.localEulerAngles = Vector3.zero;
        Debug.Log("生成护盾！！！！！！！！！！！！！！！！！！！！！！");
        // 溶解效果初始化
        curDissolve = 0;
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

    // Update is called once per frame
    void Update()
    {
        if (curDissolve <= 1)
        {
            curDissolve += DissolveSpeed * Time.deltaTime;
        }
    }
}
