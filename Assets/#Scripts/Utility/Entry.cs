//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using CUEngine.Pattern;
//public class Entry : StateBaseScriptMonoBehaviour
//{
//	public float time = 0;
//	Fade fade;
//	private void Start()
//	{
//		fade = GameObject.Find("Fade").GetComponent<Fade>();
//	}
//	private void Update()
//	{
//		time += Time.deltaTime;
//	}
//	//ノード専用
//	//フェード
//	public void StartFadeOut(float sec)
//	{
//		StartCoroutine(fade.FadeOut(sec));
//	}
//	public void StartFadeIn(float sec)
//	{
//		StartCoroutine(fade.FadeIn(sec));
//	}
//	//Imageフェード
//	public void StartImageFadeIn(Image image, float sec)
//	{
//		StartCoroutine(fade.ImageFadeIn(image, sec));
//	}
//	public void StartImageFadeOut(Image image, float sec)
//	{
//		StartCoroutine(fade.ImageFadeOut(image, sec));
//	}

//	public void imageEnd(Image endImage)
//	{
//		endImage.enabled = false;
//	}
//	public void imageStart(Image startImage)
//	{
//		startImage.enabled = true;
//	}
//	public void objectEnd(GameObject endObject)
//	{
//		endObject.SetActive(false);
//	}
//	public void objectStart(GameObject startObject)
//	{
//		startObject.SetActive(true);
//	}
//	//切り替え処理
//	public bool flowFade0(float sec)
//	{
//		if (time >= sec)
//		{
//			time = 0;
//			return true;
//		}
//		return false;
//	}
//	public void NextStage(GameObject EntryManager)
//	{
//		EntryManager.GetComponent<EntryManager>().nextStage = true;
//	}

//	public bool NameInputOK(GameObject inputName)
//	{
//		if (inputName.GetComponent<InputName>().compInputName)
//			return true;
//		else
//			return false;
//	}
//}
