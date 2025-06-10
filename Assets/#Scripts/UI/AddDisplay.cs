using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddDisplay : MonoBehaviour
{
    // 手書きで行う場合に変更する処理
    // MeterCamera からでも変更可
    [SerializeField]
    private bool OleDisplay = false;

    public int CountDis = 0;

    // 開始時に行う処理
    /*void Start()
    {
        // 手書き用
        if (OleDisplay == true)
        {
            // Display.displays[0] は主要なデフォルトのディスプレイで、常にオンです。ですから、インデックス 1 から始まります。
            // その他のディスプレイが使用可能かを確認し、それぞれをアクティブにします。
            if (Display.displays.Length > 1)
                Display.displays[1].Activate();
            if (Display.displays.Length > 2)
                Display.displays[2].Activate();
        }

        // 自動で確認してくれる用
        else
        {
            // 接続されているディスプレイを確認して表示できるようにする。
            for (int i = 1; i < Display.displays.Length; i++)
            {
                Display.displays[i].Activate();
                CountDis = i;
            }
            //モニターを数えた後に数分サイズを変えるkitamura
            Screen.SetResolution(Screen.currentResolution.width * CountDis, Screen.currentResolution.height, false);
            *//*            // 画面サイズの取得
                        Debug.Log("Screen currentResolution : " + Screen.currentResolution);*//*
        }
    }*/
}