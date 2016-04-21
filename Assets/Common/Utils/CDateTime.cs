/**
 * 时间类 
 **/
using System;
using System.Collections.Generic;

public class CDateTime
{
    /// <summary>
    /// 将Unix时间戳转换为DateTime类型时间
    /// 通过获取本地时区表示Unixk开始时间，加上Unix时间值（即过去的秒数）. 
    /// 格林威治标准时间1970-01-01 00:00:00
    /// </summary>
    /// <param name="d">long 型数字</param>
    /// <returns>DateTime</returns>
    public static DateTime toDateTime(long d)
    {
        // 方法1:
        System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
        DateTime time= dtDateTime.AddSeconds(d);

        //  DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        //  DateTime time = startTime.AddSeconds(d);
        return time;
    }

    /// <summary>
    /// 将c# DateTime时间格式转换为Unix时间戳格式
    /// 通过刻度数差，再把刻度数转换为秒数的Unix时间戳格式
    /// 1970.1.1 => -28800
    /// 2015.5.28 => 1432742400
    /// </summary>
    /// <param name="time">时间</param>
    /// <returns>double</returns>
    public static long toVal(DateTime time)
    {
        //// 方法1：
        //long tm = (time.ToUniversalTime().Ticks - 621355968000000000) / 10000000;

        //  // 方法2:
        //  DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));
        //  double seconds = (time - startTime).TotalSeconds;
        //  return (long)seconds;

        TimeSpan span = time - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        return (long)span.TotalSeconds;
    }

}

