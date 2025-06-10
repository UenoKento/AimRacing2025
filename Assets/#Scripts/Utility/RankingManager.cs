////===================================================
//// ファイル名	：RankingManager.cs
//// 概要			：Rankingの管理
//// 作成者		：藤森 悠輝
//// 作成日		：2019.08.23
////===================================================
//using UnityEngine;
//using UnityEngine.UI;
//using CUEngine.Pattern;
//using UnityEngine.SceneManagement;
//using System.IO;

//public class RankingManager : StateBaseScriptMonoBehaviour
//{
//	[SerializeField] public float time = 0;
//	[SerializeField] public float fadeTime = 0;
//	public float timeFadeMax = 6.0f;
//	public float timeFade = 1.0f;

//	[SerializeField] Fade fadeObject;
//	[SerializeField] Fade fadeObject_test;

//	[SerializeField] GameObject resultBgm;
//	[SerializeField] GameObject thanks;

//	public Image fadeImage;
//	public Image fadeImage_test;
//	public float alpha;

//	// シーン読み込み用
//	public string nextSceneName;
//	string nextPath = "/Log/rankNext.txt";

//	public bool isInputOn = false;
//	public bool corouIsOnce = false;

//	//private Logitech_test logiCon;
//	// Start is called before the first frame update
//	void Start()
//	{
//		if (fadeObject == null) Debug.LogWarning("fadeObjectがnullです。");
//		//logiCon = GameObject.Find("VehicleManager").GetComponent<Logitech_test>();

//		nextPath = Application.dataPath + nextPath;
//	}

//	// Update is called once per frame
//	void Update()
//	{
//		DownKeyCheck();

//		if(fadeObject_test.FadeAlfaReturn() < 0.02f && !isInputOn)
//		{
//			isInputOn = true;
//			return;
//		}

//		if(!isInputOn)
//		{
//			return;
//		}

//		if (fadeTime >= timeFadeMax || nextInput())
//		{
//			fadeTime = timeFadeMax;
//			//StartImageFadeIn(fadeImage,timeFade);

//			if (!corouIsOnce)
//			{
//				StartCoroutine(fadeObject_test.FadeIn());
//				corouIsOnce = true;
//			}
//			if (fadeObject_test.FadeAlfaReturn() > 0.95f)
//			{
//				if(File.Exists(nextPath))
//				{
//					DebugPrint.Log("パスから読み込み");
//					SceneManager.LoadScene(File.ReadAllText(nextPath));
//				}
//				else
//				{
//					SceneManager.LoadScene(nextSceneName);
//				}
//			}
//		}
//		else
//		{
//			time += Time.deltaTime;
//			fadeTime += Time.deltaTime;
//		}
//	}

//	public void DownKeyCheck()
//	{
//		if (Input.GetKeyDown(KeyCode.Space))
//		{
//			ChangeScene();
//		}
//	}

//	//ノード専用メソッド
//	//フェード呼び出し処理
//	public void StartFadeOut(float sec)
//	{
//		StartCoroutine(fadeObject.FadeOut(sec));
//	}
//	public void StartFadeIn(float sec)
//	{
//		StartCoroutine(fadeObject.FadeIn(sec));
//	}
//	//Imageフェード
//	public void StartImageFadeIn(Image image, float sec)
//	{
//		StartCoroutine(fadeObject.ImageFadeIn(image, sec));
//	}
//	public void StartImageFadeOut(Image image, float sec)
//	{
//		StartCoroutine(fadeObject.ImageFadeOut(image, sec));
//	}

//	//シーン切り替え
//	public void nextScene()
//	{
//		if (SceneManager.GetActiveScene().name == "Ranking")
//		{
//			if (fadeObject.changeColor)
//			{
//				fadeObject.changeColor = false;
//				resultBgm.SetActive(true);
//				//thanks.SetActive(true);
//			}
//		}
//		else
//		{
//			gameObject.GetComponent<NextScene>()?.nextScene();
//			time = 0;
//		}
//	}

//	public void ChangeScene()
//	{
//		if (SceneManager.GetActiveScene().name == "RankingMonitor")
//		{
//			gameObject.GetComponent<ChangeScene>().changeScene();
//			time = 0;
//		}
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
//	//エンター実行処理
//	public bool nextEnter()
//	{
//		if (Input.GetKeyDown(KeyCode.Return))
//		{
//			time = 0;
//			return true;
//		}
//		else
//			return false;
//	}
//	//アクセル実行処理
//	public bool nextAccel()
//	{
//		//if (Input.GetAxisRaw("R_Trigger") > 0.7 || Input.GetAxisRaw("Vertical") > 0.7 || AIM.InputHandleController.GetHandleAccelCheck() >= 0.7f)
//		//{
//		//	time = 0;
//		//	return true;
//		//}
//		//else
		
//			return false;
//	}

//	public bool nextInput()
//	{
//		////Debug.Log("入力" + Input.GetAxisRaw("R_Trigger") + "  :  " + Input.GetAxisRaw("Vertical") + "  :  " + AIM.InputHandleController.GetHandleAccelCheck());

//		if (Input.GetAxisRaw("R_Trigger") > 0.7 || Input.GetAxisRaw("Vertical") > 0.7 || AIM.InputHandleController.GetHandleAccelCheck() >= 0.7f || Input.GetKeyDown(KeyCode.Return))
//		{
//			return true;
//		}
//		else
//		{
//			return false;
//		}
//	}
//	//========================================================================================================
//	// Fadeのα値確認
//	public float FadeAlfaCheck()
//	{
//		return fadeObject.FadeAlfaReturn();
//	}
//}
