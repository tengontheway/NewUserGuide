/**
	管理所有的教学分组
	
	约定:每个TeachGroup最后的数字就是教学表中的ID eg.TeachGroup1000
	
	Added by Teng.
**/
using UnityEngine;
using System.Collections.Generic;

public class TeachGroupMgr : MonoBehaviour {
	
	public Dictionary<int, TeachGroup> teachGroupList = new Dictionary<int, TeachGroup>();
	
	const string TEACH_GROUP_PREFIX = "TeachGroup";
	
	static TeachGroupMgr _instance;
	static public TeachGroupMgr Instance
	{
		get{
			if (_instance == null) {
				_instance = GameObject.FindObjectOfType<TeachGroupMgr>();
			}
			return _instance;
		}
	}
	
	void Awake()
	{
		_instance = this;
	}
	
	// Use this for initialization
	void Start () {
		InitTeachGroups();
	}
	
	void InitTeachGroups()
	{
		Transform ts = transform;
		for (int i = 0; i < ts.childCount; ++i) {
			Transform child = ts.GetChild(i);
			
			TeachGroup tg = child.GetComponent<TeachGroup>();
			if (!tg) {
				Debug.LogError("Cannot find component TeachGroup in Control:" + child.name);
			}
			
			string name = tg.name;
			if (!name.StartsWith(TEACH_GROUP_PREFIX)) {
				Debug.LogError(string.Format("TeachGroup name {0} format is invalid!", name));
			}
			name = name.Substring(TEACH_GROUP_PREFIX.Length);
			
			int teach_id = int.Parse(name);
			if (!teachGroupList.ContainsKey(teach_id)) {
				teachGroupList.Add(teach_id, tg);
			} else {
				Debug.LogError(string.Format("TeachGroup {0} already exist!", name));
			}
		}
	}
	
	public TeachGroup GetTeachGroup(int teach_id)
	{
		TeachGroup tg;
		if (teachGroupList.TryGetValue(teach_id, out tg)) {
			return tg;
		} 
		
		return null;
	}
	
	public TeachStep GetTeachStep(int teach_id, int teach_step_id) {
		TeachGroup tg = GetTeachGroup(teach_id);
		return GetTeachStep(tg, teach_step_id);
	}
	
	public TeachStep GetTeachStep(TeachGroup tg, int teach_step_id) {
		if (tg == null)
			return null;
		return tg.GetTeachStep(teach_step_id);	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
