/**
    教学分组CSV
    
    Added by Teng.
**/
using UnityEngine;
using System.Collections.Generic;

public class TeachCSV : Singleton<TeachCSV>
{
	public List<int> teachIDList = new List<int>();
    
    CSVParse parse;
    
    public TeachCSV() {
        parse = new CSVParse("csv/Teach/teach");

		foreach(KeyValuePair<int, Dictionary<string, string>> kv in parse.csv_Data)
		{
			int id = kv.Key;
			teachIDList.Add(id);
		}
    }
    
    
    
}
