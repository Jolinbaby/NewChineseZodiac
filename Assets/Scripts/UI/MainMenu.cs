using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    Button newGameBtn;
    Button quitBtn;

    private void Awake()
    {
        newGameBtn = transform.GetChild(1).GetComponent<Button>();
        quitBtn = transform.GetChild(2).GetComponent<Button>();

        newGameBtn.onClick.AddListener(NewGame);
        quitBtn.onClick.AddListener(QuitGame);
    }

    private void NewGame()
    {
        Debug.Log("开始新游戏");
        
        /* 删除缓存 */

        // 转换场景

    }

    private void QuitGame()
    {
        Debug.Log("退出游戏");
        Application.Quit();
    }
}
