using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 已知仰角和总时间，水平方向是匀速运动，抛物到指定位置的运动
/// </summary>
public class GTest_02 : MonoBehaviour
{
	public float angle = 30;
	public float totalTime = 1.5f;
	float volocityY = 0;
	float volocityX = 0;
	float accumulateTime = 0;

	Vector3 targetPos;
	bool couldFly = false;
	Vector3 dir;
	Vector3 iPos;

	void Start()
	{
	}

	void Update()
	{
		var pos = GetTargetPos();
		if (pos != Vector3.zero)
		{
			couldFly = true;
			accumulateTime = 0;
			targetPos = pos;
			GameObject.CreatePrimitive(PrimitiveType.Cube).transform.position = targetPos;
			iPos = transform.position;

			dir = (targetPos - transform.position).normalized;
			transform.LookAt(targetPos);
			float dis = Vector3.Distance(targetPos, transform.position);
			volocityX = dis / totalTime;
			//根据加速度公式 加速度=(末速度-初速度)/时间
			//地面是平的，垂直方向的初速度=末速度，但方向相反。
			//a = (Vf-Vi)/t = (-Vi-Vi)/t => Vi=a*t/-2
			volocityY = 4.9f * totalTime;
		}

		if (!couldFly)
		{
			return;
		}
		accumulateTime += Time.deltaTime;

		if (accumulateTime < totalTime)
		{
			Vector3 moveX = volocityX * accumulateTime * dir;
			Vector3 move = iPos + moveX;
			float y = volocityY * accumulateTime - 9.8f * 0.5f * Mathf.Pow(accumulateTime, 2);
			move = move + Vector3.up * y;
			transform.position = move;
		}
		else
		{
			couldFly = false;
		}

	}

	Vector3 GetTargetPos()
	{
		//鼠标左键
		if (Input.GetButtonDown("Fire1"))
		{
			Plane plane = new Plane(new Vector3(0f, 1f, 0f), 0f);
			var p = GetPointInPlane(Camera.main, Input.mousePosition, plane);
			return p;
		}

		return Vector3.zero;
	}

	public static Vector3 GetPointInPlane(Ray ray, Plane plane)
	{
		float d = ray.origin.y - (plane.normal * plane.distance).y;
		Vector3 a = ray.direction / ray.direction.y;
		return ray.origin - a * d;
	}

	public static Vector3 GetPointInPlane(Camera cam, Vector3 sPos, Plane plane)
	{
		var ray = cam.ScreenPointToRay(sPos);
		//plane.Raycast是有物体的时候, GetPointInPlane是用的平面相交
		float distance = 0;
		if (plane.Raycast(ray, out distance))
		{
			return ray.GetPoint(distance);
		}
		return Vector3.zero;

		//return GetPointInPlane(ray, plane);
	}
}