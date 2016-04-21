/**
	为目标添加事件钩子	
**/
using UnityEngine;
using System;

public class TeachObjectEventHook
{
	protected GameObject	gameObject;
	
	// 只触发一次
	bool autoRelase = false;
	
	// 拦截事件后的回调		
	Action actTrigger;
	
	// 初始化
	// @param obj 要注入钩子的对象
	// @param act_trigger 触发事件的回调
	// @param auto_release true 播放一次后自动释放掉钩子
	//					   false 需要自己手动释放
	public void AutoBind(GameObject obj, Action act_trigger, bool auto_release = true)
	{
		gameObject = obj;
		actTrigger = act_trigger;
		autoRelase = auto_release;
		
		OnBindHook();
	}
	
	// 为对象绑定事件钩子
	public virtual void OnBindHook()
	{
	}
	
	// 为对象释放事件钩子
	public virtual void OnUnbindHook()
	{
	}
	
	// 触发钩子事件
	public void OnTriggerHook()
	{
		if (actTrigger != null) {
			actTrigger();
		}
		
		if (autoRelase) {
			OnUnbindHook();
		}
	}
}
