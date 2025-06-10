using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 概要　：セクターを通過したときにアクションを起こすプログラム
/// 担当者：22CU0235 諸星大和
/// 変更日：2024/05/17 作成
/// 　　　：2024/09/04 名称変更
/// </summary>
public class Line_Sector : MonoBehaviour
{
    private BoxCollider _boxCollider = null;

    [SerializeField]
    private TimeKeeper _timeKeeper = null;

    [SerializeField]
    private TextMeshProUGUI _tmp = null;

    [SerializeField]
    private int _sectorCount = 0;

    [SerializeField]
    private bool _isChecked = false;

    [SerializeField]
    private KeyCode _debugKeyCode = KeyCode.None;

    [SerializeField]
    private UnityEvent _unityEvent = new UnityEvent();

    private void Start()
    {
        _boxCollider = GetComponent<BoxCollider>();

        _isChecked = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(_debugKeyCode))
        {
            RegisterTime();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        RegisterTime();
    }

    /// <summary>
    /// 時間を登録する関数
    /// </summary>
    private void RegisterTime()
    {
        if (_isChecked == false)
        {
            _timeKeeper.SaveTime();
            _tmp.text = _timeKeeper.RetrieveSavedTime(_sectorCount);

            AnimationStart();

            SoundManager.Instance.PlaySE(SoundManager.SE_Type.LapSignal);

            _isChecked = true;
        }
    }

    public void AnimationStart()
    {
        _unityEvent.Invoke();
    }
}
