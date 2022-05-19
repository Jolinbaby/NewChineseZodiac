
//坦克信息
[System.Serializable]
public class AnimalInfo{
	public string id = "";	//玩家id
	public int camp = 0;	//阵营
	public int hp = 0;		//生命值

	public float x = 0;		//位置
	public float y = 0;
	public float z = 0;
	public float ex = 0;	//旋转
	public float ey = 0;
	public float ez = 0;
}

public class MsgAnimation : MsgBase
{
    public MsgAnimation() { protoName = "MsgAnimation"; }
    public int isJump=0; //是否跳跃
    public float speed = 0f; //当前速度
    //服务端补充:哪个角色放的动画
    public string id = "";

}

//进入战场（服务端推送）
public class MsgEnterBattle:MsgBase {
	public MsgEnterBattle() {protoName = "MsgEnterBattle";}
	//服务端回
	public AnimalInfo[] animals;
	public int mapId = 1;	//地图，只有一张
}

//战斗结果（服务端推送）
public class MsgBattleResult:MsgBase {
	public MsgBattleResult() {protoName = "MsgBattleResult";}
	//服务端回
	//public int winCamp = 0;	 //获胜的阵营
	public string winId = "0";
}

//玩家退出（服务端推送）
public class MsgLeaveBattle:MsgBase {
	public MsgLeaveBattle() {protoName = "MsgLeaveBattle";}
	//服务端回
	public string id = "";	//玩家id
}