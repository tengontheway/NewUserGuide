/**
    实现了教学的入场、出场动画播放，和显示效果
    
    1.显示内容可以是一个文字列表, 逐个显示直至所有文字列表都显示完
    2.对话框分成两种模式:对话模式和其他系统的组件模式
      对话模式:全屏点击直接显示文字,再次点击结束,直至所有文字显示结束
      组件模式:并不具有全屏点击能力,有接口控制显示隐藏
    
    Added by LiuJie 【2015/08/16】
**/
using UnityEngine;
using System.Collections.Generic;

// 对话框类型
public enum TalkDlgType
{
    TalkDlg_Talk        = 1,                // 对话模式(全屏点击直接显示文字,再次点击结束)
    TalkDlg_ClickBtn    = 2,                // 教学点击按钮(并不具有全屏点击能力)
}
    
public class TeachTalkDlg : MonoBehaviour
{    
    //教学系统Panel
    public GameObject teachRoot;

    // 幽灵父对象
    public GameObject ghostParent;

    //幽灵
    Transform ghost;

    //教学描述
    Transform teachBg;
    
    public TalkDlgType dlgType = TalkDlgType.TalkDlg_Talk;
    
    // 聊天内容的langID列表
    public List<int>   talkContentsList;
    
    int contentIdx;
    
    int curContentID
    {
        get{
            return talkContentsList[contentIdx];
        }
    }
    
    System.Action   onFinishedCallback;
    
    // 教学主角正播放入场动画
    bool        isGhostEntering;
    
    // 播放聊天框出现动画 true只有首次开启才播放 false每次显示对话都播放
    bool        isFirstShowDescBGAnim = true;
    
    // 描述背景正在播放显示动画
    bool        isDescBGAniming   = false;
    
    // 高亮按钮父对象
    GameObject highLightTmpParent;
    GameObject highLightObjParent;
    GameObject highLightObj;

    /**
        屏幕的显示数据
    **/
    //test 关闭按钮
    bool isClose;
    public class Display
    {
        public Vector2 leftTop;
        public Vector2 top;
        public Vector2 rightTop;
        public Vector2 left;
        public Vector2 center;
        public Vector2 right;
        public Vector2 leftBottom;
        public Vector2 bottom;
        public Vector2 rightBottom;
    }

    Display display = new Display();

    TweenPosition tweenPos;

    //教学后背景，渐变显示/隐藏
    Transform teachPanelBg;
    
    // 描述标签
    Transform tranDesc;
    
    // 描述标签的打印机标签
    TypewriterEffect descTypeEffect;
    
    UILabel descLab;
    
    Transform  colliderBG;

	int highLightObjLayer;
    
    // 点击按钮代理
    EventDelegate clickDelegate;
    
    // 初始化
    // @param trigger_obj 触发对象,仅当点击按钮时有效
    public void Init(List<int> talk_content_list, System.Action cb_finished, TalkDlgType dlg_type, GameObject trigger_obj = null)
    {
        SetContentsList(talk_content_list);
        onFinishedCallback = cb_finished;
        dlgType = dlg_type;
        
		gameObject.SetActive(true);

		tweenPos = ghostParent.GetComponent<TweenPosition>();
		ghost = ghostParent.transform.Find("youling");
		teachBg = ghostParent.transform.Find("Teachbg");
		teachPanelBg = teachRoot.transform.Find("BG");
        
        tranDesc = teachBg.Find("Describe");
        colliderBG = teachRoot.transform.FindChild("ColliderBG");
        
        descTypeEffect = tranDesc.GetComponent<TypewriterEffect>();
        descLab = tranDesc.GetComponent<UILabel>();
        
        colliderBG.gameObject.SetActive(true);
        
        // 根据对话框类型显示/隐藏
        switch(dlgType)
        {
            case TalkDlgType.TalkDlg_Talk:
            {
                ShowCircleMask(false);
                ShowHand(false);
                ShowTargetEffect(false);
		    }
				break;
            case TalkDlgType.TalkDlg_ClickBtn:
            {
                ShowCircleMask(true);
                ShowHand(true);
                ShowTargetEffect(true);
                
				AdjustLayerAndDepth(trigger_obj);
            }
            break;
            default:
            break;
        }
    }

    void Start()
    {
        Vector2 screen = GetScreenWH();

        float hw = screen.x / 2;
        float hh = screen.y / 2;

        display.leftTop = new Vector2(-hw, hh);
        display.top = new Vector2(0, hh);
        display.rightTop = new Vector2(hw, hh);
        display.left = new Vector2(-hw, 0);
        display.center = new Vector2(0, 0);
        display.right = new Vector2(hw, 0);
        display.leftBottom = new Vector2(-hw, -hh);
        display.bottom = new Vector2(0, -hh);
        display.rightBottom = new Vector2(hw, -hh);
    }

    //定义一个枚举 用来记录传入的方向
    public enum Direction
    {
        DIR_FROM_LEFT,
        DIR_FROM_RIFHT,
        DIR_FROM_TOP,
        DIR_FROM_BOTTOM,
    }

