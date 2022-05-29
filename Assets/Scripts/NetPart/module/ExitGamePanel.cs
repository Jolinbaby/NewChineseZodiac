using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ExitGamePanel: BasePanel
{
    //ȷ����ť
    private Button okBtn;

    private Button CancleBtn;

    //��ʼ��
    public override void OnInit()
    {
        //����Ƥ����ַskinPath�����Ĳ㼶layer
        skinPath = "ExitGamePanel";
        layer = PanelManager.Layer.Panel;
    }

    //��ʾ
    public override void OnShow(params object[] args)
    {
        //Ѱ�����
        okBtn = skin.transform.Find("okBtn").GetComponent<Button>();
        CancleBtn  = skin.transform.Find("CancleBtn").GetComponent<Button>();

        //����:ͨ�����ַ�ʽ�󶨼���,���Ժܺõؽ���Ԥ�������ֲ.
        okBtn.onClick.AddListener(OnOkEscClick);
        CancleBtn.onClick.AddListener(OnCancleClick);

    }

    //�ر�
    public override void OnClose()
    {

    }
    public void OnCancleClick()
    {
        Close();
    }

    public void OnOkEscClick()
    {
        Debug.Log("����˳�-----------------");
        NetManager.Close();
        //        //�˳���Ϸ
        //        //Ԥ����
#if UNITY_EDITOR    //�ڱ༭��ģʽ��
        EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif

    }






}
