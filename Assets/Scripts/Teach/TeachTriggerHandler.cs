/**
	检测教学分组的触发条件
	
	1.教学参数以#分隔开 
	eg. 关卡id#1战胜;2战败;3胜或败;4战前
		101#3 
**/
using UnityEngine;
using System.Collections.Generic;

// 教学的类型
[System.Serializable]
public enum TeachTrigger
{
	// [1, 100]主条件
	MAIN_FIN_TEACH 				= 1,		// 1:完成X教学后，主界面触发(TeachID)
	MAIN_FIRST_FIN_GATE 		= 2,		// 2:首通X关卡后，主界面触发(GateID)
	NEED_UI						= 4,		// 4:需要界面(SceneID)
	
	GATE_STATE 				= 21,		// 21:关卡状态(关卡id#1战胜;2战败;3胜或败;4战前)
	CLICK_UI 				= 22,		// 22:点击操作(界面#目标#索引 0无索引)
	
	// [100, 200]附加条件
	TEACH_STATE_EX 			= 101,		// 101:检测教学状态(教学id#0未完成;1完成)
};

public class TeachTriggerHandler 
{
	// 检测所有未完成教学检测是否满足触发条件
	public static int DetectAllTeachTrigger()
	{
		int fin_teach_id = CustomPrefs.GetFinishedTeachID();
		if (fin_teach_id >= 0) {
			int idx = TeachMgr.Instance.GetNextTeachIdx(fin_teach_id);
			
			List<int> teach_list = TeachCSV.Instance.teachIDList;
			for (int i = idx; i < teach_list.Count; ++i) {
				int id = teach_list[i];
				if (IsTrigger(id)) {
					return id;
				}
			}
		}
		return -1;
	}
	
	// 强引导，完全可以只检测下一个教学的触发条件
	// @return -1表示无触发ID
	public static int DetectNextTeachTrigger()
	{
		int fin_teach_id = CustomPrefs.GetFinishedTeachID();

		int next_idx = -1;
		if (fin_teach_id < 0) {
			next_idx = 0;
		} else {
			next_idx = TeachMgr.Instance.GetNextTeachIdx(fin_teach_id);
		}

		List<int> teach_list = TeachCSV.Instance.teachIDList;
		if (next_idx >= 0 && next_idx < teach_list.Count) {
			int id = teach_list[next_idx];
			if (IsTrigger(id)) {
				return id;
			}
		}
		return -1;
	}
	
	static void ASSERT(bool b)
	{
		System.Diagnostics.Debug.Assert(b);
	}
	
	// 检测指定的教学组是否满足开启条件
	public static bool IsTrigger(int teach_id)
	{
		TeachGroup teach_group = TeachGroupMgr.Instance.GetTeachGroup(teach_id);
		if (teach_group) {
			bool ok = false;
			
			foreach(TriggerObject to in teach_group.triggers) {
				string trig_params = to.triggerParams;
				
				string[] params_list = trig_params.Split('#');
				ok = IsTrigger(to.triggerType, params_list);
				if (!ok)
					return false;
			}
			
			if (ok)
				return true;
		}
		
		return false;
	}
	
	static bool IsTrigger(TeachTrigger trigger_type, string[] trigger_params)
	{
		switch(trigger_type)
		{
			case TeachTrigger.MAIN_FIN_TEACH:
				return OnTTMainFinTeach(trigger_type, trigger_params);
			case TeachTrigger.MAIN_FIRST_FIN_GATE:
				return OnTTMainFirstFinGate(trigger_type, trigger_params);
			case TeachTrigger.NEED_UI:
				return OnTTNeedUI(trigger_type, trigger_params);	
			case TeachTrigger.GATE_STATE:
				return OnTTGateState(trigger_type, trigger_params);
			case TeachTrigger.CLICK_UI:
				 //什么也不用做,按钮点击特殊处理,为每一个按钮自动添加一个脚本拦截事件
				return true;
			case TeachTrigger.TEACH_STATE_EX:
				return OnTTTeachStateEx(trigger_type, trigger_params);
			default:
			{
				Debug.LogError("Invalid teach trigger type:" + trigger_type);
			}
			break;
		}
		
		return false;
	}
	
	 // 完成X教学后，主界面触发(TeachID)
	 // @param teach_id
	static bool OnTTMainFinTeach(TeachTrigger trigger_type, string[] trigger_params)
	{
		ASSERT(trigger_params.Length == 1);
		
		int teach_id = int.Parse(trigger_params[0]);
		
		// TODO:主界面
		bool is_main_scene = true;
		if (!is_main_scene)
			return false;
		
		return TeachMgr.Instance.IsTeachFinished(teach_id);
	}
	
	// 首通X关卡后，主界面触发
	static bool OnTTMainFirstFinGate(TeachTrigger trigger_type, string[] trigger_params)
	{
		// TODO:首次通关
		Debug.LogError("TODO:TeachTriggerType:" + trigger_type);
		return false;
	}
	
	// 关卡状态(关卡id#1战胜;2战败;3胜或败;4战前)
	static bool OnTTGateState(TeachTrigger trigger_type, string[] trigger_params)
	{
		ASSERT(trigger_params.Length == 2);	
		
		int gate_id = int.Parse(trigger_params[0]);
		int state = int.Parse(trigger_params[1]);
		
		 // TODO:战场的状态
		switch(state)
		{
			case 1:
//				return SandBox.Instance.isGateFinished;
			break;
			case 2:
			break;
			case 3:
			break;
			case 4:
			break;
		}
		
		return false;
	}
	
	// 检测教学状态
	// @param 教学id#0未完成;1完成
	static bool OnTTTeachStateEx(TeachTrigger trigger_type, string[] trigger_params)
	{
		ASSERT(trigger_params.Length == 2);
		
		int teach_id = int.Parse(trigger_params[0]);
		int state = int.Parse(trigger_params[1]);
		
		bool is_finished = TeachMgr.Instance.IsTeachFinished(teach_id);
		if (state == 1 && is_finished) {
			return true;
		}
		
		return false;
	}
	
	// 需要指定的UI
	static bool OnTTNeedUI(TeachTrigger trigger_type, string[] trigger_params)
	{
		ASSERT(trigger_params.Length == 1);
		
		int need_scene_id = int.Parse(trigger_params[0]);
//		if (SceneManager.Instance.curSceneID == (SCENE)need_scene_id) {
//			return true;
//		}
		
		return false;
	}
	
	
}
