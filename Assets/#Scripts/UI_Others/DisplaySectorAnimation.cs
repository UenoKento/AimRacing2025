using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DisplaySectorAnimation : MonoBehaviour
{
    private Image _image = null;
    private TextMeshProUGUI _tmp = null;

    [SerializeField]
    private float _flashingCycle = 1;
    [SerializeField]
    private float _flashingTime = 1;

    [SerializeField]
    private Vector3 _toPos = Vector3.zero;
    [SerializeField]
    private Vector3 _toScale = new Vector3(1f, 1f, 1f);
    [SerializeField]
    private float _animationSpeed = 1;

    [SerializeField]
    private KeyCode _debugKeyCode = KeyCode.None;

    private float _timeRate = 0;

    private int animationState = 0;

    private bool _isAnimation = false;
    void Start()
    {
        _image = GetComponentInChildren<Image>();
        _tmp = GetComponentInChildren<TextMeshProUGUI>();


        Color color = new Color(1f, 1f, 1f, 0f);
        _image.color = color;
        _tmp.color = color;
    }

    public void AnimationStart()
    {
        _isAnimation = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(_debugKeyCode))
        {
            _isAnimation = true;
        }
        if (_isAnimation == true)
        {
            UpdateAnimation();
        }
    }

    private void UpdateAnimation()
    {
        Color newColor = new Color(1f, 1f, 1f, 1f);

        switch (animationState)
        {
            case 0:

                _timeRate += Time.deltaTime;

                float repeatValue = Mathf.Repeat(_timeRate, _flashingCycle);

                newColor.a = repeatValue >= _flashingCycle * 0.5f ? 1 : 0;

                _image.color = newColor;
                _tmp.color = newColor;

                if (_timeRate >= _flashingTime)
                {
                    newColor = new Color(1f, 1f, 1f, 1f);
                    _image.color = newColor;
                    _tmp.color = newColor;
                    _timeRate = 0;
                    animationState = 1;
                }

                break;
            case 1:
                _timeRate += _animationSpeed * Time.deltaTime;

                transform.localScale = Vector3.Lerp(transform.localScale, _toScale, _timeRate);
                transform.localPosition = Vector3.Lerp(transform.localPosition, _toPos, _timeRate);

                if (_timeRate >= 1)
                {
                    transform.localScale = _toScale;
                    transform.localPosition = _toPos;
                    _timeRate = 0;
                    animationState = -1;
                }
                break;

            case -1:
                _isAnimation = false;
                break;
        }
    }
}