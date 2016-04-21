/*
    贝塞尔动画 奖励道具沿着贝塞尔曲线运动到指定位置
 
    added by mxdy 【2015/8/18】

    eg.
    Vector3 startPos = sprite2.transform.position;
    Vector3 endPos = sprite_2.transform.position;

    Transform node = sprite2.transform.Find("rb1");

    RewardBezier rb = node.GetComponent<RewardBezier>();
    rb.StartAni(startPos, endPos, Resources.Load("Prefabs/Coin") as GameObject, null, null);
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RewardBezier : MonoBehaviour {
    // 产生多少
    public int totalCount = 10;
    // 运动时间(时间单位 s)
    public Vector2 playTime;
    // x 轴控制点
    public List<Vector2> xVecList = new List<Vector2>();
    // y 轴控制点
    public List<Vector2> yVecList = new List<Vector2>();
    
    // 大小范围
    public Vector2 scaleVec;

    // 每次运动结束回调
    System.Action eachFinishAct;
    // 整个运动结束回调
    System.Action totalFinishAct;
    // 动画计数器
    int moveCount = 0;

    // 管理创建出来的对象
    List<GameObject> prebList = new List<GameObject>();

    // 产生一堆预制体同时做贝塞尔曲线动画
    // @param Vector3 start_pos 起始点
    // @param Vector3 end_pos 终止点
    // @param GameObject prefb 动画对象
    // @param System.Action each_finish_act 每次动画完成的回调
    // @param System.Action total_finish_act 整个动画完成的回调
    public void StartAni(Vector3 start_pos, Vector3 end_pos, GameObject prefb, System.Action each_finish_act, System.Action total_finish_act)
    {
        eachFinishAct = each_finish_act;
        totalFinishAct = total_finish_act;

        // 创建一堆准备运动的小伙伴
        for (int i = 0; i < totalCount; i++)
        {
                int curIdx = Random.Range(0, xVecList.Count);
                Vector3 controlPos1 = new Vector3(Random.Range(xVecList[curIdx].x, xVecList[curIdx].y), Random.Range(yVecList[curIdx].x, yVecList[curIdx].y), 0);
                Vector3 controlPos2 = new Vector3(0, 0, 0);

                GameObject prefb_clone = GameObject.Instantiate(prefb);
                prebList.Add(prefb_clone);
                prefb_clone.transform.SetParent(gameObject.transform);
                prefb_clone.GetComponent<UISprite>().MakePixelPerfect();
                prefb_clone.name = "bezier" + i;
                float scale = Random.Range(scaleVec.x, scaleVec.y);
                prefb_clone.transform.localScale = new Vector3(scale, scale, 0);

                // 播放动画
                Bezier bezier = new Bezier(start_pos, controlPos1, controlPos2, end_pos);
            
                StartCoroutine(UpdateAniPos(bezier, prefb_clone));
        }
    }

    // 运动中更新位置
    IEnumerator UpdateAniPos(Bezier bezier, GameObject prefb)
    {
        float time = Random.Range(playTime.x, playTime.y);
        Vector3 bezierPos;
        for (float t = 0; t <= time; )
        {
            float a = 2 / (time * time);
            t += Time.deltaTime;

            float v = t * t * a / 2;
            bezierPos = bezier.GetPointAtTime(v);
            prefb.transform.position = bezierPos;
            if (t >= time)
            {
                Destroy(prefb);
                moveCount++;

                if (eachFinishAct != null)
                {
                    eachFinishAct.Invoke();
                }

                if (totalFinishAct != null)
                {
                    if (moveCount == totalCount)
                    {
                        totalFinishAct.Invoke();
                        moveCount = 0;
                    }
                }
            }
            yield return 0;
        }
    }

    public void SetEachFinishAct(System.Action action)
    {
        eachFinishAct = action;
    }

    public void SetTotalFinishAct(System.Action action)
    {
        totalFinishAct = action;
    }

    public void ClearPrefbList()
    {
        for (int i = 0; i < prebList.Count; i++)
        {
            DestroyImmediate(prebList[i]);
        }
        prebList.Clear();
    }
}
