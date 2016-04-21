/**
	教学管理器,管理所有的教学分组
**/
using UnityEngine;
using System.Collections.Generic;

public class TeachMgr : Singleton<TeachMgr> {
	
	// 正在教学的分组
	[HideInInspector]
	public int teachingID { get; set; }
	
	public Dictionary<int, bool> finishedStates = new Dictionary<int, bool>();

	// 正在教学的子步骤
	public int teachingStepID 
	{ 
		get
		{
			TeachStep step = curTeachGroup.stepList[teachingStepIdx];
			return step.stepID;
		}
	}
	
	public int teachingStepIdx { get; set; }
	
	public bool isTeaching { get; set;}

	public bool isFinishedAll = false;
	
	private bool isFinishedGroup = true;
	
	// 记录的正在教学的ID(静态变量因为重连网问题)
	static int interruptTeachID = -1;
	static int interruptTeachStepID = -1;
	
	static int finishedTeachID = -1;
	
	TeachStep curTeachStep;
	TeachGroup curTeachGroup;
	
	// 教学分步骤
	List<TeachStep> teachStepList = new List<TeachStep>();
	
	public void Init()
	{
		int fin_id = CustomPrefs.GetFinishedTeachID();
		if (finishedStates.Count == 0) {
			List<int> ls = TeachCSV.Instance.teachIDList;
			for (int i = 0; i < ls.Count; ++i) {
				int id = ls[i];

				if (id <= fin_id)
					finishedStates[id] = true;
				else
					finishedStates[id] = false;
			}
		}	
	}

	// Update is called once per frame
	public void Update () {
//		if (!SandBox.Instance.isGameInited)
//			return;

		if (isFinishedAll) {
			return;
		}
			
		if (isFinishedGroup) {
			DetectNewTeach();
		} else {
			if (curTeachStep != null) {
				if (curTeachStep.OnCheck()) {
					NextTeachStep();
				} else {
					curTeachStep.OnUpdate();
				}
			}
		}		
	}
	
	// 检测开启新教学
	public bool DetectNewTeach()
	{
//		if (!SandBox.Instance.isGameInited)
//			return false;
			
		if (isFinishedAll || isTeaching)
			return false;
		
		// 检测教学的开启条件
		int new_teach_id = TeachTriggerHandler.DetectNextTeachTrigger();
		if (new_teach_id > 0) {
			StartTeach(new_teach_id);
			return true;
		}
		return false;
	}
	
	// 开启新教学
	public void StartTeach(int teach_id)
	{
		isTeaching = true;
		isFinishedGroup = false;
		
		teachingID = teach_id;
		teachingStepIdx = 0;
		
		curTeachGroup = TeachGroupMgr.Instance.GetTeachGroup(teachingID);
		curTeachStep = CreateTeachStep(teachingStepID);
		curTeachStep.OnStart();
		
		interruptTeachID = teachingID;
		interruptTeachStepID = teachingStepID;
		
		Debug.LogError("-------------StartTeach:" + teach_id);
	}
	
	// 创建教学分步骤
	TeachStep CreateTeachStep(int step_id)
	{
		interruptTeachStepID = step_id;
		Debuger.Log("Start teach step:" + step_id);
		
		teachStepList = curTeachGroup.stepList;
		return TeachGroupMgr.Instance.GetTeachStep(curTeachGroup, step_id);
	}
	
	public void EndTeach(int teach_id)
	{
	}
	
	// 检测指定教学分组是否已经结束
	public bool IsTeachFinished(int teach_id)
	{
		return finishedStates[teach_id];
	}
	
	void NextTeachStep()
	{
		if (curTeachStep != null) {
			curTeachStep.OnOver();
		}
		
		teachingStepIdx += 1;
		if (teachingStepIdx >= teachStepList.Count) {
			// Teach finished
			isTeaching = false;
			isFinishedGroup = true;
			finishedStates[teachingID] = true;
			
			interruptTeachID = -1;
			interruptTeachStepID = -1;
			finishedTeachID = teachingID;

			// Gate finished
			finishedStates[teachingID] = true;
			CustomPrefs.SetFinishedTeachID(teachingID);

			// TODO:by Teng.和服务器交互
			Debuger.LogError(string.Format("Teach {0} finished!", teachingID));

			if (IsLastTeachIdx(teachingID)) {
				isFinishedAll = true;
			} else {
				DetectNewTeach();
			}
		} else {
			curTeachStep = CreateTeachStep(teachingStepID);
			curTeachStep.OnStart();
		}
	}

	// End of teach idx
	public bool IsLastTeachIdx(int cur_teach_id)
	{
		int idx = GetTeachIdx(cur_teach_id);
		if (idx >= 0 && idx == TeachCSV.Instance.teachIDList.Count-1) {
			return true;
		}
		return false;
	}

	public int GetTeachIdx(int cur_teach_id)
	{
		if (cur_teach_id < 0)
			return -1;

		List<int> teach_list = TeachCSV.Instance.teachIDList;
		for (int i = 0; i < teach_list.Count; ++i) {
			if (teach_list[i] == cur_teach_id) {
				return i;
			}
		}
		return -1;
	}
	
	// 根据当前的教学ID获得下一个教学索引
	public int GetNextTeachIdx(int cur_teach_id) 
	{
		List<int> teach_list = TeachCSV.Instance.teachIDList;

		int idx = GetTeachIdx(cur_teach_id);
		if (idx >= 0 && idx + 1 < teach_list.Count) {
			return idx+1;
		}
		return -1;
	}
	
	// 下一个教学ID
	public int GetNextTeachID(int cur_teach_id)
	{
		int teach_idx = GetNextTeachIdx(cur_teach_id);
		if (teach_idx < 0) {
			return -1;
		}
		
		List<int> teach_list = TeachCSV.Instance.teachIDList;
		if (teach_idx + 1 < teach_list.Count) {
			return teach_list[teach_idx + 1];
		}
		return -1;
	}
}
