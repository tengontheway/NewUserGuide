/**
	教学派生类模板
**/
using UnityEngine;
using System.Collections;

public class TeachStepTemplate : TeachStep {

	public virtual void OnStart()
	{
	}
	
	// 检测教学单元有没有完成
	public virtual bool OnCheck()
	{
		return isFinished;
	}
	
	public virtual void OnOver()
	{
	}
	
	public virtual void OnUpdate()
	{
	}
	
	// 消息机制:分发给当前执行的教学,当前教学自己判断是否需要触发
	public virtual void OnMsg(TeachEvent teach_event)
	{
		if (TeachEvent.TE_CLICK_SETTING == teach_event) {
			Finished();
		}
	}
}
