/* 
    管理弹窗 用来按照列表顺序弹出窗体
    added by mxdy 【2015/08/31】
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PopDlgManager : MonoBehaviour {
    private static PopDlgManager myDlg;
    public static PopDlgManager Instance
    {
        get 
        {
            if (!myDlg)
            {
                myDlg = GameObject.FindObjectOfType(typeof(PopDlgManager)) as PopDlgManager;
            }
            return myDlg;
        }
    }

    // 弹窗等待列表
    List<GameObject> dlgList = new List<GameObject>();

	// Use this for initialization
	void Start () {
	
	}

    // 添加到等待列表中
    public void AddIntoDlgList(GameObject go)
    {
        dlgList.Add(go);
    }

    // 从第一个移出列表
    public void removeItem()
    {
        dlgList.RemoveAt(0);
        Debug.Log("弹出窗当前剩余:" + dlgList.Count);
    }
	
	// Update is called once per frame
	void Update () {
        if (dlgList.Count > 0)
        {
            for (int i = 0; i < dlgList.Count; i++)
            {
                if (i == 0)
                {
                    dlgList[i].SetActive(true);
                }
                else
                {
                    dlgList[i].SetActive(false);
                }
            }
        }
	}
}
