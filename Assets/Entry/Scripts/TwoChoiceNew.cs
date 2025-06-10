using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TwoChoiceNew : MonoBehaviour
{
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
	bool isShortCourse;
	public G29 g29;
	private float TimeCounter;

	public static int nowChoiceNum = 1;
	int prevChoiceNum;

	private bool isLate;
	private float LateDeltaTime;
	private float LateDeltaSeconds;

	public void Start()
	{
		nowChoiceNum = 1;		//1：左　2：右
		prevChoiceNum = 0;
		leftImage = leftIcon.gameObject.GetComponent<Image>();
		//logiCon = GameObject.Find("TitleManager").GetComponent<Logitech_test>();
		rightImage = rightIcon.gameObject.GetComponent<Image>();
		unChoicedColor.a = 1.0f;

		isLate = false;
		LateDeltaTime = 0.0f;
		LateDeltaSeconds = 5f;
		TimeCounter = 0;

		leftIcon.transform.localScale = new Vector2(maxSize, maxSize);
		rightIcon.transform.localScale = new Vector2(minSize, minSize);
		leftImage.color = choicedColor;
		rightImage.color = unChoicedColor;
		leftIconsDescription.SetActive(true);
		rightIconsDescription.SetActive(false);
		g29 = GameObject.Find("G29").GetComponent<G29>();
	}
	private void Update()
	{
		TimeCounter++;
		if (TimeCounter >= 60) 
        {
			WaitChoice();
		}
	}

	public void WaitChoice()
	{
		if (g29.rec.lX < -10000/*Input.GetAxis("Horizontal") < -0.1f*/)    //    ←ハンドルとコントローラー追加
		{
			isLate = false;
			nowChoiceNum = 1;
		}
		else if (g29.rec.lX > 10000/*Input.GetAxis("Horizontal") > 0.1f*/)
		{
			isLate = false;
			nowChoiceNum = 2;
		}
		
		if (Input.GetKeyDown(KeyCode.LeftArrow))        //	←ハンドルとコントローラー追加
        {
			isLate = false;
			nowChoiceNum = 1;
		}
		else if (Input.GetKeyDown(KeyCode.RightArrow))      //	←ハンドルとコントローラー追加
        {
			isLate = false;
			nowChoiceNum = 2;
		}
		else
        {
			isLate = true;
        }

		switch (nowChoiceNum)
		{
			case 1:
				leftIcon.localScale = new Vector2(maxSize, maxSize);
				leftIconsDescription.SetActive(true);
				rightIconsDescription.SetActive(false);
				rightIcon.localScale = new Vector2(minSize, minSize);

				leftImage.color = choicedColor;
				rightImage.color = unChoicedColor;
				break;
			case 2:
				rightIcon.localScale = new Vector2(maxSize, maxSize);
				rightIconsDescription.SetActive(true);
				leftIconsDescription.SetActive(false);
				leftIcon.localScale = new Vector2(minSize, minSize);

				rightImage.color = choicedColor;
				leftImage.color = unChoicedColor;
				break;
		}
		if (prevChoiceNum != nowChoiceNum)
		{
			if (nowChoiceNum == 1)
			{
				SoundManager.Instance.PlaySE(SoundManager.SE_Type.AT);
			}
			else if(nowChoiceNum == 2)
            {
				SoundManager.Instance.PlaySE(SoundManager.SE_Type.MT);
			}
			SoundManager.Instance.PlaySE(SoundManager.SE_Type.CursorSE);

			prevChoiceNum = nowChoiceNum;
		}

		if (isLate == true) 
        {
			LateDeltaTime += Time.deltaTime;

			if(LateDeltaTime>=LateDeltaSeconds)
            {
				SoundManager.Instance.PlaySE(SoundManager.SE_Type.PushGasPedal);
				LateDeltaTime = 0;
            }
        }
		else if(isLate==false)
        {
			LateDeltaTime = 0;
        }
	}
	
	//選択結果を返す
	//0...未選択
	//1...1つ目が選択された
	//2...2つ目が選択された
	public int GetChoice()
	{
		if (Input.GetAxisRaw("L_R_Trigger") >= 0.6f || Input.GetAxis("Vertical") >= 0.2f || Input.GetKeyDown(KeyCode.W) || -Input.GetAxis("Accel") >= 0.2f|| ((g29.rec.lY / (float)-Int16.MaxValue + 1.0f) * 0.5f) >= 0.6f)
		{
			
			return nowChoiceNum;
		}
		else
			return 0;
	}
	public int GetCourse()
    {
		return nowChoiceNum;
	}
}

