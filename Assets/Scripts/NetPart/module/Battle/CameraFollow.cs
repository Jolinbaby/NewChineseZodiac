using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
	//距离矢量
	public Vector3 distance = new Vector3(0, 8, -18);
	//相机
	public Camera camera;
	//偏移值
	public Vector3 offset = new Vector3(0, 5f, 0);
	//相机移动速度
	public float speed = 70f;

	// Use this for initialization
	void Start () {
		//默认为主相机
		camera = Camera.main;
		//相机初始位置
		Vector3 pos = transform.position;
		Vector3 forward = transform.forward;
		Vector3 initPos = pos - 30*forward + Vector3.up*10;
		camera.transform.position = initPos;
	}

	//所有组件update之后发生
	void LateUpdate () {
		//坦克位置
		Vector3 pos = transform.position;
		//坦克方向
		Vector3 forward = transform.forward;
		//相机目标位置
		Vector3 targetPos = pos;
		targetPos = pos + forward*distance.z;
		targetPos.y += distance.y;
		//相机位置
		Vector3 cameraPos = camera.transform.position;
		cameraPos = Vector3.MoveTowards(cameraPos, targetPos,Time.deltaTime*speed);
		camera.transform.position = cameraPos;
		//对准坦克
		Camera.main.transform.LookAt(pos + offset);
	}
}



