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

    [Header("地图范围")]
    public float minX = -27f;
    public float maxX = 25f;
    public float minZ = -20f;
    public float maxZ = 20f;

    [Header("随机道具箱")]
    public int maxboxNum = 100;
    //public int maxboxNum = 5;
    public float intervalTime = 2f;
    public static int curBoxNum;    // 当前场景内道具箱个数
    private static int totalBoxNum;// 从游戏开始到当前生成的道具总数
    private GameObject guessBox;

    [Header("UI")]
    public Text timeScore;

    [Header("背景音乐")]
    public AudioSource audioSource;
    [SerializeField]
    private AudioClip gameBGM_1;
    [SerializeField]
    private AudioClip gameBGM_2;

    public static float timeBegin = 0;
    private Coroutine coroutine;//定义一个协程变量来获取协程的启动与关闭

    //保存物品的信息
    public ItemInfo[] items;
    //保存每个物体
    public static GameObject[] boxes;
    //当前生成的物品的索引值
    int createItemIndex;
    int maxCreateSize;

    public static void DestroyItem(int id)
    {
        if (boxes[id - 1].gameObject != null)
        {
            Destroy(boxes[id - 1].gameObject);
        }

    }
    // 将物品生成的各个信息保存下来 05201758
    public void SaveItemInfo(MsgEnterBattle msg)
    {
        items = new ItemInfo[msg.items.Length];
        boxes = new GameObject[msg.items.Length];
        maxCreateSize = msg.items.Length;
        for (int i = 0; i < msg.items.Length; i++) //把相关的物品信息放入场景中
        {
            items[i] = msg.items[i];
        }
        StartGame(); //开始游戏,生成宝箱
    }


    private void InitGame()
    {
        //randomKey();
        audioSource.clip = gameBGM_1;
        audioSource.Play();
        ItemManager.hasKey = false;
        // 初始化道具箱数量, 现在是20个
        //RandomBoxSpawn(maxCreateSize / 10);
        RandomBoxSpawn(maxCreateSize / 3);//先弄多一点测试，之后删掉
    }

    private void StartGame()
    {
        //guessBoxPrefab=
        // gameOverUI.SetActive(false);
        createItemIndex = 0;
        InitGame();
        //totalBoxNum = maxboxNum;
        //curBoxNum = maxboxNum;
        // 从第0s开始，每隔10s生成一波道具箱
        //InvokeRepeating("SpawnBox", 0, intervalTime);
        StartRun(); //在游戏一开始就启动
        Invoke("StopRun", 300);//协程启动后会每隔2s生成一个道具，但是在五分钟后调用关闭协程的方法，使协程关闭。
    }

    //定义一个负责启动协程的方法
    private void StartRun()
    {
        coroutine = StartCoroutine(Generate());
    }
    //定义一个负责关闭协程的方法
    private void StopRun()
    {
        StopCoroutine(coroutine);
    }


    /// <summary>
    /// 道具随机生成
    /// </summary>
    /// <param name="boxNum">道具生成数量</param>
    public void RandomBoxSpawn(int boxNum)
    {
        for (int i = 0; i < boxNum; i++)
        {
            RandomSingleBoxSpawn();
            //Debug.Log(guessBoxPrefab.transform.position);
        }
    }

    //创建生成物体的方法  
    private void RandomSingleBoxSpawn()
    {
        //Debug.Log("自动生成道具箱");
        // 获取随机位置x,z
        float x, z;
        int id; //物品的id号,在被玩家捡起之后有逻辑判断
        int kind;
        //x = Random.Range(minX, maxX); //-5f和(float)-5效果一样
        //z = Random.Range(minZ, maxZ);
        x = items[createItemIndex].x;
        z = items[createItemIndex].z;
        id = items[createItemIndex].id;
        kind = items[createItemIndex].kind;

        // 显示在场景中
        boxes[createItemIndex] = Instantiate(guessBoxPrefab, new Vector3(x, 24f, z), Quaternion.identity);
        boxes[createItemIndex].GetComponent<ItemBox>().InitInfo(kind, id);

        if (kind == 4)//测试,如果这个是钥匙!
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

    //创建协程，命名Generate，并定义内容
    private IEnumerator Generate()
    {
        while (true)//用while写个死循环让协程一直启动。
        {
            yield return new WaitForEndOfFrame();
            // 保持场景中道具个数最多为20
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
            //Debug.Log("当前道具个数：" + curBoxNum);
            // 
    }

    void Update()
    {
        // 显示时间
        int minute = (int)(Time.timeSinceLevelLoad - timeBegin) / 60;
        int second = (int)(Time.timeSinceLevelLoad - timeBegin) - minute * 60;
        timeScore.text = string.Format("{0:D2}:{1:D2}", minute, second);

        // 还是没有钥匙，立刻生成
        if (totalBoxNum == 50 && !ItemManager.hasKey)
        {
            RandomBoxSpawn(1);
            guessBox.gameObject.GetComponent<ItemBox>().addKey();
            Debug.Log("强制生成钥匙！！！！！！！！！！！！！！！！！");
        }

        if (ItemManager.hasPlayerTakeKey)
        {
            audioSource.clip = gameBGM_2;
            audioSource.Play();
        }
    }

    //private void randomKey()
    //{
    //    //Debug.Log("自动生成钥匙");
    //    float x, z;
    //    x = Random.Range(-20f, 70f); //-5f和(float)-5效果一样
    //    z = Random.Range(-20f, 20f);
    //    Instantiate(keyObject, new Vector3(x, 4f, z), Quaternion.identity);
    //    //Debug.Log(keyObject.transform.position);
    //    //Debug.Log("正常吗");
    //}

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // 重新加载场景
        Time.timeScale = 1; // 开始记录时间
    }

    public void QuitGame()
    {
        Application.Quit(); // 退出游戏
    }

    static public void GameOver(bool win)
    {
        if (win)
        {
            Debug.Log("玩家胜利");
           // 显示游戏胜利UI
            Time.timeScale = 0f; // 时间停止记录
        }
        else
        {
            Debug.Log("玩家失败");
            // 显示游戏失败UI
            Time.timeScale = 0f; // 时间停止记录
        }
    }
}
