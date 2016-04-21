/**
	管理所有事件钩子的管理器(也是工厂)
**/
using UnityEngine;
using System;

public class EventHookMgr : Singleton<EventHookMgr> 
{
	// 工厂函数,根据目标的类型绑定对应的事件钩子
	// 是按钮绑定点击钩子,是ScrollView绑定ScrollView钩子
	public bool AutoBindHook(GameObject target, Action actTrigger)
	{
		TeachObjectEventHook hook = null;
		
		if (target.GetComponent<UIButton>()) {
			hook = new UIButtonEventHook();
		} else {
			Debug.LogError("Unknown bind hook type:" + target.name);
		}
		
		if (hook == null)
			return false;
		
		hook.AutoBind(target, actTrigger, true);
		return true;
	}
}
