/**
	滚动数值 按照一定规律滚动数字，滚动由快变慢
	1.有数字滚动的起始和终止回调
	2.默认在脚本中自动为label更新数值,也可以在外部通过getNumber更新文本
	  有特殊情况,将label分成不同部分带颜色显示,eg.基数+[00ff00]奖励[-],需要在程序中定制
	3.可以通过setMiddleSpeed定制数值增加速度
	4.游戏中可能多个Label同时滚动，静态变量统一计数播放声音
	
	eg.
	ShiftyNumber shifty = label.GetComponent<ShiftyNumber>();
	
	// 开始事件
	shifty.setStartCallback(GameObject =>
	{
		// Play shifty sound...
	});
	
	// 结束事件
	shifty.setEndCallback(GameObject =>
	{
		// Next start shifty...
	});
	
	// 定制显示文本
	shifty.setUpdateLabelCallback((shifty_obj, shifty_lab, shifty_number) =>
    {
       int number = shifty_number.getNumber();
	   if (number < 0)
	   		shifty_lab.text = string.Format("[ff0000]{0}[-]", number);
	   else
	   		shifty_lab.text = string.Format("[00ff00]{0}[-]", number);
    });
**/
using UnityEngine;
using System;
using System.Collections;

public class ShiftyNumber : MonoBehaviour {
	// 当前数值
	public int number = 0;
	// 目标数值
	public int targetNumber = 0;
	
	// 数值<=10 的滚动速度
	public uint middleSpeed = 1;

	// 自动更新标签(有特殊情况,将label分成不同部分带颜色显示,eg.基数+[00ff00]奖励[-])
	public bool autoUpdateLabel = true;
	
	// 起始回调(播放声音等)
	Action<object> startShiftyCallback;
	// 滚动结束回调
	Action<object> endedShiftyCallback;
	// 更新文本回调
	Action<object, UILabel, ShiftyNumber> updateLabelCallback;
	
	// 是否正在滚动中
	private bool shiftying = false;

	// 文本标签
	private UILabel label;
	
	static int tmpStep = 0;
	
	// Use this for initialization
	void Start () {
		setTargetNumber(targetNumber);
		
		label = GetComponent<UILabel>();
	}
	
	// Update is called once per frame
	void Update () {
		// 滚动由快变慢
		int addScore = targetNumber - number;
		if (addScore != 0) {
			shiftying = true;
			
			int abs = Math.Abs(addScore);
			if (abs > 10) {
				number += (int)(addScore / 10);
			} else if (abs > middleSpeed && abs <= 10) {
				number += (int)(addScore / abs) * (int)middleSpeed;
			} else if (abs > 2 && abs <= middleSpeed) {
				number += (int)(addScore / abs);
			} else {
				number = targetNumber;
			}
			
			if (tmpStep++ % 8 == 0)
			{
//				SoundManager.Instance.PlaySound(SoundPool.instance.OtherSfx[0], Vector3.zero);
			}
			
			if (autoUpdateLabel) {
				if (label)
					label.text = "" + number;
			}
			
			// 定制显示内容,eg.颜色
			if (updateLabelCallback != null) {
				updateLabelCallback(gameObject, label, this);
			}
		} else {
			if (shiftying) {
				if (endedShiftyCallback != null) {
					endedShiftyCallback(gameObject);
				}
				
				if (autoUpdateLabel) {
					if (label)
						label.text = "" + number;
				}
				
				// 定制显示内容,eg.颜色
				if (updateLabelCallback != null) {
					updateLabelCallback(gameObject, label, this);
				}
				
				shiftying = false;
			}
		}
	}
	
	// 设置 数值<=10 的滚动速度
	void setMiddleSpeed(uint speed = 1)
	{
		middleSpeed = speed;
		if (middleSpeed >= 10) {
			middleSpeed = 9;
		}
	}
	
	// 是否正在滚动中
	public bool isShiftying()
	{
		return shiftying;
	}
	
	// 获得当前值
	public int getNumber()
	{
		return number;
	}
	
	// 获得目标值
	public int getTargetNumber()
	{
		return targetNumber;
	}
	
	// 获得目标值
	public void setTargetNumber(int num)
	{
		int backup = targetNumber;
		targetNumber = num;
		
		shiftying = true;
		
		if (backup != num) {
			if (startShiftyCallback != null)
				startShiftyCallback(gameObject);
		}
	}
	
	// 滚动开始回调
	public void setStartCallback(Action<object> start_cb)
	{
		startShiftyCallback = start_cb;
	}
	
	// 滚动结束回调
	public void setEndCallback(Action<object> end_cb)
	{
		endedShiftyCallback = end_cb;
	}
	
	// 定制显示内容,eg.颜色
	public void setUpdateLabelCallback(Action<object, UILabel, ShiftyNumber> update_cb)
	{
		updateLabelCallback = update_cb;
	}
}