    Direction dir = Direction.DIR_FROM_LEFT;

    void Update()
    {
        if (!isClose)
        {
            if (GameObject.Find("Camera").transform.Find("MapScene").gameObject.activeSelf == false)
            {
                isClose = true;
                //按钮关掉
                teachRoot.transform.Find("test").gameObject.SetActive(false);
            }
        }
        else
        {
            if (GameObject.Find("Camera").transform.Find("MapScene").gameObject.activeSelf == true)
            {
                isClose = false;
                //按钮打开
                teachRoot.transform.Find("test").gameObject.SetActive(true);
            }
        }
    }
    //获取屏幕的宽高
    Vector2 GetScreenWH()
    {
        UIRoot root = GameObject.FindObjectOfType<UIRoot>();
        if (root != null)
        {
            float s = (float)root.activeHeight / Screen.height;
            int height = Mathf.CeilToInt(Screen.height * s);
            int width = Mathf.CeilToInt(Screen.width * s);

            Debug.Log("height = " + height);
            Debug.Log("width = " + width);
            return new Vector2(width, height);
        }

        return new Vector2(0, 0);
    }

    //教学界面显示
    public void Show()
    {
        //方便测试
        MoveGhostToScreen(Direction.DIR_FROM_RIFHT, new Vector2(100, -200));
    }

    //播放幽灵等入场动画
    public void GhostEnter()
    {
        isGhostEntering = true;
        
        // 主角出场
        tweenPos.PlayForward();
        tweenPos.SetOnFinished(() =>
        {
            isGhostEntering = false;
            
            // Warning:必须放到设置文本前,因为TypewriterEffect的OnEnable中会重置文本
            // 放到设置文本后,会重置两次文本导致显示内存出错的bug
			teachBg.gameObject.SetActive(true);
            
            if (isFirstShowDescBGAnim) {
                if (contentIdx == 0) {
                    PlayDescPanelShowAnim();
                } else {
                    UpdateText(contentIdx);
                }
            } else {
                PlayDescPanelShowAnim();
            }
        });
        
        // 背景渐变
        teachPanelBg.GetComponent<TweenAlpha>().PlayForward();
    }
    
    // 播放对话背景出现动画
    public void PlayDescPanelShowAnim()
    {
        isDescBGAniming = true;
        
        // TODO:播放显示动画
        isDescBGAniming = false;
        UpdateText(contentIdx);
    }
    
    // 根据当前索引来更新显示文本(直接打印机显示文字效果)
    void UpdateText(int text_idx)
    {
        if (text_idx >= talkContentsList.Count)
            return;
        
        int lang_id = talkContentsList[text_idx];
//        string str = Lang.STR(lang_id);
		string str = "test";

        descLab.text = str;
        descTypeEffect.ResetToBeginning();
    }

    public void MoveGhostToScreen(Direction dir, Vector2 offestPosition)
    {        
        gameObject.SetActive(true);
        
        bool role_dir_right = true;
        
        //如果从左边进入 获取到与左中的偏移
        Vector2 offest = new Vector2();
        switch (dir)
        {
            case Direction.DIR_FROM_LEFT:
                {
                    offest.x = offestPosition.x - display.left.x;
                    offest.y = offestPosition.y - display.left.y;

                    //给form to 赋值
                    tweenPos.from = new Vector2(display.left.x - offest.x, offest.y);
                    tweenPos.to = new Vector2(offestPosition.x, offest.y);

                    //如果目标值的x方向值大于0
                    if (offest.x > 0)
                    {
                        role_dir_right = false;
                    }
                }
                break;

            case Direction.DIR_FROM_RIFHT:
                {
                    offest.x = display.right.x - offestPosition.x;
                    offest.y = offestPosition.y - display.right.y;

                    tweenPos.from = new Vector2(display.right.x + offest.x, offest.y);
                    tweenPos.to = new Vector2(offestPosition.x, offest.y);

                    if (offest.x < 0)
                    {
                        role_dir_right = false;
                    }
                }
                break;

            case Direction.DIR_FROM_TOP:
                {
                    offest.x = offestPosition.x - display.top.x;
                    offest.y = display.top.y - offestPosition.y;

                    tweenPos.from = new Vector2(offest.x, display.top.y + offest.y);
                    tweenPos.to = new Vector2(offest.x, offestPosition.y);

                    if (offest.x > 0)
                    {
                        role_dir_right = false;
                    }
                }
                break;

            case Direction.DIR_FROM_BOTTOM:
                {
                    offest.x = offestPosition.x - display.bottom.x;
                    offest.y = offestPosition.y - display.bottom.y;

                    tweenPos.from = new Vector2(offest.x, display.bottom.y - offest.y);
                    tweenPos.to = new Vector2(offest.x, offestPosition.y);

                    if (offest.x > 0)
                    {
                        role_dir_right = false;
                    }
                }
                break;
            default:
                break;
        }
        
        // 初始化主角朝向
        InitGhostDir(role_dir_right);

        GhostEnter();
    }
    
