using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/*------------------------------------------------------------------
* ファイル名：FadeImage
* 概要：Loop用Fade機能を実装の実装
* 担当者：熊彦哲
* 作成日：08/17
-------------------------------------------------------------------*/
//更新履歴
/*
* 2022/05/16 西林竜輝　Logoのフェードアウトを実装
* 2022/08/17 熊彦哲　Loop用Fade機能を実装
*/

//-----------------------------------------------------------------------------------

public class FadeImage : MonoBehaviour
{
    //変数宣言
    [SerializeField] Image MyImage;           //UnityのImageを設定するための変数
    [Header("フェードインスビート設定")]
    [SerializeField] float FadeInSpeed;
    [Header("フェードアウトスビート設定")]
    [SerializeField] float FadeOutSpeed;
    [Header("現在のアルファ値")]
    [SerializeField] private float FadeAlpha = 0;    //透明度を入れる変数
    [Header("アルファ値の設定")]
    public float Alpha = 0;
    private bool isFade = false;
    // スタートボタンを押したら実行される
    void Start()
    {
        FadeAlpha = Alpha;
    }

    // Update is called once per frame
    void Update()
    {
        //Fadeがfalseの場合、α値(透明度)を一定の速度で変える
        if ((!isFade) && (FadeInSpeed > 0)) 
        {
            //画像の色を白にしてα値をFadeAlphaで管理
            MyImage.color = new Color(255, 255, 255, FadeAlpha);

            //デルタタイムの2分の1をα値から引いていく
            FadeAlpha += Time.deltaTime / FadeInSpeed;

            //α値が0以下になったら1に戻す。
            if (FadeAlpha >= 1)
            {
                isFade = true;
            }
        }
        if ((isFade) && (FadeOutSpeed > 0))
        {
			//画像の色を白にしてα値をFadeAlphaで管理
			MyImage.color = new Color(255, 255, 255, FadeAlpha);

			//デルタタイムの2分の1をα値から引いていく
			FadeAlpha -= Time.deltaTime / FadeOutSpeed;

			//α値が0以下になったら1に戻す。
			if (FadeAlpha <= 0)
			{
                isFade = false;
			}
		}
    }
}
