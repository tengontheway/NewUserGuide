/**
	对TweenTransformEx.cs类的属性的编辑器定制，实现了功能自动化处理，节省开发时间
	
	Added by Teng. 2015.7.26
**/
using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof (TweenTransformEx))]
public class TweenTransformHelperEditor : Editor {
	
	private TweenTransformEx _tweener;
	
	private string name;
	
	private void Awake() {
		_tweener = (TweenTransformEx) target;
		
		name = _tweener.gameObject.name;
	}
	
	public override void OnInspectorGUI() {
		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("Create from anchor")) {
			CreateAnchorFrom();
		}
		if (GUILayout.Button("Destroy")) {
			DestroyAnchor(_tweener.FromAnchor);
		}
		EditorGUILayout.EndHorizontal();
		_tweener.FromAnchor = (GameObject) EditorGUILayout.ObjectField(_tweener.FromAnchor, typeof (GameObject));
		
		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button("Create to anchor")) {
			CreateAnchorTo();
		}
		if (GUILayout.Button("Destroy")) {
			DestroyAnchor(_tweener.ToAnchor);
		}
		EditorGUILayout.EndHorizontal();
		_tweener.ToAnchor = (GameObject) EditorGUILayout.ObjectField(_tweener.ToAnchor, typeof (GameObject));
		
		CreateAndApplyTweener();
		UpdateUI();
	}
	
	private void CreateAndApplyTweener() {
		bool toAnchorNotEqualsNull = _tweener.ToAnchor;
		bool fromAnchorNotEqualsNull = _tweener.FromAnchor;
		
		if (!fromAnchorNotEqualsNull) {
			EditorGUILayout.HelpBox("From anchor not created!", MessageType.Warning);
		}
		else if (!toAnchorNotEqualsNull) {
			EditorGUILayout.HelpBox("To anchor not created!", MessageType.Warning);
		}
		else {
			if (GUILayout.Button("Apply to tween")) {
				var tweenComponent = _tweener.GetComponent<TweenTransform>() ?? _tweener.gameObject.AddComponent<TweenTransform>();
				tweenComponent.from = _tweener.FromAnchor.transform;
				tweenComponent.to = _tweener.ToAnchor.transform;
				tweenComponent.enabled = false;
			}
		}
	}
	
	private void UpdateUI() {
		if (GUI.changed) {
			EditorUtility.SetDirty(_tweener);
		}
	}
	
	private void DestroyAnchor(GameObject gameObj) {
		if (gameObj == null) {
			return;
		}
		DestroyImmediate(gameObj);
	}
	
	private void CreateAnchorTo() {
		var anchor = CreateAnchor("$anchorTo_" + name);
		_tweener.ToAnchor = anchor;
	}
	
	private void CreateAnchorFrom() {
		var anchor = CreateAnchor("$anchorFrom_" + name);
		_tweener.FromAnchor = anchor;
	}
	
	private GameObject CreateAnchor(string anchorName) {
		var anchorGameObj = new GameObject(anchorName);
		anchorGameObj.transform.parent = _tweener.transform.parent;
		anchorGameObj.transform.localPosition = Vector3.zero;
		anchorGameObj.transform.localScale = Vector3.one;

		var widgetScript = anchorGameObj.AddComponent<UIWidget>();
		widgetScript.width = widgetScript.height = 100;
		return anchorGameObj;
	}
}