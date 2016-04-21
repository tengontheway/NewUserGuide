/**
	用来拦截按钮事件
**/
using UnityEngine;
using System.Collections;

public class UIButtonEventHook : TeachObjectEventHook {
	
	UIButton button;
	
	EventDelegate clickDelegate;
	
	public override void OnBindHook()
	{
		button = gameObject.GetComponent<UIButton>();
		if (button != null) {
			OnUnbindHook();
			
			clickDelegate = new EventDelegate(OnTriggerHook);
			button.onClick.Add(clickDelegate);
		}
	}
	
	public override void OnUnbindHook()
	{
		if (clickDelegate != null) {
			button.onClick.Remove(clickDelegate);
		}
	}
}
