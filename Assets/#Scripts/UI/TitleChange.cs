using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/*------------------------------------------------------------------
* ファイル名：TitleChange
* 概要：タイトル画面の切り替えとエフェクトの実装
* 担当者：西林竜輝
* 作成日：5/1
-------------------------------------------------------------------*/
//更新履歴
/*
* 2022/4/30　西林竜輝　画像切り替えの実装
* 2022/5/13　西林竜輝　画像切り替え時のエフェクトの実装
* 2022/5/19　西林竜輝　Unity側でシーン遷移先の名前を指定できるように設定
* 2022/5/30　西林竜輝　タイトル画面で先のシーンをロードできるよう変更
*/
//-----------------------------------------------------------------------------------

public class TitleChange : MonoBehaviour
{
    //変数宣言
    public Image MyImage;           //UnityのImageを設定するための変数
    public Sprite SecondImage;      //切り替える画像をUnityで設定するための変数
    private float step_time;        //秒数で画像を切り替える用の変数
    private float FadeAlpha = 0;    //透明度を入れる変数
    public string SceneName;        //ロードするシーン名
    public string UnloadScene;      //アンロードするシーン名
    private bool Unloaded = false;  //アンロードが終了しているかのフラグ
    private bool FadeIn;
    private bool FadeOut;

    private bool NextScene() { return Unloaded; }   //アンロードしたかどうかを確かめる関数

    // スタートボタンを押したら実行される
    void Start()
    {
        Cursor.visible = false;

        //初期化
        step_time = 0.0f;
       
        //シーンのロード
        if (SceneName != null)
        {
            StartCoroutine(LoadScene(SceneName)); 
        }
    }

    //シーンを先にロードする関数
    IEnumerator LoadScene(string name)
    {
        //読み込むシーン
        AsyncOperation LoadAsync=SceneManager.LoadSceneAsync(name);
        if (LoadAsync != null)
        {
            LoadAsync.allowSceneActivation = false;
        }
        //NextSceneがtrueになったらロードしたシーンに切り替える
        yield return new WaitUntil(NextScene);
        if (LoadAsync != null)
        {
            LoadAsync.allowSceneActivation = true;
        }
        yield return null;
    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(1);
        FadeIn = true;
    }
    IEnumerator Change()
    {
		yield return new WaitForSeconds(0.5f);
		MyImage.sprite = SecondImage;
		FadeIn = false;
		FadeOut = true;
		FadeAlpha = 0;
	}
	// Update is called once per frame
	void Update()
    {
        //α値(透明度)を一定の速度で変える
        //画像の色を白にしてα値をFadeAlphaで管理
        MyImage.color = new Color(255, 255, 255, FadeAlpha);

        if (!FadeIn)
        {
            //デルタタイムの2分の1をα値に足していく
            FadeAlpha += Time.deltaTime / 3.0f;
            //α値が0以下になったら1に戻す。
            if (FadeAlpha >= 1)
            {
                StartCoroutine(wait());
            }
        }
        else
        {
            //デルタタイムの2分の1をα値から引いていく
            FadeAlpha -= Time.deltaTime ;

			//α値が0以下になったら1に戻す。
			//フェードアウトしたら画像を切り替える
			if (!FadeOut)
            {
                if (FadeAlpha <= 0)
                {
                    
					StartCoroutine(Change());
                }
            }
            else if (FadeIn && FadeAlpha <= 0)
            {
                FadeAlpha = 10.0f;
            }
        }
        
        //ゲーム内の時間を代入
        step_time += Time.deltaTime;

        

        //二枚目の画像がフェードアウトしたらシーンを切り替える
        if (FadeAlpha >=10.0f)
        {
            Unloaded = true;
            SceneManager.UnloadSceneAsync(UnloadScene);
        }

    }
}
