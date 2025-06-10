
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_StarCountDown : MonoBehaviour
{
	[SerializeField]
	private Image _guideImage = null;

	[SerializeField]
    private TextMeshProUGUI _tmp = null;

    [SerializeField]
    private int _count = 0;
    [SerializeField]
    private float _interval = 0f;
    private float _deltaTimer = 0f;

    [SerializeField]
    private bool _isChecked = false;

    [SerializeField]
    private VehicleController2024 _vehicleController = null;

    [SerializeField]
    private int _state = 0;

    [SerializeField]
    UnityEvent m_startEvent;

    bool m_SoundPlayFlag = false;

    void Start()
    {
        _vehicleController = _vehicleController.GetComponent<VehicleController2024>();
        _state = 0;

		SetActives(false);
    }

    void Update()
    {
        if (_state == 0)
        {
            _guideImage.enabled = true;

            if (_vehicleController.Accel >= 1f && _vehicleController.EngineRPM >= 6000f)
			{
                //_guideImage.enabled = false;
                _isChecked = true;
				_state = 1;
                m_SoundPlayFlag  = true; // 3‚Ì‰¹‚ð–Â‚ç‚·‚½‚ß
			}
		}

        if (_isChecked == true)
        {
            SetActives(true);

            UpdateCountDown();
        }

        PlayCountDwonSound();
    }

    void UpdateCountDown()
    {
        float value = 0;
        Color color = new Color(1f, 1f, 1f, 0f);
        _tmp.color = color;

        float scaleValue = 0;
        Vector3 vector = new Vector3(2f, 2f, 2f);
        _tmp.rectTransform.localScale = vector;

        if (_count > 0)
        {
            _deltaTimer += Time.deltaTime;

            if (_tmp.color.a <= 1)
            {
                value = Mathf.Lerp(0, 1, _deltaTimer);
                color.a += value;
                _tmp.color = color;

                if (_tmp.color.a >= 1)
                {
					value = Mathf.Lerp(1, 0, _deltaTimer);
					color.a -= value;
					_tmp.color = color;
				}
				if (_tmp.color.a <= 1)
				{
                    _tmp.color = new Color(1f, 1f, 1f, 1f);
				}
			}
			
			if (_tmp.rectTransform.localScale.x >= 0.8f)
            {
                scaleValue = Mathf.Lerp(2f, 0.8f, _deltaTimer);
                _tmp.rectTransform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);

                if (_tmp.rectTransform.localScale.x <= 0.8f)
                {
                    _tmp.rectTransform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                }
            }

            if (_deltaTimer >= _interval)
            {
                _count--;
                _deltaTimer = 0f;
                _tmp.rectTransform.localScale = new Vector3(3f, 3f, 3f);
                m_SoundPlayFlag = true; 
			}
        }
        if (_count == 0)
        {
			m_startEvent.Invoke();
            _guideImage.enabled = false;
            _isChecked = false;
            SetActives(false);
            m_SoundPlayFlag = true;

		}

        _tmp.text = _count.ToString();
    }

    void SetActives(bool value)
    {
        _tmp.gameObject.SetActive(value);
    }

    void PlayCountDwonSound()
    {
        if (!m_SoundPlayFlag)
            return;

		switch (_count)
		{
			case 3: SoundManager.Instance.PlaySE(SoundManager.SE_Type.Three); break;
			case 2: SoundManager.Instance.PlaySE(SoundManager.SE_Type.Two); break;
			case 1: SoundManager.Instance.PlaySE(SoundManager.SE_Type.One); break;
			case 0: SoundManager.Instance.PlaySE(SoundManager.SE_Type.GO); break;
		}

        m_SoundPlayFlag = false;
	}
}