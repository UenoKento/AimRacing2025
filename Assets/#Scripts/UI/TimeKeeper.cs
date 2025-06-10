using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;
using static UnityEngine.Rendering.DebugUI;

/// <summary>
/// 概要　：時間を計測し、テキストとして表示するプログラム
/// 担当者：22CU0235 諸星大和
/// 変更日：2024/05/17 作成
/// 　　　：2024/09/04 名称変更、セクタータイム用の処理を追加
/// </summary>
public class TimeKeeper : MonoBehaviour
{
    /// <summary>
    /// 時間保存用クラス
    /// </summary>
    [System.Serializable]
    private class SavedTimeInfo
    {
        public int minutes;
        public int seconds;
        public float milliSecond;
        public float totalMilliSecond;

        public SavedTimeInfo(int Minutes, int Seconds, float MilliSecond)
        {
            minutes = Minutes;
            seconds = Seconds;
            milliSecond = MilliSecond;

            totalMilliSecond = (minutes * 60 * 1000) + (seconds * 1000) + milliSecond;
        }
    }
    [SerializeField] List<SavedTimeInfo> _savedTimeInfo = new List<SavedTimeInfo>();

    [SerializeField]
    private int _minutes = 0;

    [SerializeField]
    private int _seconds = 0;

    [SerializeField]
    private float _milliSecond = 0;

    [SerializeField]
    private bool _isActive = false;

    [SerializeField]
    private TextMeshProUGUI _totalTimeText = null;

    void Start()
    {
        _isActive = false;
    }

    void FixedUpdate()
    {
        if (_isActive == true)
        {
            Count_upTimer();
        }
    }

    public int GetSavedTimeInfoCount()
    {
        return _savedTimeInfo.Count;
    }

    /// <summary>
    /// 時間を加算する関数
    /// </summary>
    void Count_upTimer()
    {
        _milliSecond += Time.deltaTime * 1000;

        if (_milliSecond >= 1000)
        {
            _seconds++;
            _milliSecond -= 1000;

            if (_seconds >= 60)
            {
                _minutes++;
                _seconds -= 60;
            }
        }

        _totalTimeText.text = _minutes.ToString("00")
            + ":" + _seconds.ToString("00")
            + "." + ((int)_milliSecond).ToString("000");
    }

    /// <summary>
    /// 稼働させるかを管理する関数
    /// </summary>
    /// <param name="flag">true=>Active : false=>InActive</param>
    public void ControlActiveFlag(bool flag)
    {
        _isActive = flag;
    }


    /// <summary>
    /// 呼び出したときに、時間を保存する関数
    /// </summary>
    public void SaveTime()
    {
        _savedTimeInfo.Add(new SavedTimeInfo(_minutes, _seconds, _milliSecond));
    }

    /// <summary>
    /// 保存した時間を取得する関数
    /// </summary>
    /// <returns></returns>
    public string RetrieveSavedTime(int value)
    {
        TextMeshProUGUI tmp = new TextMeshProUGUI();
        int minutes = 0;
        int seconds = 0;
        float milliSecond = 0f;
        float totalMilliSecond = 0f;

        // Sector1のとき
        if (value == 0)
        {
            tmp.text = "{" + _savedTimeInfo[value].minutes.ToString("00")
                + ":" + _savedTimeInfo[value].seconds.ToString("00")
                + "." + ((int)_savedTimeInfo[value].milliSecond).ToString("000") + "}";
        }
        // Sector2以降のとき
        if (value != 0)
        {
            totalMilliSecond = _savedTimeInfo[value].totalMilliSecond - _savedTimeInfo[value - 1].totalMilliSecond;
            seconds = (int)(totalMilliSecond / 1000);
            minutes = seconds / 60;
            seconds = seconds % 60;
            milliSecond = (totalMilliSecond - ((seconds * 1000) + (minutes * 60 * 1000)));

            tmp.text = "{" + minutes.ToString("00")
                + ":" + seconds.ToString("00")
                + "." + ((int)milliSecond).ToString("000") + "}";
        }

        return tmp.text;
    }

    /// <summary>
    /// 保存した時間を取得する関数
    /// </summary>
    /// <returns></returns>
    public string RetrieveSavedTimeToMeter(int value)
    {
        TextMeshProUGUI tmp = new TextMeshProUGUI();
        int minutes = 0;
        int seconds = 0;
        float milliSecond = 0f;
        float totalMilliSecond = 0f;

        // Sector1のとき
        if (value == 0)
        {
            tmp.text = _savedTimeInfo[value].minutes.ToString("00")
                + ":" + _savedTimeInfo[value].seconds.ToString("00")
                + "." + ((int)_savedTimeInfo[value].milliSecond).ToString("000");
        }
        // Sector2以降のとき
        if (value != 0)
        {
            totalMilliSecond = _savedTimeInfo[value].totalMilliSecond - _savedTimeInfo[value - 1].totalMilliSecond;
            seconds = (int)(totalMilliSecond / 1000);
            minutes = seconds / 60;
            seconds = seconds % 60;
            milliSecond = (totalMilliSecond - ((seconds * 1000) + (minutes * 60 * 1000)));

            tmp.text = minutes.ToString("00")
                + ":" + seconds.ToString("00")
                + "." + ((int)milliSecond).ToString("000");
        }

        return tmp.text;
    }
    /// <summary>
    /// ゴールした時間を取得する関数
    /// </summary>
    /// <returns></returns>
    public string RetrieveSavedTotalTime()
    {
        TextMeshProUGUI tmp = new TextMeshProUGUI();

        tmp.text = _minutes.ToString("00")
            + ":" + _seconds.ToString("00")
            + "." + ((int)_milliSecond).ToString("000");

        return tmp.text;
    }
}
