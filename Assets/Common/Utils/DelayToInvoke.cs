/*
 * 
 * 延迟调用一个方法
 * 
 * mxdy
 *
 * 
 * 
 * exp:延迟一秒跳转页面
 * StartCoroutine(DelayToInvoke.DelayIt(() =>
    {
        Application.LoadLevel("GameFaildScene");
    }, 1f));
 */
using UnityEngine;
using System.Collections;
using System;

public class DelayToInvoke : MonoBehaviour {

    public static IEnumerator DelayIt(Action action, float delayTime) {
        yield return new WaitForSeconds(delayTime);
        action();
    }
}
