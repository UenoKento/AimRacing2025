/*------------------------------------------------------------------
* �t�@�C�����FATMTWindow.cs
* �T�v�FSellect_transmission_type�I�u�W�F�N�g�̐ݒ�̂���ATMTWindow�p�̃X�N���v�g
* �S���ҁF�F�F�N
* �쐬���F08/17
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
	private float GameTime;         //���̃X�N���v�g�̋N������

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
