using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
	// 门板物体（从 Inspector 拖入）
	public Transform door;

	// 门打开后的目标角度（绕 Y 轴旋转 90 度）
	public float openAngle = -90f;

	// 开门速度
	public float openSpeed = 2f;

	private bool isOpen = false;
	private Quaternion closedRotation;
	private Quaternion openRotation;

	void Start()
	{
		// 记录门关闭时的初始角度
		closedRotation = door.rotation;
		openRotation = Quaternion.Euler(door.eulerAngles + new Vector3(0, openAngle, 0));
	}

	void Update()
	{
		// 如果需要开门，平滑旋转到目标角度
		if (isOpen)
		{
			door.rotation = Quaternion.Lerp(door.rotation, openRotation, Time.deltaTime * openSpeed);
		}
	}

	// 玩家进入触发区域时调用
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			isOpen = true;
		}
	}
}
