using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;
using static UnityEngine.Rendering.DebugUI;

/// <summary>
/// �T�v�@�F���Ԃ��v�����A�e�L�X�g�Ƃ��ĕ\������v���O����
/// �S���ҁF22CU0235 ������a
/// �ύX���F2024/05/17 �쐬
/// �@�@�@�F2024/09/04 ���̕ύX�A�Z�N�^�[�^�C���p�̏�����ǉ�
/// </summary>
public class TimeKeeper : MonoBehaviour
{
    /// <summary>
    /// ���ԕۑ��p�N���X
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
    /// ���Ԃ����Z����֐�
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
    /// �ғ������邩���Ǘ�����֐�
    /// </summary>
    /// <param name="flag">true=>Active : false=>InActive</param>
    public void ControlActiveFlag(bool flag)
    {
        _isActive = flag;
    }


    /// <summary>
    /// �Ăяo�����Ƃ��ɁA���Ԃ�ۑ�����֐�
    /// </summary>
    public void SaveTime()
    {
        _savedTimeInfo.Add(new SavedTimeInfo(_minutes, _seconds, _milliSecond));
    }

    /// <summary>
    /// �ۑ��������Ԃ��擾����֐�
    /// </summary>
    /// <returns></returns>
    public string RetrieveSavedTime(int value)
    {
        TextMeshProUGUI tmp = new TextMeshProUGUI();
        int minutes = 0;
        int seconds = 0;
        float milliSecond = 0f;
        float totalMilliSecond = 0f;

        // Sector1�̂Ƃ�
        if (value == 0)
        {
            tmp.text = "{" + _savedTimeInfo[value].minutes.ToString("00")
                + ":" + _savedTimeInfo[value].seconds.ToString("00")
                + "." + ((int)_savedTimeInfo[value].milliSecond).ToString("000") + "}";
        }
        // Sector2�ȍ~�̂Ƃ�
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
    /// �ۑ��������Ԃ��擾����֐�
    /// </summary>
    /// <returns></returns>
    public string RetrieveSavedTimeToMeter(int value)
    {
        TextMeshProUGUI tmp = new TextMeshProUGUI();
        int minutes = 0;
        int seconds = 0;
        float milliSecond = 0f;
        float totalMilliSecond = 0f;

        // Sector1�̂Ƃ�
        if (value == 0)
        {
            tmp.text = _savedTimeInfo[value].minutes.ToString("00")
                + ":" + _savedTimeInfo[value].seconds.ToString("00")
                + "." + ((int)_savedTimeInfo[value].milliSecond).ToString("000");
        }
        // Sector2�ȍ~�̂Ƃ�
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
    /// �S�[���������Ԃ��擾����֐�
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