    // 设置幽灵的朝向
    void InitGhostDir(bool dir_right)
    {
        if (dir_right) {
            teachBg.localPosition = new Vector3(280, 40, 0);
            ghost.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        } else {
            teachBg.localPosition = new Vector3(-280, 40, 0);
            ghost.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
    }

    //教学系统离场动画
    void MoveGhostOutScreen()
    {
        tweenPos.PlayReverse();
        teachPanelBg.GetComponent<TweenAlpha>().PlayReverse();

        //将教学内容隐藏(在动画结束后隐藏)
        StartCoroutine(DelayToInvoke.DelayIt(() =>
        {
            gameObject.SetActive(false);
            
            teachBg.gameObject.SetActive(false);
            tranDesc.GetComponent<UILabel>().text = "";
            
            colliderBG.gameObject.SetActive(false);

			if (onFinishedCallback != null)
            {
				onFinishedCallback();
            }
        }, tweenPos.duration));
    }
    
    // 教学离场
    public void TeachOver(System.Action cb_finished = null)
    {
        onFinishedCallback = cb_finished;
        
        MoveGhostOutScreen();
    }
    
    public void SetContentsList(List<int> talk_content_list)
    {
        talkContentsList = talk_content_list;
        contentIdx = 0;
    }
    
    // 点击屏幕
    public void OnClicked()
    {
        if (dlgType == TalkDlgType.TalkDlg_Talk) 
        {
            // 1.如果正在播放打印机动画,停止动画。 否则显示下一组文字
            // 2.如果已经是最后一组,结束动画后触发回调 
            if (descTypeEffect.isActive) {
                descTypeEffect.Finish();
            } else {
                if (contentIdx + 1 >= talkContentsList.Count) {
                    MoveGhostOutScreen();
                } else {
                    contentIdx += 1;
                    
                    UpdateText(contentIdx);
                }
            }
        }
    }
    
    // 显示圆圈掩码
    public void ShowCircleMask(bool is_show)
    {
        // TODO:
        // 1.通过蒙版(圆圈)，让半透的背景显示一个圆窟窿
    }
    
    // 显示引导手指特效
    public void ShowHand(bool is_show)
    {
        // TODO:
    }
    
    // 显示教学目标周围的扩散效果
    public void ShowTargetEffect(bool is_show)
    {
        // TODO:
        // 1.获得按钮位置
        // 2.在colliderBG上添加一个水波特效
    }
    
    // 调整层级和深度,保证在最上方	
	public void AdjustLayerAndDepth(GameObject high_light_obj)
	{
        highLightObj = high_light_obj;
		highLightObjParent = highLightObj.gameObject.transform.parent.gameObject;
        
        highLightTmpParent = new GameObject();
        highLightTmpParent.name = "HighLightPanel";
        highLightTmpParent.AddComponent<UIPanel>();
        highLightTmpParent.SetActive(false);
        highLightTmpParent.transform.parent = highLightObjParent.transform;
        highLightTmpParent.transform.localPosition = new Vector3(0, 0, 0);
        highLightTmpParent.transform.localScale = new Vector3(1, 1, 1);
        
        //将新的父节点Panel的depth调至新手引导的depth上
        UIPanel highlighttmp = highLightTmpParent.GetComponent<UIPanel>();
        highlighttmp.depth = gameObject.GetComponent<UIPanel>().depth + 1;
        highlighttmp.sortingOrder = 3;
        
        //设置新父节点的层到新的层上
		if (highLightObj != null)
		{
            highLightObj.transform.SetParent(highLightTmpParent.transform);
            NGUITools.SetLayer(highLightObjParent, gameObject.layer);
		}

       	//相机重新渲染
        if (!highlighttmp.gameObject.activeSelf)
        {
            highlighttmp.gameObject.SetActive(true);
        }
        
//        // 拦截JellyButton点击事件
//        JellyButton jelly = highLightObj.GetComponent<JellyButton>();
//        if (jelly) {
//            clickDelegate = new EventDelegate();
//            clickDelegate.target = this;
//            clickDelegate.methodName = "OnClickedHighLightObj";
//            
//            if (!jelly.onClick.Contains(clickDelegate)) {
//                jelly.onClick.Add(clickDelegate);
//            }
//        }
	}
    
    // 点击高亮对象
    // 仅当点击模式有效
    public void OnClickedHighLightObj()
	{
		// 恢复层级
		ResetLayerAndDepth();

//        JellyButton btn = highLightObj.GetComponent<JellyButton>();
//        if (btn) {
//            btn.onClick.Remove(clickDelegate);
//        }

		MoveGhostOutScreen();
	}
	
	// 重置层级
	public void ResetLayerAndDepth()
	{
        if (highLightObj) {
             highLightObj.transform.SetParent(highLightObjParent.transform);

			Destroy(highLightTmpParent);
        }
	}
    
    // 更新教学对象的位置
    void UpdateTeachTargetPosition(GameObject target)
    {
        // 1.调整引导手指特效的位置
        // 2.调整引导光圈特效的位置
        // 3.调整蒙版光圈的位置
    }

}
