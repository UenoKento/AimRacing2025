/*
 * ファイル名：PeopleAnimScript.cs
 * 内容      ：観客アニメーションコントローラー
 * 作成者    ：20CU0302安雪シン
 * 時間      ：2022/09/02
 * 更新      ：
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleAnimScript : MonoBehaviour
{
    //-----------各変数定義-------------
    private Animator visitorAnimator;
    //アニメーション切り替えるカウンター
    private int cont = 0;

    bool isClap = false;
    bool isCool = false;
    bool isCall = false;
    bool isChangeble = true;

    int waitTime = 0;

    //ランダム数
    int randamNum = 0;
    //----------------------------------
    // Start is called before the first frame update
    void Start()
    {
        visitorAnimator = GetComponent<Animator>();
        //Animatorの各パラメータズを初期化
        visitorAnimator.SetBool("isClap", false);
        visitorAnimator.SetBool("isCool", false);
        visitorAnimator.SetBool("isCall", false);
        visitorAnimator.SetBool("isChangeble", true);


        //始めた時、観客アニメーションをランダムにするため
        randamNum = Random.Range(0, 3);
        //アニメーション時間をランダムにする
        cont = Random.Range(1, 10) * 100;
        //アニメーションさせる前に待つ時間（複数のObject同時に同じアニメーションしないように）
        waitTime = Random.Range(0, 120);
    }

    // Update is called once per frame
    void Update()
    {
        if (waitTime > 0)
        {
            waitTime--;
            return;
        }
        //time_ += Time.deltaTime;
        selectAnimation(randamNum);
        
        if (!visitorAnimator.GetCurrentAnimatorStateInfo(0).IsTag("ClapMain") &&
            !visitorAnimator.GetCurrentAnimatorStateInfo(0).IsTag("CoolMain") &&
            !visitorAnimator.GetCurrentAnimatorStateInfo(0).IsTag("CallMain") &&
            cont == 0)
        {
            cont = Random.Range(2, 10) * 50;
            /*Debug.LogError(Time.time + "         " + cont);*/
        }

        if ((visitorAnimator.GetCurrentAnimatorStateInfo(0).IsTag("ClapMain") ||
            visitorAnimator.GetCurrentAnimatorStateInfo(0).IsTag("CoolMain") ||
            visitorAnimator.GetCurrentAnimatorStateInfo(0).IsTag("CallMain")) &&
            cont > 0)
        {
            isChangeble = false;
            cont--;
            if (cont == 0)
            {
                isChangeble = true;
                randamNum = Random.Range(0, 3);
            }
        }

        visitorAnimator.SetBool("goClap", isClap);
        visitorAnimator.SetBool("goCool", isCool);
        visitorAnimator.SetBool("goCall", isCall);
        visitorAnimator.SetBool("isChangeble", isChangeble);
    }

    //アニメーションを選択する関数
    void selectAnimation(int Num)
    {
        if (Num == 0)
        {
            isClap = true;
            isCool = false;
            isCall = false;
        }
        if (Num == 1)
        {
            isClap = false;
            isCool = true;
            isCall = false;
        }
        if (Num == 2)
        {
            isClap = false;
            isCool = false;
            isCall = true;
        }
    }
}
