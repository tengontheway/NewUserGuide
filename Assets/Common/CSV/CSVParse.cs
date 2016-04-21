/**
	解析CSV
	
	1.第一列为ID列，为整数
	2.//或第一列为空,当前行为注释行，丢弃
	Added by QingXin
	
	3.增加了对包含","分隔符的内容解析，CSV会为表中有分隔符的内容自动增加""
	eg. 您好,朋友！ => CSV自动处理为:"您好，朋友!"
	
	Modified by Teng.
**/
using UnityEngine;
using System.Collections.Generic;
using System;

public class IDRepeatException : ApplicationException//由用户程序引发，用于派生自定义的异常类型
{  
	/// <summary>  
	/// 默认构造函数  
	/// </summary>  
	public IDRepeatException()
	{
	} 
	
	public IDRepeatException(string message)  
		: base(message)
	{
	} 
	
	public IDRepeatException(string message, Exception inner)  
		: base(message, inner)
	{
	}  
	
}  

public class CSVParse
{
	// 分隔符
	const char SPLIT_CHAR = ',';
	string fileName;
	
	public CSVParse(string file_path)
	{
		fileName = file_path;
		
		try
		{
			if (!readCSVFile(file_path))
			{
				Debug.LogError("CSVParse fail");
			}
		} catch (Exception e)
		{
			
		}
		
	}
	
	public CSVParse()
	{
		
	}
	
	private Dictionary<int, Dictionary<string, string>> csv_data = new Dictionary<int, Dictionary<string, string>>();
	
	// 标题
	public string[] heads;
	
	// Key:int:ID Val:Dictionary:(Key:string:HeadName, Val:string:Val)
	public Dictionary<int, Dictionary<string, string>> csv_Data
	{
		get
		{
			return csv_data;
		}
	}
	
	private bool ID_repeat_check = false;
	
	public bool readCSVFile(string file_path)
	{
		/**
         * 
         *  第一列(ID列) 如果以 // 开头, 就认为是注释行. 后边的数据都不读取. (直接丢弃)
         *  
         *  第一列(ID列) 如果以 为空白 放弃这一样.
         *  
         *  第一列(ID列) 如果把字符串转成数字的时候, 发生异常, 就抛出报错.
         * 
         *  *ID重复检验. 做成开关, 当打开时, 每读取一个ID的时候, 都需要和前面的所有ＩＤ判断是否重复．　否则报错．
         * 
         * 
         * */
		
		
		//读取文本文件
		TextAsset binAsset = Resources.Load(file_path, typeof(TextAsset)) as TextAsset;
		
		if (binAsset == null)
		{
			Debug.LogError("CSV file read error!");
			throw new System.ApplicationException();           
		}
		
		// load from text.
		return readCSVFileFromText(binAsset.text);
	}
	
