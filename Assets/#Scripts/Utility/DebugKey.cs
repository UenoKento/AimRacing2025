/*------------------------------------------------------------------
* ファイル名：DebugKey
* 概要：DebugKeyの実装
* 担当者：西林竜輝
* 作成日：08/04
* 
-------------------------------------------------------------------*/
//更新履歴
/*
* 2022/08/04 西林竜輝　F1～F5のデバッグキーの設定
*/
//-----------------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugKey : MonoBehaviour
{
    //ポーズしているかどうかのフラグ
    bool Pause;

    // Start is called before the first frame update
    void Start()
    {
        
    }
   

    //Pauseを解除する変数
    private void NotPause()
    {
        Pause = false;
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        //Escape押すと強制終了してエクスプローラーを開く
        //のちにexeの場所を決めてexeの場所を開くようにする。
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //System.Diagnostics.Process.Start("explorer.exe", Application.dataPath);
            Application.Quit();
        }
        //F1を押すとタイトルに移動する。
        if (Input.GetKeyDown(KeyCode.F1))
        {
            SoundManager.Instance.StopBGM();
            GameManager.Instance.ReLoadingScene("Splash");
        }
        //F2を押すとインゲームに飛ぶ
        if (Input.GetKeyDown(KeyCode.F2))
        {
            SoundManager.Instance.StopBGM();
            GameManager.Instance.ReLoadingScene("Map");
        }
#if UNITY_EDITOR
        //F3を押すとエディタを一時停止する。
        if (Input.GetKeyDown(KeyCode.F3))
        {
            UnityEditor.EditorApplication.isPaused = true;
        }
#endif
        //F4を押すと一時停止する。
        if (Input.GetKeyDown(KeyCode.F4))
        {
            Pause = !Pause;
            if (Pause)
            {
                SoundManager.Instance.MuteBGM();
                Time.timeScale = 0;
            }
            if (!Pause)
            {
                SoundManager.Instance.ResumeBGM();
                Time.timeScale = 1;
            }
        }

        //F4を押すと
        if (Input.GetKeyDown(KeyCode.F4))
        {
            
        }

        //F5押すと
        if (Input.GetKeyDown(KeyCode.F5))
        {
            
        }
      


        // F6を押すとエディタをFPSを表示する。
        if (Input.GetKeyDown(KeyCode.F6))
		{
            GameManager.Instance.ShowFPS = !GameManager.Instance.ShowFPS;
		}
        
        // F7を押すとSIMVRの状態を表示する。
        if (Input.GetKeyDown(KeyCode.F7))
		{
            GameManager.Instance.ShowStateWIZMO = !GameManager.Instance.ShowStateWIZMO;
		}
        
        // F9を押すとマウスカーソルを表示/非表示
        if (Input.GetKeyDown(KeyCode.F9))
		{
            GameManager.Instance.ShowMouce = !GameManager.Instance.ShowMouce;
		}
        
        // F10を押すとデバイスを列挙表示する。
        if (Input.GetKeyDown(KeyCode.F10))
		{
            DebugDevice.Instance.ShowLogInGame = !DebugDevice.Instance.ShowLogInGame;
		}


	}
}