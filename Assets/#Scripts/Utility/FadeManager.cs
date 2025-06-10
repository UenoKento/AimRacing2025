////===================================================
//// ファイル名	：FadeManager.cs
//// 概要			：Fadeの切り替え
//// 作成者		：藤森 悠輝
//// 作成日		：2019.06.06
////===================================================
//using UnityEngine;
//using CUEngine.Pattern;

//public class FadeManager : StateBaseScriptMonoBehaviour
//{
// 	//プロパティ────────────────────────────
//	[SerializeField] public float time = 0;
//	[SerializeField] Fade fade;
//	//─────────────────────────────────

//		//初期化──────────────────────────────
//	private void Start()
//	{
//		fade = GameObject.Find("Fade").GetComponent<Fade>();
//	}
//	//─────────────────────────────────

//	//更新処理─────────────────────────────
//	private void Update()
//	{
//		time += Time.deltaTime;
//	}
//	//─────────────────────────────────

//	//ノード専用メソッド
//	public void StartFadeOut(float sec)
//	{
//		DebugPrint.Log("AIM");

//		StartCoroutine(fade.FadeOut(sec));
//	}
//	public void StartFadeIn(float sec)
//	{
//		StartCoroutine(fade.FadeIn(sec));
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
//}
