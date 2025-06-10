using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/*
 * 概要　　：フェード・イン/フェード・アウトを操作するクラス
 * 作成者　：22CU0235 諸星大和
 * 変更日　：2024/06/25　現状で必要な機能は完成。後にGetButtonDownを用意する。
 */
public class FadeAnimation : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve _fadeCurve = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(1f, 0f));

    [SerializeField]
    private float _speed = 0f;
    [SerializeField]
    private float _correctionValue = 0f;

    [SerializeField]
    private bool _isLoop = false;

    [SerializeField]
    private bool _isStartAnimation = false;

    [SerializeField]
    private bool _isSkip = false;

    private Image _image;

    private float _curveRate = 0f;

    [SerializeField,ShowInInspector]
    private bool _end = false;

    [SerializeField]
    private KeyCode _keyCode = KeyCode.None;

    public bool StartAnimation
    {
        get
        {
            return _isStartAnimation;
        }
        set
        {
            _isStartAnimation = value;
        }
    }

    public float CurveRate
    {
        get
        {
            return _curveRate;
        }
        set
        {
            _curveRate = value;
        }
    }

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    private void Start()
    {
        Color color = new Color(1, 1, 1, _fadeCurve.Evaluate(_curveRate));
		_image.color = color;

        _curveRate = 0f;
        _isSkip = false;
    }

    private void Update()
    {
        if (_isStartAnimation == true)
        {
            // ショートカットキー
            if (Input.GetKeyDown(_keyCode))
            {
                _isSkip = true;
            }

            UpdateFade();
        }

        else if (_isStartAnimation == false)
        {
            _curveRate = 0f;
        }
    }

    /// <summary>
    /// フェードの更新処理
    /// </summary>
    private void UpdateFade()
    {
        Color color = new Color(1f, 1f, 1f, 1f);
        _image.color = color;

        if (_isSkip == true)
        {
            _curveRate = Mathf.Clamp(_curveRate + (_speed * Time.deltaTime) * _correctionValue, 0f, 1f);
        }
        else
        {
            _curveRate = Mathf.Clamp(_curveRate + _speed * Time.deltaTime, 0f, 1f);
        }

        color.a = _fadeCurve.Evaluate(_curveRate);
        _image.color = color;

        if (_curveRate >= 1f)
        {
            if (_isLoop == true)
            {
                _curveRate -= 1.0f;
            }
            if (_isLoop == false)
            {
                _isStartAnimation = false;
            }
        }
        else
        {
            // 画像が見えなくなったら非表示にする
            if (_image.color.a <= 0.05f)
            {
                color = new Color(1f, 1f, 1f, 0f);
                _image.color = color;
            }
        }
    }

    /// <summary>
    /// アニメーションが終了したかを取得する
    /// </summary>
    /// <returns></returns>
    public bool GetEndAnimationFlag()
    {
        if (_curveRate >= 1f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool GetEndAnimationFlagOnce()
    {
		if (_curveRate >= 1f)
		{
            _end = true;
		}

        return _end;
	}
}