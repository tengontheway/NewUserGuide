/**
	手机本地推送 仅支持iOS，Andriod需要开启Services
	
	1.晚9点到早8点不发送任何推送,期间的任何推送推迟到早上8点推送
	2.可以定制推送区间
	3.在派生类中定制本地推送
	
	Added by Teng.【2015/08/20】
**/
using UnityEngine;
#if UNITY_IOS
using UnityEngine.iOS;
#endif
using System;
 
public class Notification : MonoBehaviour {
	// 每日推送的最早时间
	public static TimeSpan beginTime = new TimeSpan(8, 0, 0);
	
	// 每日推送的最晚时间
	public static TimeSpan endTime = new TimeSpan(21, 0, 0);
	
	//本地推送:当天的 小时:分钟:秒 触发
	// param: bool isRepeatDay 每天定时触发
	// param: int hour [0, 24]
	// param: int minute [0, 60]
	// param: int second [0, 60]
	public static void NotificationMessage(string message, int hour, int minute, int second, bool isRepeatDay)
	{
		int year = System.DateTime.Now.Year;
		int month = System.DateTime.Now.Month;
		int day= System.DateTime.Now.Day;
		
		System.DateTime newDate = new System.DateTime(year, month, day, hour, minute, second);
		NotificationMessage(message, newDate, isRepeatDay);
	}
	
	// 本地推送:当天的 小时:分钟:秒 触发
	// param int totalSeconds 总时间(秒) XX秒后触发
	public static void NotificationMessage(string message, int totalSeconds, bool isRepeatDay)
	{
		int year = System.DateTime.Now.Year;
		int month = System.DateTime.Now.Month;
		int day= System.DateTime.Now.Day;
		
		System.DateTime newDate = new System.DateTime(year, month, day, 0, 0, 0);
		newDate += new TimeSpan(TimeSpan.TicksPerSecond * totalSeconds);
		
		NotificationMessage(message, newDate, isRepeatDay);
	}
	
	
	//本地推送 传入一个固定的推送时间
	public static void NotificationMessage(string message, System.DateTime newDate, bool isRepeatDay)
	{
		// 修正推送时间，更加人性化
		newDate = adjustDateTime(newDate);
#if UNITY_IOS
		//推送时间需要大于当前时间
		if(newDate > System.DateTime.Now)
		{
			UnityEngine.iOS.LocalNotification localNotification = new UnityEngine.iOS.LocalNotification();
			localNotification.fireDate =newDate;	
			localNotification.alertBody = message;
			localNotification.applicationIconBadgeNumber = 1;
			localNotification.hasAction = true;
			if(isRepeatDay)
			{
				//是否每天定期循环
				localNotification.repeatCalendar = UnityEngine.iOS.CalendarIdentifier.ChineseCalendar;
				localNotification.repeatInterval = UnityEngine.iOS.CalendarUnit.Day;
			}
			localNotification.soundName = UnityEngine.iOS.LocalNotification.defaultSoundName;
			UnityEngine.iOS.NotificationServices.ScheduleLocalNotification(localNotification);
		}
#endif
	}
 
	void Awake()
	{
		//第一次进入游戏的时候清空，有可能用户自己把游戏冲后台杀死，这里强制清空
		CleanNotification();
	}
	
	void OnApplicationPause(bool paused)
	{
		//程序进入后台时
		if(paused)
		{
			CustomLocalNotification();
		}
		else
		{
			//程序从后台进入前台时
			CleanNotification();
		}
	}
 
	//清空所有本地消息
	void CleanNotification()
	{
#if UNITY_IOS
		UnityEngine.iOS.LocalNotification l = new UnityEngine.iOS.LocalNotification (); 
		l.applicationIconBadgeNumber = -1; 
		UnityEngine.iOS.NotificationServices.PresentLocalNotificationNow (l); 
		UnityEngine.iOS.NotificationServices.CancelAllLocalNotifications (); 
		UnityEngine.iOS.NotificationServices.ClearLocalNotifications ();
#endif 
	}
	
	// 修正推送时间
	// 考虑到人性化的设计,晚上时间不发送,期间的任何推送推迟到早上8点
	// 05/06/2015 00:00:01 => 05/06/2015 00:00:01
	static DateTime adjustDateTime(DateTime dt)
	{
		int hour = dt.Hour;
		int minute = dt.Minute;
		int second = dt.Second;
		
		TimeSpan span = new TimeSpan(hour, minute, second);
		
		if (span < beginTime) {
			dt = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0, DateTimeKind.Local);
			dt += beginTime;
		} else if (span > endTime) {
			dt = new DateTime(dt.Year, dt.Month, dt.Day + 1, 0, 0, 0, DateTimeKind.Local);
			dt += beginTime;
		}
		
		return dt;
	}
	
	// 测试调整时间
	void DebugAdjustDateTime(DateTime dt)
	{
		DateTime dtx = adjustDateTime(dt);
		Debug.Log(string.Format("{0} => {1}", dt.ToString(), dtx.ToString()));
	}
	
	// 定制本地的推送
	public virtual void CustomLocalNotification()
	{
		/**
		if (!SandBox.Instance.IsEnergyFull()) {
			// 差XX体力恢复满
			int remains = Constant.MAX_ENERGY - SandBox.Instance.Energy;
			int totalSeconds = remains * Constant.AUTO_RECOVERY_ENERGY_TIME;
			
			DateTime server_time = SandBox.Instance.ServerDateTime();
        	TimeSpan ts = server_time - SandBox.Instance.LastEnergyGain;
			int seconds = (int)ts.TotalSeconds % Constant.AUTO_RECOVERY_ENERGY_TIME;
			
			totalSeconds -= seconds;
			
			if (totalSeconds > 0) {
				// 恢复体力满时间
				NotificationMessage("主人,我的体力已经恢复满了,快来参加比赛吧！", System.DateTime.Now.AddSeconds(totalSeconds), false);
				
				// 三小时后
				NotificationMessage("主人,你让我闲着好无聊哇,快来陪我一起赛跑吧！", System.DateTime.Now.AddSeconds(totalSeconds + 3 * 3600), false);
				
				// 六小时后
				NotificationMessage("主人,你难道把我忘记了吗？", System.DateTime.Now.AddSeconds(totalSeconds + 6 * 3600), false);
			}
		}
		
		//每天晚9点推送
		NotificationMessage("今日的比赛奖励尚未领取,快来参赛领取奖励吧!", 21, 0, 0, true);
		
		NotificationMessage("勇士,热乎的地图已开启,快来挑战你的小伙伴们吧!", 11, 30, 0, true);
		*/
	}
}