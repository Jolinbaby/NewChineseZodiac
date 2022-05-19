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
        Debug.Log("��ʼ����Ϸ");
        
        /* ɾ������ */

        // ת������

    }

    private void QuitGame()
    {
        Debug.Log("�˳���Ϸ");
        Application.Quit();
    }
}
