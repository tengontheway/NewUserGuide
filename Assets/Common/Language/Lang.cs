/**
	多语言配置表
	
	1.支持通过ID和宏来获得内容
	2.支持csv表中设定一个宏区间，程序中可以根据区间来随机
	
	int start = Lang.Instance.getIDByMacro("IM_LOADING_TIPS_BEGIN");
    int end = Lang.Instance.getIDByMacro("IM_LOADING_TIPS_END");
    
    string content = Lang.Instance.getLangByID(Random.Range(start, end));
	
	3.切换语言时,广播推送。UI中的Label、Sprite同步切换
	UIRoot.Broadcast("OnLocalize");
	
	4.场景中需要语言切换的对象(label、Sprite...)需要挂载UILocalizeEx.cs
	
	5.语言名 en、zh、zh-Hans、zh-Hant...
	
	6.游戏语言优先级: 上次记录的语言 > 系统语言 > 默认语言
	
	Added by Teng.
**/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
public class Lang : Singleton<Lang> 
{
	// key:string:macro(key) val:int:id 宏和id对应的列表
	public Dictionary<string, int>  macroIdList = new Dictionary<string, int>();
	
	// id对应语言
	public Dictionary<int, string[]> idContentList = new Dictionary<int, string[]>();
	
	// 默认语言
	private const string PLAYERPREF_LANG_NAME = "Language";
	
	// 支持的多语言列表 "en"、"zh"...
	private string[] supportLangs;

	// Currently selected language
	private string curLang;
	
	// 当前语言索引
	private int curLangIdx = -1;
	
	private bool isInit = false;
	
	// key:string:langName(en、zh...), val:SystemLanguage:type 多语言查询列表
	private Dictionary<string, SystemLanguage> allLangNameTypes = new Dictionary<string, SystemLanguage>();
	private Dictionary<SystemLanguage, string> allLangTypeNames = new Dictionary<SystemLanguage, string>();
	
    public Lang() {
        Init();
    }

	public bool Init()
	{
		if (isInit)
			return true;
		
		InitAllLangDict();
		
		// 解析器
		CSVParse parse = new CSVParse("csv/lang");
		
		// **前两行预留【ID】【Macro】,后面的是标题
		int lang_types = parse.heads.Length - 2;
		if (lang_types <= 0) {
			Debug.LogError("Invalid lang types in lang.csv");
			return false;
		}
		supportLangs = new string[lang_types];
		for (int i = 0; i < lang_types; ++i) {
			supportLangs[i] = parse.heads[2 + i];
			
			// 确保标题有效 en、zh...
			if (!allLangNameTypes.ContainsKey(supportLangs[i])) {
				Debug.LogError("Invalid lang head name:" + supportLangs[i]);
			}
		}

		Dictionary<int, Dictionary<string, string>> data = parse.csv_Data;
		foreach (var line in data)
		{
			int id = line.Key;
			string macro = parse.getStringByID(id, "Macro");

			string[] contents = new string[lang_types];
			for (int i = 0; i < lang_types; ++i) {
				string lang_name = supportLangs[i];
				contents[i] = parse.getStringByID(id, lang_name);
			}
			
			//  Debug.Log(string.Format("-------id:{0} macro:{1} content:{2}", id, macro, content));
			
			if (macro.Length != 0) {
#if UNITY_EDITOR
				if (macroIdList.ContainsKey(macro)) {
					Debuger.LogWarning(string.Format("Lang >> macro:{0} already exist!", macro));
				}
#endif
				try
				{
					macroIdList.Add(macro, id);
				}
				catch (System.Exception ex)
				{
					Debug.LogError("Unable to add '" + macro + "' to the Localization dictionary.\n" + ex.Message);
				}
			}
			
			idContentList.Add(id, contents);
		}

		InitDefaultLang();

		isInit = true;
		return true;
	}
	
