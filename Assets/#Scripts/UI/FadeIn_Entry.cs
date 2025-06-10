using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using FMOD.Studio;
/*------------------------------------------------------------------
* ファイル名：FadeOut_Logo
* 概要：名前入力画面のフェードインの実装
* 担当者：西林竜輝
* 作成日：5/16
-------------------------------------------------------------------*/
//更新履歴
/*
* 2022/5/16 西林竜輝　名前入力の画面の出現を実装
*/
//-----------------------------------------------------------------------------------

public class FadeIn_Entry : MonoBehaviour
{
    //変数宣言
    public Image MyImage;           //UnityのImageを設定するための変数
    private float FadeAlpha = 0;    //透明度を入れる変数
    private float step_time;        //秒数で画像を切り替える用の変数
    public bool isFade = false;
    GameObject LogoObject;
    FadeOut_Logo FadeOutScript;

    // スタートボタンを押したら実行される
    void Start()
    {
        //初期化
        LogoObject = GameObject.Find("AIMRACING2022");
        FadeOutScript = LogoObject.GetComponent<FadeOut_Logo>();
        step_time = 0.0f;
        MyImage.color = new Color(255, MyImage.color.b, MyImage.color.g, FadeAlpha);
    }

    // Update is called once per frame
    void Update()
    {
        if (FadeAlpha == 0)
        {
            isFade = FadeOutScript.GetIsFade;
        }
        
        //Fadeがfalseの場合、α値(透明度)を一定の速度で変える
        if (isFade)
        {
            step_time += Time.deltaTime*1.2f;

            if (step_time >= 1.0f)
            {
                //デルタタイムを1にする
                FadeAlpha = 1;

                //画像の色を白にしてα値をFadeAlphaで管理
                MyImage.color = new Color(255, MyImage.color.b, MyImage.color.g, FadeAlpha);

                //フラグをオフにする
                isFade = false;
                if (this.GetComponent<Entry>()!=null)
                {
                    this.GetComponent<Entry>().enabled = true;
                }
            }            
        }
    }
}
