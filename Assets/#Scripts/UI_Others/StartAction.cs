
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StartAction : MonoBehaviour
{
    [SerializeField]
    private Image _image = null;
	[SerializeField]
	private Image _guideImage = null;

	[SerializeField]
    private TextMeshProUGUI _tmPro = null;

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

    bool m_SoundPlayFlag;


    void Start()
    {
        _image = _image.GetComponent<Image>();
        _tmPro = _tmPro.GetComponent<TextMeshProUGUI>();

        _vehicleController = _vehicleController.GetComponent<VehicleController2024>();
        _state = 0;

		SetActives(false);
    }

    void Update()
    {
		if (_state == 0)
        {
            _guideImage.enabled = true;

			if (_vehicleController.Accel >= 1f)
			{
                _guideImage.enabled = false;
                _isChecked = true;
				_state = 1;
                m_SoundPlayFlag  = true; // 3‚Ì‰¹‚ð–Â‚ç‚·‚½‚ß
			}
		}

        if (_isChecked)
        {
            SetActives(true);
            UpdateCountDown();
            UpdateCircle();
        }

        PlayCountDwonSound();
    }

    void UpdateCountDown()
    {
        if (_count > 0)
        {
            _deltaTimer += Time.deltaTime;

            if (_deltaTimer >= _interval)
            {
                _count--;
                _deltaTimer = 0f;
                m_SoundPlayFlag = true; 
			}
        }
        if (_count == 0)
        {
			m_startEvent.Invoke();
			_isChecked = false;
            SetActives(false);
            m_SoundPlayFlag = true;

		}

        _tmPro.text = _count.ToString();
    }

    void UpdateCircle()
    {
        float value = Mathf.InverseLerp(0, _interval, _deltaTimer);
        _image.fillAmount = value;
    }

    void SetActives(bool value)
    {
        _tmPro.gameObject.SetActive(value);
        _image.gameObject.SetActive(value);
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