	// 初始化所有所有语言的字典
	void InitAllLangDict()
	{
		allLangNameTypes.Clear();
		allLangTypeNames.Clear();
		
		allLangNameTypes.Add("af", SystemLanguage.Afrikaans);
		allLangNameTypes.Add("ar", SystemLanguage.Arabic);
		allLangNameTypes.Add("eu", SystemLanguage.Basque);
		allLangNameTypes.Add("be", SystemLanguage.Belarusian);
		allLangNameTypes.Add("bg", SystemLanguage.Bulgarian);
		allLangNameTypes.Add("ca", SystemLanguage.Catalan);
		allLangNameTypes.Add("zh", SystemLanguage.Chinese);
		allLangNameTypes.Add("cs", SystemLanguage.Czech);
		allLangNameTypes.Add("da", SystemLanguage.Danish);
		allLangNameTypes.Add("nl", SystemLanguage.Dutch);
		allLangNameTypes.Add("en", SystemLanguage.English);
		allLangNameTypes.Add("et", SystemLanguage.Estonian);
		allLangNameTypes.Add("fo", SystemLanguage.Faroese);
		allLangNameTypes.Add("fu", SystemLanguage.Finnish);
		allLangNameTypes.Add("fr", SystemLanguage.French);
		allLangNameTypes.Add("de", SystemLanguage.German);
		allLangNameTypes.Add("el", SystemLanguage.Greek);
		allLangNameTypes.Add("he", SystemLanguage.Hebrew);
		allLangNameTypes.Add("is", SystemLanguage.Icelandic);
		allLangNameTypes.Add("id", SystemLanguage.Indonesian);
		allLangNameTypes.Add("it", SystemLanguage.Italian);
		allLangNameTypes.Add("ja", SystemLanguage.Japanese);
		allLangNameTypes.Add("ko", SystemLanguage.Korean);
		allLangNameTypes.Add("lv", SystemLanguage.Latvian);
		allLangNameTypes.Add("lt", SystemLanguage.Lithuanian);
		allLangNameTypes.Add("nn", SystemLanguage.Norwegian);
		allLangNameTypes.Add("pl", SystemLanguage.Polish);
		allLangNameTypes.Add("pt", SystemLanguage.Portuguese);
		allLangNameTypes.Add("ro", SystemLanguage.Romanian);
		allLangNameTypes.Add("ru", SystemLanguage.Russian);
		allLangNameTypes.Add("sr", SystemLanguage.SerboCroatian);
		allLangNameTypes.Add("sk", SystemLanguage.Slovak);
		allLangNameTypes.Add("sl", SystemLanguage.Slovenian);
		allLangNameTypes.Add("es", SystemLanguage.Spanish);
		allLangNameTypes.Add("sv", SystemLanguage.Swedish);
		allLangNameTypes.Add("th", SystemLanguage.Thai);
		allLangNameTypes.Add("tr", SystemLanguage.Turkish);
		allLangNameTypes.Add("uk", SystemLanguage.Ukrainian);
		allLangNameTypes.Add("vi", SystemLanguage.Vietnamese);
		allLangNameTypes.Add("zh_Hans", SystemLanguage.ChineseSimplified);
		allLangNameTypes.Add("zh_Hant", SystemLanguage.ChineseTraditional);
		allLangNameTypes.Add("hu", SystemLanguage.Hungarian);
		
		foreach (var item in allLangNameTypes)
		{
			allLangTypeNames.Add(item.Value, item.Key);
		}
	}
	
	// 初始化当前语言
	void InitDefaultLang()
	{
		curLang = GetLangNameInRecord();
		
		for (int i = 0; i < supportLangs.Length; ++i)
		{
			if (supportLangs[i] == curLang)
				curLangIdx = i;
		}
	}
	
	// 检测是否拥有指定宏(key)
	public bool Contains(string macro)
	{
		int id = -1;
		if (macroIdList.TryGetValue(macro, out id)) {
			return idContentList.ContainsKey(id);
		}
		return false;
	}
	
	public bool TryGetValue(string macro, out string[] value)
	{
		int id = -1;
		if (macroIdList.TryGetValue(macro, out id)) {
			if (idContentList.TryGetValue(id, out value)) {
				return true;
			}
		}

		value = null;
		return false;
	}
	
