using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [Header("GameObject")]
    public GameObject keyObjectPrefab;
    public GameObject guessBoxPrefab;
    public GameObject rockPrefab;
    public GameObject bombPrefab;

    [Header("��ͼ��Χ")]
    public float minX = -27f;
    public float maxX = 25f;
    public float minZ = -20f;
    public float maxZ = 20f;

    [Header("���������")]
    public int maxboxNum = 100;
    //public int maxboxNum = 5;
    public float intervalTime = 2f;
    public static int curBoxNum;    // ��ǰ�����ڵ��������
    private static int totalBoxNum;// ����Ϸ��ʼ����ǰ���ɵĵ�������
    private GameObject guessBox;

    [Header("UI")]
    public Text timeScore;

    [Header("��������")]
    public AudioSource audioSource;
    [SerializeField]
    private AudioClip gameBGM_1;
    [SerializeField]
    private AudioClip gameBGM_2;

    public static float timeBegin = 0;
    private Coroutine coroutine;//����һ��Э�̱�������ȡЭ�̵�������ر�

    //������Ʒ����Ϣ
    public ItemInfo[] items;
    //����ÿ������
    public static GameObject[] boxes;
    //��ǰ���ɵ���Ʒ������ֵ
    int createItemIndex;
    int maxCreateSize;

    public static void DestroyItem(int id)
    {
        if (boxes[id - 1].gameObject != null)
        {
            Destroy(boxes[id - 1].gameObject);
        }

    }
    // ����Ʒ���ɵĸ�����Ϣ�������� 05201758
    public void SaveItemInfo(MsgEnterBattle msg)
    {
        items = new ItemInfo[msg.items.Length];
        boxes = new GameObject[msg.items.Length];
        maxCreateSize = msg.items.Length;
        for (int i = 0; i < msg.items.Length; i++) //����ص���Ʒ��Ϣ���볡����
        {
            items[i] = msg.items[i];
        }
        StartGame(); //��ʼ��Ϸ,���ɱ���
    }


    private void InitGame()
    {
        //randomKey();
        audioSource.clip = gameBGM_1;
        audioSource.Play();
        ItemManager.hasKey = false;
        RandomBoxSpawn(maxCreateSize / 2);
    }

    private void StartGame()
    {
        //guessBoxPrefab=
        // gameOverUI.SetActive(false);
        createItemIndex = 0;
        InitGame();
        //totalBoxNum = maxboxNum;
        //curBoxNum = maxboxNum;
        // �ӵ�0s��ʼ��ÿ��10s����һ��������
        //InvokeRepeating("SpawnBox", 0, intervalTime);
        StartRun(); //����Ϸһ��ʼ������
        Invoke("StopRun", 300);//Э���������ÿ��2s����һ�����ߣ�����������Ӻ���ùر�Э�̵ķ�����ʹЭ�̹رա�
    }

    //����һ����������Э�̵ķ���
    private void StartRun()
    {
        coroutine = StartCoroutine(Generate());
    }
    //����һ������ر�Э�̵ķ���
    private void StopRun()
    {
        StopCoroutine(coroutine);
    }


    /// <summary>
    /// �����������
    /// </summary>
    /// <param name="boxNum">������������</param>
    public void RandomBoxSpawn(int boxNum)
    {
        for (int i = 0; i < boxNum; i++)
        {
            RandomSingleBoxSpawn();
            //Debug.Log(guessBoxPrefab.transform.position);
        }
    }

    //������������ķ���  
    private void RandomSingleBoxSpawn()
    {
        //Debug.Log("�Զ����ɵ�����");
        // ��ȡ���λ��x,z
        float x, z;
        int id; //��Ʒ��id��,�ڱ���Ҽ���֮�����߼��ж�
        int kind;
        //x = Random.Range(minX, maxX); //-5f��(float)-5Ч��һ��
        //z = Random.Range(minZ, maxZ);
        x = items[createItemIndex].x;
        z = items[createItemIndex].z;
        id = items[createItemIndex].id;
        kind = items[createItemIndex].kind;

        // ��ʾ�ڳ�����
        boxes[createItemIndex] = Instantiate(guessBoxPrefab, new Vector3(x, 24f, z), Quaternion.identity);
        boxes[createItemIndex].GetComponent<ItemBox>().InitInfo(kind, id);

        if (kind == 4)//����,��������Կ��!
        {
            Debug.Log("generate key!!!!");
            GameObject skinRes = ResManager.LoadPrefab("getKey_Particle");
            GameObject getKeyParticle = (GameObject)Instantiate(skinRes);
            getKeyParticle.transform.position = new Vector3(x, 0f, z);
            //getKeyParticle.transform.parent = this.gameObject.transform;
        }
        //totalBoxNum++;
        //curBoxNum++;
        createItemIndex++;
    }

    //����Э�̣�����Generate������������
    private IEnumerator Generate()
    {
        while (true)//��whileд����ѭ����Э��һֱ������
        {
            yield return new WaitForEndOfFrame();
            // ���ֳ����е��߸������Ϊ20
            //if (curBoxNum <= maxboxNum)
            //{
            //    RandomSingleBoxSpawn();
            //    if (totalBoxNum > 200)
            //    {
            //        StopRun();
            //    }
            //}
            if(createItemIndex<maxCreateSize)
            {
                RandomSingleBoxSpawn();
            }
            else
            {
                StopRun();
            }
            yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(2f);
        }
            //Debug.Log("��ǰ���߸�����" + curBoxNum);
            // 
    }

    void Update()
    {
        // ��ʾʱ��
        int minute = (int)(Time.timeSinceLevelLoad - timeBegin) / 60;
        int second = (int)(Time.timeSinceLevelLoad - timeBegin) - minute * 60;
        timeScore.text = string.Format("{0:D2}:{1:D2}", minute, second);

        // ����û��Կ�ף���������
        if (totalBoxNum == 50 && !ItemManager.hasKey)
        {
            RandomBoxSpawn(1);
            guessBox.gameObject.GetComponent<ItemBox>().addKey();
            Debug.Log("ǿ������Կ�ף���������������������������������");
        }

        if (ItemManager.hasPlayerTakeKey)
        {
            audioSource.clip = gameBGM_2;
            audioSource.Play();
        }
    }

    //private void randomKey()
    //{
    //    //Debug.Log("�Զ�����Կ��");
    //    float x, z;
    //    x = Random.Range(-20f, 70f); //-5f��(float)-5Ч��һ��
    //    z = Random.Range(-20f, 20f);
    //    Instantiate(keyObject, new Vector3(x, 4f, z), Quaternion.identity);
    //    //Debug.Log(keyObject.transform.position);
    //    //Debug.Log("������");
    //}

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // ���¼��س���
        Time.timeScale = 1; // ��ʼ��¼ʱ��
    }

    public void QuitGame()
    {
        Application.Quit(); // �˳���Ϸ
    }

    static public void GameOver(bool win)
    {
        if (win)
        {
            Debug.Log("���ʤ��");
           // ��ʾ��Ϸʤ��UI
            Time.timeScale = 0f; // ʱ��ֹͣ��¼
        }
        else
        {
            Debug.Log("���ʧ��");
            // ��ʾ��Ϸʧ��UI
            Time.timeScale = 0f; // ʱ��ֹͣ��¼
        }
    }
}
