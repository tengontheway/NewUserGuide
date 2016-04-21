/**
 	下拉列表来切换语言,UI中的label、Sprite同步切换
	 
	 Added by Teng.
 **/
using UnityEngine;

/// <summary>
/// Turns the popup list it's attached to into a language selection list.
/// </summary>
/**
[RequireComponent(typeof(UIPopupList))]
[AddComponentMenu("NGUI/Interaction/Custom Language Selection")]
public class LanguageSelectionEx : MonoBehaviour
{
	UIPopupList mList;

	void Awake ()
	{
		mList = GetComponent<UIPopupList>();
		Refresh();
	}

	void Start () { 
		EventDelegate.Add(mList.onChange, delegate() {Lang.Instance.Language = UIPopupList.current.value; }); 
	}

	/// <summary>
	/// Immediately refresh the list of known languages.
	/// </summary>
	public void Refresh ()
	{
		if (mList != null && Lang.Instance.SupportLangs != null)
		{
			mList.Clear();

			for (int i = 0, imax = Lang.Instance.SupportLangs.Length; i < imax; ++i)
				mList.items.Add(Lang.Instance.SupportLangs[i]);

			mList.value = Lang.Instance.Language;
		}
	}
}

**/