	// 支持的语言名字列表.即CSV中填写的多语言
	public string[] SupportLangs
	{
		get
		{
			if (!isInit) 
				Init();
				
			return supportLangs;
		}
	}
	
   // 获得当前系统对应的语言名 eg."en"、"zh"...
   // 如果无法识别,返回默认语言
   string GetSystemLangName()
   {
	   SystemLanguage sl = Application.systemLanguage;
	   string name;
	   
#if UNITY_EDITOR
		if (Constant.IGNORE_SYSTEM_LANG) {
			name = "";
		}
#else
		if (allLangTypeNames.TryGetValue(sl, out name)) {
			return name;
		}
#endif
	   
	   return Constant.DEFUALT_LANG_NAME;
   }
   
   // 获得记录中的语言名字
   string GetLangNameInRecord()
   {
	   string name = PlayerPrefs.GetString(PLAYERPREF_LANG_NAME, "");

#if UNITY_EDITOR
		// Ignore system language
		if (Constant.IGNORE_RECORD_LANG) {
			name = "";
		}
#endif

	   if (IsValidLangName(name)) {
		   return name;
	   }
	   
	   return GetSystemLangName();
   }
   
   // 检索语言名是否有效 
   bool IsValidLangName(string lang_name)
   {
	   return allLangNameTypes.ContainsKey(lang_name);
   }
	
	// 当前使用的语言名字
	public string Language
	{
		get
		{
			if (string.IsNullOrEmpty(curLang))
			{
				curLang = GetLangNameInRecord();
				LoadAndSelect(curLang);
			}
			return curLang;
		}
		set
		{
			if (!IsValidLangName(value)) {
				Debuger.LogWarning("Set a invalid lang name:" + value);
				value = Constant.DEFUALT_LANG_NAME;
			}
			
			if (curLang != value)
			{
				curLang = value;
				LoadAndSelect(value);
			}
		}
	}
	
	// 加载指定的语言
	bool LoadAndSelect(string value)
	{
		if (!string.IsNullOrEmpty(value))
		{
			if (SelectLanguage(value)) 
				return true;
		}

		// Either the language is null, or it wasn't found
		if (string.IsNullOrEmpty(value)) 
			PlayerPrefs.DeleteKey(PLAYERPREF_LANG_NAME);
		return false;
	}
	
	// Select the specified language
	bool SelectLanguage (string language)
	{
		curLangIdx = -1;

		for (int i = 0, imax = supportLangs.Length; i < imax; ++i)
		{
			if (supportLangs[i] == language)
			{
				curLangIdx = i;
				curLang = language;
				PlayerPrefs.SetString(PLAYERPREF_LANG_NAME, curLang);
				
				//  if (onLocalize != null) 
				//  	onLocalize();
				UIRoot.Broadcast("OnLocalize");
				return true;
			}
		}
		return false;
	}

	// 通过ID查找多语言
	public string getLangByID(int id)
	{
		if (curLangIdx < 0) {
			return "null0";
		}

		string[] contents;
		if (idContentList.TryGetValue(id, out contents)) {
			if (curLangIdx < contents.Length) {
				return contents[curLangIdx];
			}

			Debuger.LogError(string.Format("Invalid get lang id:{0} langIdx:{1} contentsLen:{2}", id, curLangIdx, contents.Length));
		}
		
		return "null1";
	}
	
	// 通过宏(key)查找多语言
	public string getLangByMacro(string macro)
	{
		int id = -1;
		if (macroIdList.TryGetValue(macro, out id)) {
			return getLangByID(id);
		}
		
		return "null";
	}
	
	// 通过宏获得对应的ID，获得ID可以随机区间
	public int getIDByMacro(string macro)
	{
		int id = -1;
		if (macroIdList.TryGetValue(macro, out id)) {
			return id;
		}
		
		Debug.LogError("Invalid id in lang csv:" + macro);
		return -1;
	}

	// 获得字符串(简化函数)
	public static string STR(int id)
	{
		return Instance.getLangByID(id);
	}
	
	public static string STR(string macro)
	{
		return Instance.getLangByMacro(macro);
	}
  
}
**/