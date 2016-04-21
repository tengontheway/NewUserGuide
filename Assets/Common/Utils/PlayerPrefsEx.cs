/**
 	PlayerPrefs的扩展
	实现游戏数据的保存和读取
	1.扩展允许读写向量、bool数组、int数组、float数组、string数组
	2.保存数组时,数组为空则删除此字段
    3.所有接口不涉及到保存
	
	Added by Teng.
 **/
using UnityEngine;
using System;

public static class PlayerPrefsEx
{
    #region Bool 
    
    /// <summary>
    /// Stores a bool value into a Key
    /// </summary>
    public static void SetBool(string key, bool val)
    {
        PlayerPrefs.SetInt(key, val ? 1 : 0);
    }
    
    public static bool GetBool(string key)
    {
        int val = PlayerPrefs.GetInt(key, 0);
        return val != 0;
    }
    
    #endregion
    
    #region Vector 3

    /// <summary>
    /// Stores a Vector3 value into a Key
    /// </summary>
    public static bool SetVector3(string key, Vector3 vector)
    {
        return SetFloatArray(key, new float[3] { vector.x, vector.y, vector.z });
    }

    /// <summary>
    /// Finds a Vector3 value from a Key
    /// </summary>
    public static Vector3 GetVector3(string key)
    {
        float[] floatArray = GetFloatArray(key);
        if (floatArray.Length < 3)
            return Vector3.zero;
        return new Vector3(floatArray[0], floatArray[1], floatArray[2]);
    }

    #endregion

    #region Bool Array

    /// <summary>
    /// Stores a Bool Array or Multiple Parameters into a Key
    /// </summary>
    public static bool SetBoolArray(string key, params bool[] boolArray)
    {
        if (boolArray.Length == 0)
        {
            PlayerPrefs.DeleteKey(key);
            return true;
        }

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        for (int i = 0; i < boolArray.Length - 1; i++)
            sb.Append(boolArray[i]).Append("|");
        sb.Append(boolArray[boolArray.Length - 1]);

        try
        {
            PlayerPrefs.SetString(key, sb.ToString());
        }
        catch (Exception e)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// Returns a Bool Array from a Key
    /// </summary>
    public static bool[] GetBoolArray(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            string[] stringArray = PlayerPrefs.GetString(key).Split('|');
            bool[] boolArray = new bool[stringArray.Length];
            for (int i = 0; i < stringArray.Length; i++)
                boolArray[i] = Convert.ToBoolean(stringArray[i]);
            return boolArray;
        }

        return new bool[0];
    }

    /// <summary>
    /// Returns a Bool Array from a Key
    /// Note: Uses default values to initialize if no key was found
    /// </summary>
    public static bool[] GetBoolArray(string key, bool defaultValue, int defaultSize)
    {
        if (PlayerPrefs.HasKey(key))
            return GetBoolArray(key);
        bool[] boolArray = new bool[defaultSize];
        for (int i = 0; i < defaultSize; i++)
            boolArray[i] = defaultValue;
        return boolArray;
    }

    #endregion

    #region Int Array

    /// <summary>
    /// Stores a Int Array or Multiple Parameters into a Key
    /// </summary>
    public static bool SetIntArray(string key, params int[] intArray)
    {
        if (intArray.Length == 0)
        {
            PlayerPrefs.DeleteKey(key);
            return true;
        }

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        for (int i = 0; i < intArray.Length - 1; i++)
            sb.Append(intArray[i]).Append("|");
        sb.Append(intArray[intArray.Length - 1]);

        try { PlayerPrefs.SetString(key, sb.ToString()); }
        catch (Exception e) { return false; }
        return true;
    }

