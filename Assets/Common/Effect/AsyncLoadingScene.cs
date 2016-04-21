/**
	异步加载场景
	1.动态的返回加载进度
	2.加载进度平稳前进不跳跃
	3.如果在加载游戏主场景之前还需要解析数据表格，进行网络连接等操作，
	那么可以给这些操作赋予一个权重，利用权重就可以计算加载的进度
	
	eg.异步加载场景，并返回进度，加载完毕直接进入游戏
	 StartCoroutine(AsyncLoadingScene.load("game", (prog) =>
	            {
	                Debug.Log("Prog:" +(float) prog/100);
	                progressBar.value = (float)prog/100;
	            }));
				
    Added by Teng.
*/
using UnityEngine;
using System.Collections;
using System;

// 异步加载场景的信号
public class AsyncLoadingSingnal
{
	public bool changeSceneSingnal;
};

public class AsyncLoadingScene 
{	
	// 异步加载, 显示进度
	// param Action<int> loadingProg 返回加载进度
	// AsyncOperation.allowSceneActivation设置为false后，Unity就只会加载场景到90%，剩下的10%要等到allowSceneActivation设置为true后才加载
	public static IEnumerator load(string scene_name, Action<int> loadingProg)
	{
        int displayProgress = 0;
        int toProgress = 0;

        AsyncOperation op = Application.LoadLevelAsync(scene_name);

        op.allowSceneActivation = false;
        while (op.progress < 0.9f)
        {
            toProgress = (int)op.progress * 100;
            while (displayProgress < toProgress)
            {
                ++displayProgress;
                if (loadingProg != null)
                {
                    loadingProg(displayProgress);
                }

                yield return new WaitForEndOfFrame();
            }
            yield return 0;
        }

        //allowSceneActivation设置为false后，Unity就只会加载场景到90%，剩下的10%要等到allowSceneActivation设置为true后才加载
        toProgress = 100;
        while (displayProgress < toProgress)
        {
            ++displayProgress;
            if (loadingProg != null)
            {
                loadingProg(displayProgress);
            }
            yield return new WaitForEndOfFrame();
        }
        op.allowSceneActivation = true;
		yield return 0;
	}
	
	// 异步加载, 显示进度
	public static IEnumerator load(int scene_idx, Action<int> loadingProg)
	{
		int displayProgress = 0;
        int toProgress = 0;
		
		AsyncOperation op = Application.LoadLevelAsync(scene_idx);
        op.allowSceneActivation = false;
        while(op.progress < 0.9f) {
            toProgress = (int)op.progress * 100;
            while(displayProgress < toProgress) {
                ++displayProgress;
				if (loadingProg != null) {
					loadingProg(displayProgress);
				}

                yield return new WaitForEndOfFrame();
            }
			yield return 0;
        }
    	
		// allowSceneActivation设置为false后，Unity就只会加载场景到90%，剩下的10%要等到allowSceneActivation设置为true后才加载
        toProgress = 100;
        while(displayProgress < toProgress){
            ++displayProgress;
            if (loadingProg != null) {
				loadingProg(displayProgress);
			}
            yield return new WaitForEndOfFrame();
        }
        op.allowSceneActivation = true;
		yield return 0;
	}
	
	// 异步加载场景,加载完后(90%),等待信号切换场景
	// @param signal 异步加载等待切换场景信号(因为IEnumerator函数无法传递ref out,只能通过对象传递信号)
	// @param loadStart 异步加载的开始回调
	// @param loadFinish 异步加载结束(90%)回调
	public static IEnumerator loadWithSignal(string scene_name, AsyncLoadingSingnal signal, 
											Action<AsyncOperation> loadStart = null, Action<AsyncOperation> loadFinish = null)
	{
		AsyncOperation op = Application.LoadLevelAsync(scene_name);
		op.allowSceneActivation = false;
		
		if (loadStart != null) {
			loadStart(op);
		}
		
		 while(!op.isDone && op.progress < 0.9f) {
             yield return new WaitForEndOfFrame();
        }
		
		while(signal != null) {
			if (!signal.changeSceneSingnal)
                yield return new WaitForEndOfFrame();
			else
				break;
		}
		
		if (loadFinish != null) {
			loadFinish(op);
		}

        op.allowSceneActivation = true;
	}
}
