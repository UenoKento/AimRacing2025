/*------------------------------------------------------------------
* ファイル名：ATMTWindow.cs
* 概要：Sellect_transmission_typeオブジェクトの設定のためATMTWindow用のスクリプト
* 担当者：熊彦哲
* 作成日：08/17
-------------------------------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ATMTWindow : MonoBehaviour
{

	GameObject ATMTWindowImage;
	public GameObject ATMTUI;
	[SerializeField] float SetTime;
	private float GameTime;         //このスクリプトの起動時間

	public G29 g29;

	private void Start()
	{
		GameTime = 0;
		ATMTWindowImage = GameObject.Find("SelectWindows");
		ATMTWindowImage.SetActive(true);
		Debug.Log("SetActiveStart( ) ");
		//SoundManager.Instance.PlaySE(SoundManager.SE_Type.GuideATMT);
	}

	// Update is called once per frame
	void Update()
	{
		GameTime += Time.deltaTime;

		if (GameTime > SetTime)
		{
			ATMTUI.SetActive(true);
			Debug.Log("SetActive(true) ");
		}
	}
}
