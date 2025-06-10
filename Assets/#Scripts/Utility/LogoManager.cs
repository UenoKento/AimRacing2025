////===================================================
//// ファイル名	：LogoManager.cs
//// 概要			：Logoの管理
//// 作成者		：藤森 悠輝
//// 作成日		：2019.07.01
////===================================================
//using UnityEngine;
//using CUEngine.Pattern;
//using UnityEngine.UI;

//public class LogoManager : StateBaseScriptMonoBehaviour
//{
//	[SerializeField] public float time = 0;
//	[SerializeField] Fade fadeObject;
//	[SerializeField] Image image;

//    void Start()
//    {
//		//Screen.SetResolution(5760, 1080, false, 60);
//		Cursor.visible = false;
//		if (fadeObject == null) Debug.LogWarning("fadeObjectがnullです。");
//    }

//    void Update()
//    {
//		time += Time.deltaTime;
//    }

//	//ノード専用メソッド
//	//フェード呼び出し処理
//	public void StartFadeIn(float sec)
//	{
//		StartCoroutine(fadeObject.FadeOut(sec));
//	}
//	public void StartFadeOut(float sec)
//	{
//		StartCoroutine(fadeObject.FadeIn(sec));
//	}
//	//シーン切り替え
//	public void nextScene()
//	{
//		gameObject.GetComponent<NextScene>().nextScene();
//	}

//	public void SetActive(GameObject obj)
//	{
//		obj.SetActive(true);
//	}
//	public void SetUnActive(GameObject obj)
//	{
//		obj.SetActive(false);
//	}

//	//状態変移用メソッド
//	public bool flowFade0(float sec)
//	{
//		if (time >= sec)
//		{
//			time = 0;
//			return true;
//		}
//		return false;
//	}

//	public bool flow()
//	{
//		time = 0;
//		return true;
//	}

//	public void SetColorWhite(float alpha)
//	{
//		image.color = new Color(1, 1, 1, alpha);
//	}
//	public void SetColorBlack(float alpha)
//	{
//		image.color = new Color(0, 0, 0, alpha);
//	}
//}
