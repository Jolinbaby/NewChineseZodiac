//ע��
public class MsgRegister : MsgBase
{
    public MsgRegister() { protoName = "MsgRegister"; }
    //�ͻ��˷�
    public string id = "";
    public string pw = "";
    //����˻أ�0-�ɹ���1-ʧ�ܣ�
    public int result = 0;
}


//��½
public class MsgLogin : MsgBase
{
    public MsgLogin() { protoName = "MsgLogin"; }
    //�ͻ��˷�
    public string id = "";
    public string pw = "";
    //����˻أ�0-�ɹ���1-ʧ�ܣ�
    public int result = 0;
}


//�����ߣ���������ͣ�
public class MsgKick : MsgBase
{
    public MsgKick() { protoName = "MsgKick"; }
    //ԭ��0-�����˵�½ͬһ�˺ţ�
    public int reason = 0;
}