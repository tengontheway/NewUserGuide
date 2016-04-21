﻿/**
	对应类的语法糖扩展
	
	Added by Teng.
**/
using UnityEngine;
using System.Collections;  

public static class Extensions  
{  
	#region Transform
	public static void SetPositionX(this Transform t, float newX)  
	{  
		t.position = new Vector3(newX, t.position.y, t.position.z);  
	}
	
	public static void SetPositionY(this Transform t, float newY)  
	{  
		t.position = new Vector3(t.position.x, newY, t.position.z);  
	}  
	
	public static void SetPositionZ(this Transform t, float newZ)  
	{  
		t.position = new Vector3(t.position.x, t.position.y, newZ);  
	}  
	
	public static float GetPositionX(this Transform t)  
	{  
		return t.position.x;  
	}  
	
	public static float GetPositionY(this Transform t)  
	{  
		return t.position.y;  
	}  
	
	public static float GetPositionZ(this Transform t)  
	{  
		return t.position.z;  
	}  
	#endregion
	
	#region GameObject
	public static bool HasRigidbody(this GameObject gobj)  
	{  
		return (gobj.GetComponent<Rigidbody>() != null);  
	}
	
	public static bool HasAnimation(this GameObject gobj)  
	{  
		return (gobj.GetComponent<Animation>() != null);  
	} 
	#endregion
	
	public static void SetSpeed(this Animation anim, float newSpeed)  
	{  
		anim[anim.clip.name].speed = newSpeed;   
	}  
}
