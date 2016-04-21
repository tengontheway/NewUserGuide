/**
	管理所有的抖动
	
	目前有局限性,NGUI的根节点和Camera等坐标绑定的无法抖动

	Added by Teng.
	2015-10-01
**/
using UnityEngine;

public class ShakeManager : Singleton<ShakeManager>
{
	// 抖动指定控件
	// @param shake_obj 抖动对象
	// @param time 抖动时间
	// @param strength 抖动的力度(1左右刚好)
	public void Shake(Transform shake_obj, float time, float strength)
	{
		Shake shake = NGUITools.AddMissingComponent<Shake>(shake_obj.gameObject);
		shake.shakeTime = time;
		shake.strength = strength;
		shake.StartShake();
	}
}
