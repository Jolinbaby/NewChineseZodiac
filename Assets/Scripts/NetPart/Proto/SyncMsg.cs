//ͬ��������Ϣ
public class MsgSyncAnimal : MsgBase
{
    public MsgSyncAnimal() { protoName = "MsgSyncAnimal"; }
    //λ�á���ת��������ת
    public float x = 0f;
    public float y = 0f;
    public float z = 0f;
    public float ex = 0f;
    public float ey = 0f;
    public float ez = 0f;
    //public float turretY = 0f;
    //public float gunX = 0f;
    //����˲���
    public string id = "";		//�ĸ�����
}
//ͬ��Կ����Ϣ��˭�õ���Կ��
public class MsgKey : MsgBase
{
    public MsgKey() { protoName = "MsgKey"; }
   
    //����˲���
    public string id = "";		//�ĸ�����
}
//����
public class MsgFire : MsgBase
{
    public MsgFire() { protoName = "MsgFire"; }
    //�ڵ���ʼλ�á���ת
    public float x = 0f;
    public float y = 0f;
    public float z = 0f;
    public float ex = 0f;
    public float ey = 0f;
    public float ez = 0f;

    public string Fireid = "";
    //����˲���
    public string id = "";		//�ĸ�����
}

//����
public class MsgHit : MsgBase
{
    public MsgHit() { protoName = "MsgHit"; }
    //����˭
    public string targetId = "";
    //���е�	
    public float x = 0f;
    public float y = 0f;
    public float z = 0f;
    public string Fireid = "";//
    //����˲���
    public string id = "";      //�ĸ�����
    public int hp = 0;          //�����ж���Ѫ��
    public int damage = 0;		//�ܵ����˺�
}

public class MsgPickup : MsgBase
{
    public MsgPickup() { protoName = "MsgPickup"; }
    public int itemid; //�ƺ�ֻҪ������Ʒ��id�Ϳ���
}