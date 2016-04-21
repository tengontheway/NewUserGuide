/**
	抖屏效果
	
	局限:NGUI的根节点和Camera等坐标绑定的无法抖动 也可以参考DOTween的Shake
	
	Added by Teng.
	2015-10-01
**/
using UnityEngine;

public class Shake : MonoBehaviour {
	
	public float strength = 1f;           // 抖动力度
	
	public float shakeTime = 0.5f;       // 抖动时间
	
	private float restTime;				 // 剩余时间
	
	private bool shakeSwitch = false;    // 抖动开关
	
	private Vector3 originPosition;      // 保存对象原位置
	
	//  void OnGUI (){
	//  	if (GUI.Button (new Rect (20, 40,80,20), "Shake")){
	//  		StartShake();
	//  	}
	//  }
	
	// Use this for initialization
	void Start () {
		restTime = shakeTime;
		
		// 保存对象原位置
		originPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		//抖动开关开否
		if (shakeSwitch)
		{
			float force = NowShakeForce();
			
			//抖动当前对象
			transform.position = originPosition + Random.insideUnitSphere * force;
			
			//抖动结束，退出抖动
			restTime = restTime - Time.deltaTime;
			if (restTime <= 0)
			{
				transform.position = originPosition;
				shakeSwitch = false;
			}
		}
	}
	
	// 抖动开启
	public void StartShake(float _strength)
	{
		restTime = shakeTime;
		strength = _strength;
		
		shakeSwitch = true;
	}
	
	// 抖动开启
	public void StartShake()
	{
		restTime = shakeTime;
		shakeSwitch = true;
	}
	
	// 当前的抖动的力
	float NowShakeForce()
	{
		return strength * restTime / shakeTime * 0.05f;
	}
}
