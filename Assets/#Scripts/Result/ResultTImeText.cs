/*
 * ファイル名：ResultTimeText.cs
 * 概　　　要：リザルト画面でタイム表示
 * 作　成　者：20CU0213　小林大輝
 * 作　成　日：2022/05/26
 */

/*
 * 更新履歴：
 * 2022/05/26　[小林大輝]　テキストを受け取り表示
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SocialPlatforms.Impl;

public class ResultTImeText : MonoBehaviour
{
    public TextMeshProUGUI ResultTimeText;//最終タイムの文字列を管理する変数
    public Text ResultText;
    //private GoalJudgment goaljudge = null;//GolaJudgmentクラスオブジェクト
    //2024/2/5/坂本郁樹追記
    private string accessKey;//GASアクセスキー
    private string score = "";
    private string tempdata = "";
    private bool DataPosted = false;
    //private GoalJudgment Timeobj;

    // Start is called before the first frame update
    void Start()
    {
        //坂本郁樹追記
        DataPosted = false;
        
        accessKey = "https://script.google.com/macros/s/" + "AKfycbzD7D-O4dA0N4dbPjlI1eCgcJXfHlvMWdGjD9_KwFavuzOPEV9x0MKmHrVToQgvXEOZ" + "/exec";
        //-------------
        //goaljudge = new GoalJudgment();
        if(ResultTimeText)
        {
            //ResultTimeText.text = goaljudge.getResultTime();//リザルトタイムを取得
        }
        
        //ResultText.text = goaljudge.getResultTime();
        //score= goaljudge.getResultTime();
        //ResultText.text = score;
        //Timeobj = new GoalJudgment();
        //score = Timeobj.getResultTime().Replace(":", ".");
        //score = Timeobj.getResultTime();
        //score = score.Replace(".", "");


        //坂本郁樹追記
        //Debug.Log("コルーチン前のTime" + Timeobj.getTimeText());
        //StartCoroutine(PostData(ResultText.text));
        StartCoroutine(PostData(score));
        //-------------
    }

    //坂本郁樹追記------------------
    //スプレッドシートに送るデータの準備をする
    private IEnumerator PostData(string db_time)
    {
        if (DataPosted == false)
        {
            tempdata = db_time;
            DataPosted = true;
        }
        //readonly string timescore = db_time;
        //タイムの取得
        //名前の取得
        //string username = Entry.GetPlayerName();
        //if (username == "")
        //{
        //    username = "Guest";
        //}

        //Debug.Log("送信するデータの準備：time = " + tempdata + ",name = " + username);

        Debug.Log("データ送信開始・・・");
        var form = new WWWForm();
        form.AddField("time", tempdata);
        //form.AddField("time", "04:00:01");
        //form.AddField("name", username);

        //var request = UnityWebRequest.Post(accessKey, form);

        //yield return request.SendWebRequest();

        using (UnityWebRequest request = UnityWebRequest.Post(accessKey, form))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("データ送信成功");
                //Debug.Log("送信したデータ：time = " + tempdata + ",name = " + username);
            }
            else
            {
                Debug.Log("データ送信失敗");
            }
        }
    }
    //--------------------------
}