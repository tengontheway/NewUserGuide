/**
	定制编辑器功能菜单. CustomEditor已经被占用
	1.查找perfab引用的所有场景,并打印目录
	2.实例化一个perfab(相当于拖拽实例化perfab)
	
	Added by Teng.
**/
using UnityEngine;
using UnityEditor;

public class TestCustomEditor : EditorWindow
{
	[MenuItem("Assets/Find Prefab In All Scenes")]
	private static void OnSearchForReferences()
	{
		//确保鼠标右键选择的是一个Prefab
		if(Selection.gameObjects.Length != 1)
		{
			return;
		}
		
		//遍历所有游戏场景
		foreach(EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
		{
			if(scene.enabled)
			{
				//打开场景
				EditorApplication.OpenScene(scene.path);

				string perfab_path = AssetDatabase.GetAssetPath(Selection.activeGameObject);

				//获取场景中的所有游戏对象
				GameObject []gos = (GameObject[])FindObjectsOfType(typeof(GameObject));
				foreach(GameObject go  in gos)
				{
					//判断GameObject是否为一个Prefab的引用
					if(PrefabUtility.GetPrefabType(go)  == PrefabType.PrefabInstance)
					{
						UnityEngine.Object parentObject = EditorUtility.GetPrefabParent(go); 
						string path = AssetDatabase.GetAssetPath(parentObject);
						//判断GameObject的Prefab是否和右键选择的Prefab是同一路径。
						//if(path == perfab_path)
						if (parentObject == Selection.activeObject)
						{
							//输出场景名，以及Prefab引用的路径
							Debug.Log(scene.path  + "  " + GetGameObjectPath(go));
						}
					}
				}
			}
		}
	}
	public static string GetGameObjectPath(GameObject obj)
	{
		string path = "/" + obj.name;
		while (obj.transform.parent != null)
		{
			obj = obj.transform.parent.gameObject;
			path = "/" + obj.name + path;
		}
		return path;
	}
	
	[MenuItem("Assets/Find Prefab In All Scenes", true)]
	private static bool OnSearchForReferencesValidation()
	{
		// This returns true when the selected object is a Texture2D (the menu item will be disabled otherwise).
    	return PrefabUtility.GetPrefabType(Selection.activeObject)  == PrefabType.Prefab;
	}

	// 实例化一个perfab
	// [MenuItem("Assets/Instantiate In Scene %i")]
	public static void PlacePrefabInScene ()
	{
		if (Selection.activeObject != null) {
			PrefabUtility.InstantiatePrefab (Selection.activeObject);
		}
	}
	
}