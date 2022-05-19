//同步动物信息
public class MsgSyncAnimal : MsgBase
{
    public MsgSyncAnimal() { protoName = "MsgSyncAnimal"; }
    //位置、旋转、炮塔旋转
    public float x = 0f;
    public float y = 0f;
    public float z = 0f;
    public float ex = 0f;
    public float ey = 0f;
    public float ez = 0f;
    //public float turretY = 0f;
    //public float gunX = 0f;
    //服务端补充
    public string id = "";		//哪个动物
}
//同步钥匙信息：谁拿到了钥匙
public class MsgKey : MsgBase
{
    public MsgKey() { protoName = "MsgKey"; }
   
    //服务端补充
    public string id = "";		//哪个动物
}
//开火
public class MsgFire : MsgBase
{
    public MsgFire() { protoName = "MsgFire"; }
    //炮弹初始位置、旋转
    public float x = 0f;
    public float y = 0f;
    public float z = 0f;
    public float ex = 0f;
    public float ey = 0f;
    public float ez = 0f;

    public string Fireid = "";
    //服务端补充
    public string id = "";		//哪个动物
}

//击中
public class MsgHit : MsgBase
{
    public MsgHit() { protoName = "MsgHit"; }
    //击中谁
    public string targetId = "";
    //击中点	
    public float x = 0f;
    public float y = 0f;
    public float z = 0f;
    public string Fireid = "";//
    //服务端补充
    public string id = "";      //哪个动物
    public int hp = 0;          //被击中动物血量
    public int damage = 0;		//受到的伤害
}