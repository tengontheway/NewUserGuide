/**
	教学步骤基类
	
	详细实现请扩展
	Added by Teng.
**/
using UnityEngine;
using System.Collections.Generic;

// 教学分步骤的类型
[SerializeField]
public enum TeachStepType
{
	TST_NONE			= 0,		// 0:无需对话框
	TST_TALK 			= 1,		// 1:纯对话
	TST_CLICK_BTN		= 2,		// 2:手指点击
};

public class TeachStep : MonoBehaviour
{
	public int stepID;
	
	public TeachStepType type = TeachStepType.TST_TALK;
	
	protected bool isStarted = false;
	protected bool isFinished = false;
	public string desc = "Step description!";
	public string talkContentList = "";
	
	// 触发对象,仅【手指点击】模式有效
	public GameObject triggerObject;
	
	public TeachStep()
	{
		#if UNITY_EDITOR
			if (type == TeachStepType.TST_TALK || type == TeachStepType.TST_CLICK_BTN) {
				string[] datas = talkContentList.Split('#');
				if (datas.Length == 0) {
					Debug.LogError(string.Format("Teach step {0} has invalid params 【talkContentList】", stepID));
				}
			}
			
			if (type == TeachStepType.TST_CLICK_BTN) {
				if (triggerObject == null) {
					Debug.LogError(string.Format("Teach step {0} has no trigger object!", stepID));
				}
			}
		#endif
	}
	
	// 开始教学
	public virtual void OnStart()
	{
		ShowStepDlg();
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
	public virtual void OnMsg()
	{
		
	}
	
	public void Finished()
	{
		isFinished = true;
	}
	
	// 显示教学面板
	protected void ShowStepDlg()
	{
		/**
		GameObject go = SceneManager.Instance.GetGameObject(SCENE.COMMON_TEACH_TALK);
		TeachTalkDlg dlg = go.GetComponent<TeachTalkDlg>();
		
		string[] contents = talkContentList.Split('#');
		
		List<int> ls = new List<int>();
		for (int i = 0; i < contents.Length; ++i) {
			int lang_id = int.Parse(contents[i]);
			ls.Add(lang_id);
		}

		// TODO:显示教学面板
		switch(type)
		{
			case TeachStepType.TST_TALK:
				{
					dlg.Init(ls, ()=>
					{
						isFinished = true;
					}, TalkDlgType.TalkDlg_Talk);
					dlg.Show();
				}
			break;
			case TeachStepType.TST_CLICK_BTN:
				{
					dlg.Init(ls, ()=>
					{
						isFinished = true;
					}, TalkDlgType.TalkDlg_ClickBtn, triggerObject);
					dlg.Show();
				}
			break;
			default:
			break;
		}
		**/
	}
	
}
