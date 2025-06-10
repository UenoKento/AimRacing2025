// 車の位置(Pos)・回転(Rot)・時間(Time)を読み込んで車を動かすプログラム
// 作成者：23CU0325_土田修平
// 作成日：2025/06/06
// 追記日：2025/06/06 読み込まない問題の調査

using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadPosRotTime : MonoBehaviour
{
    private struct PosRotTime_Load
    {
        public Vector3 Load_pos;    // 位置
        public Vector3 Load_rot;   // 回転
        public float Load_time;    // 時間

        public PosRotTime_Load(Vector3 pos, Vector3 rot, float time)
        {
            Load_pos = pos;
            Load_rot = rot;
            Load_time = time;
        }
    }

    public string fileName = "/PosRotTimeLog.txt";
    private List<PosRotTime_Load> replayData = new List<PosRotTime_Load>();
    private int currentIndex = 0;       // 0から加算させる配列番号
    private float elapsedTime = 0f;     // 経過時間

    void Start()
    {
        LoadData();
    }

    void Update()
    {
        // データがないもしくは、最大数に達したら処理しない
        if (replayData.Count == 0 || currentIndex >= replayData.Count) return;

        // 
    }

    // 車の位置(Pos)・回転(Rot)・時間(Time)を読み込んで車を動かす関数
}
