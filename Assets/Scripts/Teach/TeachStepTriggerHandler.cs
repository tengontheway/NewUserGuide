/**
	检测教学分步骤的触发条件
**/
using UnityEngine;
using System.Collections.Generic;


public class TeachStepTriggerHandler 
{
	// 检测指定的教学组是否满足开启条件
	public static bool IsTrigger(int step_id)
	{
//		Dictionary<int, Dictionary<int, string[]>> id_triggers = TeachCSV.Instance.IDTriggerDicts;
//		
//		Dictionary<int, string[]> trigger_list = new Dictionary<int, string[]>();
//		if (id_triggers.TryGetValue(step_id, out trigger_list)) {
//			foreach(KeyValuePair<int, string[]> kv in trigger_list) {
//				bool b = IsTrigger((TeachStepType)kv.Key, kv.Value);
//				if (b == false)
//					return false;
//			}
//			
//			return true;
//		}
		
		return false;
	}
	
	static void ASSERT(bool b)
	{
		System.Diagnostics.Debug.Assert(b);
	}
	
	static bool IsTrigger(TeachStepType trigger_type, string[] trigger_params)
	{
//		switch(trigger_type)
//		{
//			case TeachStepTrigger.TT_CLICK_UI:
//				// 什么也不用做,按钮点击特殊处理,为每一个按钮自动添加一个脚本拦截事件
//				return true;
//			default:
//			{
//				Debug.LogError("Invalid teach trigger type:" + trigger_type);
//			}
//			break;
//		}
		
		return false;
	}
	
	
}
