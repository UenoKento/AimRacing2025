using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/*
 * �T�v�@�@�F�t�F�[�h�E�C��/�t�F�[�h�E�A�E�g�𑀍삷��N���X
 * �쐬�ҁ@�F22CU0235 ������a
 * �ύX���@�F2024/06/25�@����ŕK�v�ȋ@�\�͊����B���GetButtonDown��p�ӂ���B
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
            // �V���[�g�J�b�g�L�[
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
    /// �t�F�[�h�̍X�V����
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
            // �摜�������Ȃ��Ȃ������\���ɂ���
            if (_image.color.a <= 0.05f)
            {
                color = new Color(1f, 1f, 1f, 0f);
                _image.color = color;
            }
        }
    }

    /// <summary>
    /// �A�j���[�V�������I�����������擾����
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