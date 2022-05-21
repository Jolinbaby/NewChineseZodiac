using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EscPanel : BasePanel
{
    //ȷ����ť
    private Button EscBtn;
    
    //��ʼ��
    public override void OnInit()
    {
        //����Ƥ����ַskinPath�����Ĳ㼶layer
        skinPath = "EscPanel";
        layer = PanelManager.Layer.Panel;
    }

    //��ʾ
    public override void OnShow(params object[] args)
    {
        //Ѱ�����
        EscBtn = skin.transform.Find("ESC_ExitGame").GetComponent<Button>();

        //����:ͨ�����ַ�ʽ�󶨼���,���Ժܺõؽ���Ԥ�������ֲ.
        EscBtn.onClick.AddListener(OnEscClick);

        
    }

    //�ر�
    public override void OnClose()
    {
        
    }
    //������ע�ᰴť
    public void OnEscClick()
    {
        PanelManager.Open<ExitGamePanel>();
        
        //Close();
    }

   
}
