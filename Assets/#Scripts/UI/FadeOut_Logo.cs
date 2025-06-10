using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/*------------------------------------------------------------------
* ファイル名：FadeOut_Logo
* 概要：名前入力画面のフェードアウトの実装
* 担当者：西林竜輝
* 作成日：5/16
-------------------------------------------------------------------*/
//更新履歴
/*
* 2022/5/16 西林竜輝　Logoのフェードアウトを実装
*/

//-----------------------------------------------------------------------------------

public class FadeOut_Logo : MonoBehaviour
{
    //変数宣言
    public Image MyImage;           //UnityのImageを設定するための変数
    private float FadeAlpha = 0;    //透明度を入れる変数
    private bool isFadeIn = false;
	private bool isFadeOut = false;
	public bool GetIsFade { get{ return isFadeOut; } }

    //public GarageSound GS;

    //タイマー
    public float timeCnt = 0.0f;

    // スタートボタンを押したら実行される
    void Start()
    {
        //GS = GameObject.Find("GarageSound").GetComponent<GarageSound>();

        //初期化
        timeCnt = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        timeCnt += Time.deltaTime;

        //Fadeがfalseの場合、α値(透明度)を一定の速度で変える
        if (!isFadeIn)
        {
            //画像の色を白にしてα値をFadeAlphaで管理
            MyImage.color = new Color(255, 255, 255, FadeAlpha);

            //デルタタイムの2分の1をα値から引いていく
            FadeAlpha += Time.deltaTime;

            //α値が0以下になったら1に戻す。
            if (FadeAlpha >= 1)
            {
                isFadeIn = true;
                //GS.PlayWelcomAim();
            }
        }
        else
        {
            ////画像の色を白にしてα値をFadeAlphaで管理
            //MyImage.color = new Color(255, 255, 255, FadeAlpha);

            ////デルタタイムの2分の1をα値から引いていく
            //FadeAlpha -= Time.deltaTime / 1.5f;

            ////α値が0以下になったら1に戻す。
            //if (FadeAlpha <= 0)
            //{
            //	isFadeOut = true;
            //}
            if (timeCnt >= 2.5f)
            {
                //画像の色を白にしてα値をFadeAlphaで管理
                //MyImage.color = new Color(255, 255, 255, 0);
                MyImage.enabled = false;
                isFadeOut = true;
            }
        }
    }
}
