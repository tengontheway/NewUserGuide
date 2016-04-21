/**
    设置特定的一些本地保存
    add by mxdy
**/

using UnityEngine;
using System.Collections;

public static class CustomPrefs
{
    // 是否播放过开场动画
    const string START_ANIM_FINISHED = "WaitedStartAnim";
    
    const string FINISHED_TEACH_ID = "FinishedTeachID";
    
    const string INTERRUPT_TEACH_ID = "InterruptTeachID";
    const string INTERRUPT_TEACH_STEP_ID = "InterruptTeachStepID";
    
    public static int GetIntValue(string key, int default_val)
   {
       return PlayerPrefs.GetInt(key, default_val);
   }
   
   public static bool GetBoolValue(string key, bool default_val)
   {
       return PlayerPrefsEx.GetBool(START_ANIM_FINISHED);
   }
   
   public static void SetIntValue(string key, int val)
   {
       PlayerPrefs.SetInt(key, val);
   }
   
   public static void SetBoolValue(string key, bool val)
   {
       PlayerPrefsEx.SetBool(key, val);
   }
    
     public static bool isStartAnimFinished()
    {
        return PlayerPrefsEx.GetBool(START_ANIM_FINISHED);
    }

    // 开场动画是否播放完毕
    public static void SetStartAnimFinished(bool finished = true)
    {
        PlayerPrefsEx.SetBool(START_ANIM_FINISHED, finished);
    }
   
   #region Teach
   public static int GetFinishedTeachID()
   {
       return PlayerPrefs.GetInt(FINISHED_TEACH_ID, -1);
   }
   
   public static int GetInterruptTeachID()
   {
       return PlayerPrefs.GetInt(INTERRUPT_TEACH_ID, -1);
   }
   
   public static int GetInterruptTeachStepID()
   {
       return PlayerPrefs.GetInt(INTERRUPT_TEACH_STEP_ID, -1);
   }
   
   public static void SetFinishedTeachID( int fin_teach_id)
   {
       PlayerPrefs.SetInt(FINISHED_TEACH_ID, fin_teach_id);
   }
   
    public static void SetInterruptTeachID( int teach_id)
   {
       PlayerPrefs.SetInt(FINISHED_TEACH_ID, teach_id);
   }
   
    public static void SetInterruptTeachStepID( int teach_step_id)
   {
       PlayerPrefs.SetInt(FINISHED_TEACH_ID, teach_step_id);
   }
   
   public static void ClearTeachRecord()
   {
       PlayerPrefs.DeleteKey(FINISHED_TEACH_ID);
       PlayerPrefs.DeleteKey(INTERRUPT_TEACH_ID);
       PlayerPrefs.DeleteKey(INTERRUPT_TEACH_STEP_ID);
   }
   #endregion
 
}
