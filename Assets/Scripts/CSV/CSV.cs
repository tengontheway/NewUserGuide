using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CSV<T> where T : class, new()
{
    private static T myInstance;

    public static T instance
    {
        get
        {
            if (myInstance == null)
            {
                myInstance = new T();
            }
            return myInstance;
        }
    }

    /// <summary>
    /// 读取csv数据
    /// </summary>
    public CSVParse getCsv(string csvName)
    {
        return new CSVParse("csv/" + csvName);
    }

    /// <summary>
    /// 获得csv数据的key列表
    /// </summary>
    /// <param name="parse"></param>
    /// <returns></returns>
    public List<int> getKeyList(CSVParse parse)
    {
        List<int> keylist = new List<int>();
        foreach (int id in parse.csv_Data.Keys)
        {
            keylist.Add(id);
        }
        return keylist;
    }
}
