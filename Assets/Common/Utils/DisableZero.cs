using UnityEngine;
using System.Collections;

public class DisableZero : MonoBehaviour {
	private bool Loaded = false;

	public enum LoadMethod
	{	
		OnAwake,
		OnStart,
		OnUpdate
	}

	public LoadMethod loadMethod = LoadMethod.OnUpdate;
	public bool AutoDisableSelf = true;
	public bool ResetPositionZero = true;
	public bool DisableGameObj = true;

	void Reset()
	{
		if (Loaded == false) 
		{
			if (DisableGameObj) gameObject.SetActive(false);
			if (ResetPositionZero) gameObject.transform.localPosition = Vector3.zero;

			if (AutoDisableSelf)
			{
				this.enabled = false;
			}

			Loaded = true;
		}
	}

	void Awake()
	{
		if (loadMethod == LoadMethod.OnAwake)
		{
			Reset();
		}
	}
	
	// Use this for initialization
	void Start () {
		if (loadMethod == LoadMethod.OnStart)
		{
			Reset();
		}
	}

	void Update() 
	{
		if (loadMethod == LoadMethod.OnUpdate)
		{
			Reset();
		}
	}

}
