//��ѯ�ɼ�
public class MsgGetAchieve : MsgBase
{
    public MsgGetAchieve() { protoName = "MsgGetAchieve"; }
    //����˻�
    public int win = 0;
    public int lost = 0;
}

//������Ϣ,��Ҫ���������ʾ�ǿ����л���,����Unity��JsonUtility�޷���ȷ�Ľ���rooms����
[System.Serializable]
public class RoomInfo
{
    public int id = 0;      //����id
    public int count = 0;   //����
    public int status = 0;	//״̬0-׼���� 1-ս����
}

//���󷿼��б�
public class MsgGetRoomList : MsgBase
{
    public MsgGetRoomList() { protoName = "MsgGetRoomList"; }
    //����˻�
    public RoomInfo[] rooms;
}

//��������
public class MsgCreateRoom : MsgBase
{
    public MsgCreateRoom() { protoName = "MsgCreateRoom"; }
    //����˻�
    public int result = 0;
}

//���뷿��
public class MsgEnterRoom : MsgBase
{
    public MsgEnterRoom() { protoName = "MsgEnterRoom"; }
    //�ͻ��˷�
    public int id = 0;
    //����˻�
    public int result = 0;
}


//�����Ϣ
[System.Serializable]
public class PlayerInfo
{
    public string id = "ChrisZhang";    //�˺�
    public int camp = 0;        //��Ӫ,�����ǵ���Ϸ����Ա�ʾʹ�����ֶ���
    public int win = 0;         //ʤ����
    public int lost = 0;        //ʧ����
    public int isOwner = 0;		//�Ƿ��Ƿ���
}

//��ȡ������Ϣ
public class MsgGetRoomInfo : MsgBase
{
    public MsgGetRoomInfo() { protoName = "MsgGetRoomInfo"; }
    //����˻�
    public int roomId; //2022.5,8����:����ı����Ϣ,Ϊ����ʾ��Text��
    public PlayerInfo[] players;
}

//�뿪����
public class MsgLeaveRoom : MsgBase
{
    public MsgLeaveRoom() { protoName = "MsgLeaveRoom"; }
    //����˻�
    public int result = 0;
}

//��ս
public class MsgStartBattle : MsgBase
{
    public MsgStartBattle() { protoName = "MsgStartBattle"; }
    //����˻�
    public int result = 0;
}