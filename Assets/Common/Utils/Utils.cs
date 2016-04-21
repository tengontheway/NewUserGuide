/**
 *  公用的工具函数
 *  1.BitArray和Int64之间的转换，节省网络传递数据
 * */
using UnityEngine;
using System.Collections;
using System;

public class Utils : MonoBehaviour
{
    public static Utils _util;

    public static Utils instance()
    {
        if (_util == null)
        {
            _util = new Utils();
        }

        return _util;
    }

    void Start()
    {
        Debug.Log("---------------------");
        BitArray barray = new BitArray(128);
        barray.Set(0, true);
        Utils.printInt64Array(barray);

        barray.Set(9, true);
        Utils.printInt64Array(barray);

        barray.Set(62, true);
        Utils.printInt64Array(barray);

        barray.Set(63, true);
        Utils.printInt64Array(barray);

        barray.Set(64, true);
        Utils.printInt64Array(barray);

    }

    /// <summary>
    /// bitArray转换为int
    /// </summary>
    /// <param name="bitArray"></param>
    /// <returns></returns>
    public static int toInt(BitArray bitArray)
    {
        if (bitArray.Length > 32)
            throw new ArgumentException("Argument length shall be at most 64 bits.");

        int[] array = new int[1];
        bitArray.CopyTo(array, 0);
        return array[0];
    }

    /// <summary>
    /// 将bitarray转行成64位整数
    /// 多用于节省内存,如向服务器发送网络数据
    /// </summary>
    /// <param name="bitArray"></param>
    /// <returns></returns>
    public static Int64 toInt64(BitArray bitArray)
    {
        if (bitArray.Length > 64)
            throw new ArgumentException("Argument length shall be at most 64 bits.");

        Int64[] array = new Int64[1];
        bitArray.CopyTo(array, 0);
        return array[0];
    }

    /// <summary>
    /// 将位数组转换为int64数组
    /// 多用于节省内存,如向服务器发送网络数据
    /// BitArray barray = new BitArray(128);
    /// barray.Set(0, true);
    public static Int64[] toInt64Array(BitArray bitArray)
    {
        int len = bitArray.Length;
        int cnt = len / 64;
        if (len % 64 != 0)
        {
            cnt += 1;
        }
        //Debug.Log("Len:" + cnt);

        // int64占据8个byte
        byte[] arr = new byte[cnt * 8];
        bitArray.CopyTo(arr, 0);

        Int64[] ints = new Int64[cnt];
        for (int i = 0; i < cnt; ++i)
        {
            ints[i] = BitConverter.ToInt64(arr, i * 8);
        }
        return ints;
    }

    /// <summary>
    /// int转换为bitArray
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static BitArray toBitArray(Int64 value)
    {
        byte[] bytes = BitConverter.GetBytes(value);
        return new BitArray(bytes);
    }

    /// <summary>
    /// Int64[]转换为bitArray
    /// </summary>
    /// <param name="arr"></param>
    /// <returns></returns>
    public static BitArray toBitArray(Int64[] arr)
    {
        int len = arr.Length;

        // 总数据
        byte[] bytes = new byte[len * 8];
        for (int i = 0; i < arr.Length; ++i)
        {
            byte[] bts = BitConverter.GetBytes(arr[i]);
            bts.CopyTo(bytes, i * 8);
        }

        return new BitArray(bytes);
    }

    /// <summary>
    /// int转换为bitArray
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static BitArray toBitArray(int value)
    {
        byte[] bytes = BitConverter.GetBytes(value);
        return new BitArray(bytes);
    }

    // 测试函数:打印int64数组
    private static void printInt64Array(BitArray barray)
    {
        Debug.Log("--------------");

        Int64[] ints = Utils.toInt64Array(barray);
        for (int i = 0; i < ints.Length; ++i)
        {
            Int64 val = ints[i];
            Debug.Log("val:" + val);
        }
    }

    // 本地时间转格林威治时间
    public static DateTime LocalTime2GreenwishTime(DateTime lacalTime)
    {
        TimeZone localTimeZone = System.TimeZone.CurrentTimeZone;
        TimeSpan timeSpan = localTimeZone.GetUtcOffset(lacalTime);
        DateTime greenwishTime = lacalTime - timeSpan;
        return greenwishTime;
    }

    // 格林威治时间时间转本地时间
    public static DateTime GreenwishTime2LocalTime(DateTime greenwishTime)
    {
        TimeZone localTimeZone = System.TimeZone.CurrentTimeZone;
        TimeSpan timeSpan = localTimeZone.GetUtcOffset(greenwishTime);
        DateTime lacalTime = greenwishTime + timeSpan;
        return lacalTime;
    }
}
