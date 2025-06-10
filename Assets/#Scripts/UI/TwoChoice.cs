/**
 * @file	TwoChoice.cs
 * @brief	UIで二択の時に使える機能
 * @author	作成者：不明
 *			更新者：22CU0219 鈴木友也
 */

using UnityEngine;
using UnityEngine.UI;
using System;

public class TwoChoice : MonoBehaviour
{
	enum ChoiceType
	{ 
		Left = -1,
		None = 0,
		Right = 1,
	}

	//プロパティ───────────────────────────────────────
	[Header("leftアイコン")]
	[SerializeField] Transform leftIcon;
	[Header("rightアイコン")]
	[SerializeField] Transform rightIcon;
	[Header("最小拡大率")]
	[SerializeField] float minSize;
	[Header("最大拡大率")]
	[SerializeField] float maxSize;
	[Header("leftアイコンの説明文")]
	[SerializeField] GameObject leftIconsDescription;
	[Header("rightアイコンの説明文")]
	[SerializeField] GameObject rightIconsDescription;
	[Header("選択カラー")]
	[SerializeField] Color choicedColor;
	[Header("未選択カラー")]
	[SerializeField] Color unChoicedColor;
	[Header("デフォルトで選択している方")]
	Image leftImage;
	Image rightImage;

	[Header("開始までの待機時間")]
	[SerializeField] float waitTime = 1f;
	float activeTime;

	[Header("「アクセル踏んで」までの時間")]
	[SerializeField] float pushGasTime = 1f;
	float lastSelectTime;

	[SerializeField,ShowInInspector]
	ChoiceType nowChoiceNum = 0;
	ChoiceType prevChoiceNum = 0;

	

	public void Start()
	{
		nowChoiceNum = 0;       //-1：左 0:未選択　1：右
		prevChoiceNum = 0;
		leftImage = leftIcon.gameObject.GetComponent<Image>();
		rightImage = rightIcon.gameObject.GetComponent<Image>();
		unChoicedColor.a = 1.0f;

		leftIcon.transform.localScale = new Vector2(minSize, minSize);
		rightIcon.transform.localScale = new Vector2(minSize, minSize);
		leftImage.color = unChoicedColor;
		rightImage.color = unChoicedColor;
		leftIconsDescription.SetActive(false);
		rightIconsDescription.SetActive(false);

		// スタート時間保持
		activeTime = Time.time;
	}
	private void Update()
	{ 
		if (Time.time - activeTime > waitTime)
		{
			Choice();
		}
	}

	public void Choice()
	{
		// 選択番号によって画像を拡大
		switch (nowChoiceNum)
		{
			case ChoiceType.Left:
				leftIcon.localScale = new Vector2(maxSize, maxSize);
				leftIconsDescription.SetActive(true);
				rightIconsDescription.SetActive(false);
				rightIcon.localScale = new Vector2(minSize, minSize);

				leftImage.color = choicedColor;
				rightImage.color = unChoicedColor;
				break;
			case ChoiceType.Right:
				rightIcon.localScale = new Vector2(maxSize, maxSize);
				rightIconsDescription.SetActive(true);
				leftIconsDescription.SetActive(false);
				leftIcon.localScale = new Vector2(minSize, minSize);

				rightImage.color = choicedColor;
				leftImage.color = unChoicedColor;
				break;
		}

		// 選択対象が切り替わり時の処理
		if (prevChoiceNum != nowChoiceNum)
		{
			if (nowChoiceNum == ChoiceType.Left)
			{
				SoundManager.Instance.PlaySE(SoundManager.SE_Type.AT);
			}
			else if (nowChoiceNum == ChoiceType.Right)
			{
				SoundManager.Instance.PlaySE(SoundManager.SE_Type.MT);
			}
			SoundManager.Instance.PlaySE(SoundManager.SE_Type.CursorSE);

			prevChoiceNum = nowChoiceNum;
			lastSelectTime = Time.time;
		}
		

		if(lastSelectTime - Time.time > pushGasTime && nowChoiceNum != 0)
		{
			SoundManager.Instance.PlaySE(SoundManager.SE_Type.PushGasPedal);
			lastSelectTime = Time.time;
		}
	}

	//選択結果を返す
	//0...未選択
	//-1...1つ目が選択された
	//1...2つ目が選択された
	public int GetChoice()
	{
		return (int)nowChoiceNum;
	}

	public void SetRight()
	{
		nowChoiceNum = ChoiceType.Right;
	}
	public void SetLeft()
	{
		nowChoiceNum = ChoiceType.Left;
	}
}