    /// <summary>
    /// Returns a Int Array from a Key
    /// </summary>
    public static int[] GetIntArray(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            string[] stringArray = PlayerPrefs.GetString(key).Split('|');
            int[] intArray = new int[stringArray.Length];
            for (int i = 0; i < stringArray.Length; i++)
                intArray[i] = Convert.ToInt32(stringArray[i]);
            return intArray;
        }
        return new int[0];
    }

    /// <summary>
    /// Returns a Int Array from a Key
    /// Note: Uses default values to initialize if no key was found
    /// </summary>
    public static int[] GetIntArray(string key, int defaultValue, int defaultSize)
    {
        if (PlayerPrefs.HasKey(key))
            return GetIntArray(key);
        int[] intArray = new int[defaultSize];
        for (int i = 0; i < defaultSize; i++)
            intArray[i] = defaultValue;
        return intArray;
    }

    #endregion

    #region Float Array

    /// <summary>
    /// Stores a Float Array or Multiple Parameters into a Key
    /// </summary>
    public static bool SetFloatArray(string key, params float[] floatArray)
    {
        if (floatArray.Length == 0)
        {
            PlayerPrefs.DeleteKey(key);
            return true;
        }

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        for (int i = 0; i < floatArray.Length - 1; i++)
            sb.Append(floatArray[i]).Append("|");
        sb.Append(floatArray[floatArray.Length - 1]);

        try
        {
            PlayerPrefs.SetString(key, sb.ToString());
        }
        catch (Exception e)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// Returns a Float Array from a Key
    /// </summary>
    public static float[] GetFloatArray(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            string[] stringArray = PlayerPrefs.GetString(key).Split('|');
            float[] floatArray = new float[stringArray.Length];
            for (int i = 0; i < stringArray.Length; i++)
                floatArray[i] = Convert.ToSingle(stringArray[i]);
            return floatArray;
        }
        return new float[0];
    }

    /// <summary>
    /// Returns a String Array from a Key
    /// Note: Uses default values to initialize if no key was found
    /// </summary>
    public static float[] GetFloatArray(string key, float defaultValue, int defaultSize)
    {
        if (PlayerPrefs.HasKey(key))
            return GetFloatArray(key);
        float[] floatArray = new float[defaultSize];
        for (int i = 0; i < defaultSize; i++)
            floatArray[i] = defaultValue;
        return floatArray;
    }

    #endregion

    #region String Array

    /// <summary>
    /// Stores a String Array or Multiple Parameters into a Key w/ specific char seperator
    /// </summary>
    public static bool SetStringArray(string key, char separator, params string[] stringArray)
    {
        if (stringArray.Length == 0)
        {
            PlayerPrefs.DeleteKey(key);
            return true;
        }

        try
        {
            PlayerPrefs.SetString(key, String.Join(separator.ToString(), stringArray));
        }
        catch (Exception e)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// Stores a Bool Array or Multiple Parameters into a Key
    /// </summary>
    public static bool SetStringArray(string key, params string[] stringArray)
    {
        if (!SetStringArray(key, '\n', stringArray))
            return false;
        return true;
    }

    /// <summary>
    /// Returns a String Array from a key & char seperator
    /// </summary>
    public static string[] GetStringArray(string key, char separator)
    {
        if (PlayerPrefs.HasKey(key))
            return PlayerPrefs.GetString(key).Split(separator);
        return new string[0];
    }

    /// <summary>
    /// Returns a Bool Array from a key
    /// </summary>
    public static string[] GetStringArray(string key)
    {
        if (PlayerPrefs.HasKey(key))
            return PlayerPrefs.GetString(key).Split('\n');
        return new string[0];
    }

    /// <summary>
    /// Returns a String Array from a key & char seperator
    /// Note: Uses default values to initialize if no key was found
    /// </summary>
    public static string[] GetStringArray(string key, char separator, string defaultValue, int defaultSize)
    {
        if (PlayerPrefs.HasKey(key))
            return PlayerPrefs.GetString(key).Split(separator);
        string[] stringArray = new string[defaultSize];
        for (int i = 0; i < defaultSize; i++)
            stringArray[i] = defaultValue;
        return stringArray;
    }

    /// <summary>
    /// Returns a String Array from a key
    /// Note: Uses default values to initialize if no key was found
    /// </summary>
    public static String[] GetStringArray(string key, string defaultValue, int defaultSize)
    {
        return GetStringArray(key, '\n', defaultValue, defaultSize);
    }

    #endregion
}
