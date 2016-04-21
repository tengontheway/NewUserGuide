/**
	仅拦截NGUI点击事件 不像UIButton脚本变幻颜色
	
	Added by Teng.
**/
using UnityEngine;
using System.Collections.Generic;

public class UIClickHook : MonoBehaviour {
	/// <summary>
	/// Click event listener.
	/// </summary>

	public List<EventDelegate> onClick = new List<EventDelegate>();
	
	public List<EventDelegate> onPressed = new List<EventDelegate>();
	
	public List<EventDelegate> onPressedCancel = new List<EventDelegate>();
	
	/// <summary>
	/// Call the listener function.
	/// </summary>

	protected virtual void OnClick ()
	{
		EventDelegate.Execute(onClick);
	}
	
	protected virtual void OnPress(bool pressed)
	{
		if (pressed) {
			EventDelegate.Execute(onPressed);
		} else {
			EventDelegate.Execute(onPressedCancel);
		}
	}
	
}
