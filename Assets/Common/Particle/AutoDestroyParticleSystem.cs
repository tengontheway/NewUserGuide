/**
	自动销毁粒子，粒子必须是非loop的,否则脚本无效
	Add this class to a ParticleSystem so it auto destroys once it has stopped emitting.
	Make sure your ParticleSystem isn't looping, otherwise this script will be useless
	
	Added by Teng.
**/
using UnityEngine;
using System;
using System.Collections;

public class AutoDestroyParticleSystem : MonoBehaviour 
{
	// 粒子子对象列表
	ParticleSystem[] particleSystems;

	[HideInInspector]
	public Action finishCallback;
	
	void LateUpdate()
	{
		if (particleSystems == null)
			particleSystems = transform.GetComponentsInChildren<ParticleSystem>();

		bool destoryStatus = true;
		foreach(ParticleSystem particleObject in particleSystems)
		{
			if (particleObject == null || particleObject.IsAlive(false)) {
				destoryStatus = false;
				break;
			}
		}

		if(destoryStatus) {
			if (finishCallback != null) {
				finishCallback();
			}

			Destroy(this.gameObject);
		}
	}


	public void SetFinishedCallback(Action cb)
	{
		finishCallback = cb;
	}
}