/*------------------------------------------------------------------
* ファイル名：CurveSign
* 概要：転向マークのエフェクトを表示
* 担当者：熊彦哲
* 作成日：07/14
-------------------------------------------------------------------*/
//更新履歴
/*
* 2022/07/14  熊彦哲　　転向マークのエフェクトの実装
* 2022/07/15  熊彦哲　　fadeエフェクト削除
* 2022/08/20　松下和樹　Imageに追加処理＆強度振り分け
* 2024/08/06  22cu0219 鈴木友也　変数名変更、Rangeの追加
*/

//-----------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CurveSign : MonoBehaviour
{
    //変数宣言--------------------------------------------------------
    [SerializeField] public bool isCollision = false;    //ゴールの判定処理
    public Image image;           //UnityのImageを設定するための変数
    private float FadeAlpha = 0;    //透明度を入れる変数
    private int type;            //1:TurnLeft 2:TurntRight 3:SignGoal

    // 松下追加-------------------------------------------------------
    private GameObject signImage;

    [Range(1,3)]
    [SerializeField] private int curveIntensity;    // カーブの強さ 1:弱 2:凡 3:強
    //----------------------------------------------------------------

    //関数処理--------------------------------------------------------
    //プレイヤーがゴールを通った際にフラグをtrueにする
    private void OnTriggerEnter(Collider collider)
    {
        //当たったColliderのタグがPlayerならフラグをtrueに
        if (this.tag == "SignTurnLeft")
        {
            type = 1;
            FadeAlpha = 1.0f;
        }
        else if (this.tag == "SignTurnRight")
        {
            type = 2;
            FadeAlpha = 1.0f;
        }
        else if (this.tag == "SignGoal")
        {
            type = 3;
            FadeAlpha = 0.0f;
        }

        if (collider.tag == "Player")
        {
            isCollision = true;

            if (type == 1)
            {
                if (curveIntensity == 1) SoundManager.Instance.PlaySE(SoundManager.SE_Type.Easy_L);
                else if (curveIntensity == 2) SoundManager.Instance.PlaySE(SoundManager.SE_Type.Turn_L);
                else if (curveIntensity == 3) SoundManager.Instance.PlaySE(SoundManager.SE_Type.Acute_L);
            }
            else if (type == 2)
            {
                if (curveIntensity == 1) SoundManager.Instance.PlaySE(SoundManager.SE_Type.Easy_R);
                else if (curveIntensity == 2) SoundManager.Instance.PlaySE(SoundManager.SE_Type.Turn_R);
                else if (curveIntensity == 3) SoundManager.Instance.PlaySE(SoundManager.SE_Type.Acute_R);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // 松下追加--------------------------------------------
        switch (curveIntensity)
        {
            case 1:
                signImage = GameObject.Find("SignBlue");
                break;

            case 2:
                signImage = GameObject.Find("SignYellow");
                break;

            case 3:
                signImage = GameObject.Find("SignRed");
                break;

            default:
                break;
        }

        // NULLチェックを行う
        // エラーを吐いていたところ
        // 2023/07/13
        if (signImage != null)
        {
            image = signImage.GetComponent<Image>();
        }
        //-----------------------------------------------------

        //初期化
        image.color = new Color(255, 255, 255, FadeAlpha);
    }

    // Update is called once per frame
    void Update()
    {
        if(isCollision && this.gameObject)
        {
           CollisionFlag();
        }
    }
    private void CollisionFlag()
    {
        if (type == 3)        //Goal判定
        {
            //画像はturnrightの場合、表示されない時がturnleftに戻ります
            if (image.gameObject.transform.eulerAngles.y == 180)
            {
                Vector3 newAngle = image.gameObject.transform.eulerAngles;
                newAngle.y = 180;
                image.gameObject.transform.eulerAngles -= newAngle;
            }
            image.color = new Color(255, 255, 255, FadeAlpha);
            Destroy(this.gameObject);
        }
        else
        {
            if (type == 2)
            {
                // 左右切り替え
                Vector3 newAngle = image.gameObject.transform.eulerAngles;
                newAngle.y = 180;
                image.gameObject.transform.eulerAngles += newAngle;
            }

            //画像の色を白にしてα値をFadeAlphaで管理
            image.color = new Color(255, 255, 255, FadeAlpha);

            Destroy(this.gameObject);
        }
    }
}
