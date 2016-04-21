/**
 	基于NGUI的UITexture的纹理动画
	
	1.图片首尾相接的UITexture,可以播放UV纹理动画
	2.可以根据定制UV动画方向、速度
	3.图片属性: 【Texture Type】:Texture  【Wrap Mode】:Repeat, 图片属性必须基于纹理，不能是Sprite(2D 或 UI)
	
	Added by Teng. 		
 **/
using UnityEngine;
using System.Collections;
public class UVTextureMove : MonoBehaviour
{
	// 纹理对象
	public UITexture uiTexture;
	
	// 移动速度
	public float speedX = 0.1f;
	public float speedY = 0.1f;
	
	private float offset_x = 0.0f;
	private float offset_y = 0.0f;
	private float uv_w = 0;
	private float uv_h = 0;
	
	void Start()
	{
		if (uiTexture == null) {
			uiTexture = gameObject.GetComponent<UITexture>();
		}
		
		if (uiTexture == null) {
			Debug.LogError("UITexture not exist!");
		}
		
		uv_w = uiTexture.uvRect.width;
		uv_h = uiTexture.uvRect.height;
	}
	
	void Update ()
	{
		offset_x += Time.deltaTime * speedX;
		offset_y += Time.deltaTime * speedY;
		
		uiTexture.uvRect = new Rect(offset_x, offset_y, uv_w, uv_h);
	}
}