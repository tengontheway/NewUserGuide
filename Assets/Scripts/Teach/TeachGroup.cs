/**
	教学分组:管理所有教学步骤
**/
using UnityEngine;
using System;
using System.Collections.Generic;

/**
	教学的触发对象
**/
[System.Serializable]
public class TriggerObject
{
	// 教学触发条件
	public 	TeachTrigger triggerType = TeachTrigger.NEED_UI;
	
	// 教学触发的参数(详见TeachTriggerHandler表)
	public string triggerParams;
}


public class TeachGroup : MonoBehaviour {
	
	[NonSerialized]
	public List<TeachStep> stepList = new List<TeachStep>();
	
	// 触发列表，只有所有触发条件都满足,才触发教学
	public List<TriggerObject> triggers = new List<TriggerObject>();
		
	// Use this for initialization
	void Start () {
		InitSteps();
	}
	
	void InitSteps()
	{
		stepList.Clear();
		
		int i = 0;
		while(true)
		{
			Transform step_trans = transform.FindChild("" + i);
			if (step_trans) {
				TeachStep step = step_trans.GetComponent<TeachStep>();
				if (!step) {
					Debug.LogError(string.Format("Cannnot find TeachStep componet in teachgroup:{0} idx:{1}",
						gameObject.name, i));
				}
				stepList.Add(step);
			} else {
				break;
			}
			++i;
		}
		
		if (stepList.Count == 0 || stepList.Count != transform.childCount) {
			Debug.LogError(string.Format("TeachStepCount {0} != TeachGroup:{1} child count!", stepList.Count, gameObject.name));
		}
	}
	
	public TeachStep GetTeachStep(int teach_step_id) {
		if (teach_step_id >= 0 && teach_step_id < stepList.Count) {
			return stepList[teach_step_id];
		}
		return null;
	}
}