	public bool readCSVFileFromText(string text)
	{
		//将每一行存入数组,去掉换行符(windows下换行符为\r\n)
		string[] line_array = text.Split(new char[]{'\r','\n'}, StringSplitOptions.RemoveEmptyEntries);
		
		bool first_line = true;
		int _id = -1;
		int head_cnt = 0;
		
		//从第2行开始读取数据,根据ID和对应的heads存入csv_data中
		for (int i = 0; i < line_array.Length; i++)
		{
			string line_text = line_array[i];
			
			if (string.IsNullOrEmpty(line_text))
				continue;
			
			int pos = 0;
			int rows = 0;
			int col_idx = 0;
			
			Dictionary<string,string> dic = new Dictionary<string, string>();
			
			while (pos < line_text.Length)
			{
				string value;
				
				// 引号区域特殊处理
				if (line_text[pos] == '"')
				{
					// 跳过第一个引号
					pos++;
					
					// 解析引号中的内容
					int start = pos;
					while (pos < line_text.Length)
					{
						// 引号配对测试
						if (line_text[pos] == '"')
						{
							// 引号计数
							pos++;
							
							// 如果两个引号在一起,使用一个(如果表格中有引号"，CSV会自动生成两个"")
							// 否则,就是表示的值
							if (pos >= line_text.Length || line_text[pos] != '"')
							{
								pos--;
								break;
							}
						}
						pos++;
					}
					value = line_text.Substring(start, pos - start);
					value = value.Replace("\"\"", "\"");
				}
				else
				{
					// 解析非引号
					int start = pos;
					while (pos < line_text.Length && line_text[pos] != SPLIT_CHAR)
						pos++;
					
					value = line_text.Substring(start, pos - start);
				}
				
				value.Trim();
				
				// 第一列
				if (col_idx == 0) {
					// 空行
					if (string.IsNullOrEmpty(value))
						break;
					
					// 注释行
					if (value.StartsWith("//") || value.StartsWith("\""))
						break;
					
					// 非第一行(标题行),解析ID
					if (!first_line) {
						//将ID从string类型转换为int类型,错误是抛出异常
						try
						{
							_id = int.Parse(value);
						} catch (FormatException)
						{
							Debug.LogError("int.parse failed by format exception" + i);
						} catch (OverflowException)
						{
							Debug.LogError("int.parse failed by overflow exception" + i);
						}
						
						//检查ID是否重复
						if (ID_repeat_check)
						{
							string[] pit = new string[line_array.Length];
							
							if (pit [_id] != null)
							{
								pit [_id] = value;
							} else
							{
								throw new IDRepeatException("CSV file :ID repeat!!!" + i);                    
							}
						}
					}
				}
				
				// 第一行,解析标题
				if (first_line) {
					heads = line_text.Split(SPLIT_CHAR);
					
					first_line = false;
					head_cnt = heads.Length;
					break;
				}
				
				if (col_idx >= 1) {
					dic.Add(heads[col_idx], value);
				}
				
				// 移动到下一个分隔符
				while (pos < line_text.Length && line_text[pos] != SPLIT_CHAR)
					pos++;

				if (pos < line_text.Length) {
					pos++;

					// 最后一个分隔符
					if (pos >= line_text.Length) {
						if (col_idx + 1 < head_cnt)
							dic.Add(heads[col_idx + 1], "");
					}
				}					
				
				col_idx++;
			}
			
			if (dic.Count > 0)
				csv_data.Add(_id, dic);
		}
		return true;
	}
	
	/// <summary>
	/// 根据对象ID和KEY值,返回制定的value
	/// 
	/// 
	/// QingXin.
	/// </summary>
	public string getStringByID(int id, string key)
	{
		if (csv_data == null)
		{
			return null;
		}
		
		if (csv_data.ContainsKey(id) == false)
		{
			return null;
		}
		
		return csv_data [id] [key];
	}
	
	/// <summary>
	/// 从CSV中获取一个int
	/// </summary>
	/// <param name="id"></param>
	/// <param name="key"></param>
	/// <returns></returns>
	public int getIntByID(int id, string key)
	{
		int x;
		//先读取x,y,z的值
		if (!int.TryParse(getStringByID(id, key), out x))
		{
			#if UNITY_EDITOR
			Debug.LogError("TryParse error. ID:" + id + " Key:" + key + " file:" + fileName);
			#endif
			return 0;
		}
		
		return x;
	}
	
	/// <summary>
	/// 从CSV中获取一个float
	/// </summary>
	/// <param name="id"></param>
	/// <param name="key"></param>
	/// <returns></returns>
	public float getFloatByID(int id, string key)
	{
		float x;
		//先读取x,y,z的值
		if (!float.TryParse(getStringByID(id, key), out x))
		{
			#if UNITY_EDITOR
			Debug.LogError("TryParse error. ID:" + id + " Key:" + key + " file:" + fileName);
			#endif
			return 0;
		}
		
		return x;
	}
	
	/// <summary>
	/// 从CSV中获取一个bool
	/// </summary>
	/// <param name="id"></param>
	/// <param name="key"></param>
	/// <returns></returns>
	public bool getBoolByID(int id, string key)
	{
		bool x;
		//先读取x,y,z的值
		if (!bool.TryParse(getStringByID(id, key), out x))
		{
			#if UNITY_EDITOR
			Debug.LogError("TryParse error. ID:" + id + " Key:" + key + " file:" + fileName);
			
			#endif
			return false;
		}
		
		return x;
	}
	
	public Dictionary<string, string> getDicByID(int id)
	{
		if (csv_data == null)
		{
			return null;
		}
		
		Dictionary<string, string> value;
		if (csv_data.TryGetValue(id, out value)) {
			return value;
		}
		
		return null;
	}
	
	// 检索是否存在指定行
	public bool isExist(int id)
	{
		var data = getDicByID(id);
		return data != null;
	}
	
	/// <summary>
	/// 获得数据行数
	/// </summary>
	/// <returns></returns>
	public int getLength() {
		int length = 0;
		foreach (int idx in csv_data.Keys) {
			length++;
		}
		return length;
	}
	
	
}
