using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugShowLoad : MonoBehaviour
{

    [SerializeField]
    WheelController2024 m_FR;
	[SerializeField]
	WheelController2024 m_FL;
	[SerializeField]
	WheelController2024 m_RR;
	[SerializeField]
	WheelController2024 m_RL;

	private float m_Load_FR;
	private float m_Load_FL;
	private float m_Load_RR;
	private float m_Load_RL;

	private Vector2 m_centerOfMassPosition;

	[SerializeField]
	Image m_centerOfMassImage;
	RectTransform defaultPos;

	//[SerializeField]
	//List<WheelController2024> m_wheelControllers;

	void Start()
    {
		defaultPos = m_centerOfMassImage.GetComponent<RectTransform>();
	}

    void Update()
    {
		UpdateLoadValue();
		UpdateCenterOfMassPosition();

		if(m_centerOfMassPosition.x > 0)
		{
			m_centerOfMassPosition.x *= -1.0f;
		}
		if (m_centerOfMassPosition.y > 0)
		{
			m_centerOfMassPosition.y *= -1.0f;
		}

		m_centerOfMassImage.GetComponent<RectTransform>().anchoredPosition = new Vector2(m_centerOfMassPosition.x * 25.0f, m_centerOfMassPosition.y * 50.0f);
		//Debug.Log("yyyyyyyyyyyyyyyyyy" + m_centerOfMassPosition.y);
		//Debug.Log("xxxxxxxxxxxxxxxxxx" + m_centerOfMassPosition.x);

	}
	
	private void UpdateLoadValue()
	{
		m_Load_FR = m_FR.SuspensionLoad;
		m_Load_FL = m_FL.SuspensionLoad;
		m_Load_RR = m_RR.SuspensionLoad;
		m_Load_RL = m_RL.SuspensionLoad;
	}

	private void UpdateCenterOfMassPosition()
	{
		//m_centerOfMassPosition.y = ScaleValue(m_Load_FR, 2000.0f, 8000.0f, -1.0f, 1.0f) - ScaleValue(m_Load_RR, 2000.0f, 8000.0f, -1.0f, 1.0f);
		//m_centerOfMassPosition.x = ScaleValue(m_Load_FR, 2000.0f, 8000.0f, -1.0f, 1.0f) - ScaleValue(m_Load_FL, 2000.0f, 8000.0f, -1.0f, 1.0f);

		m_centerOfMassPosition.y = m_Load_FR - m_Load_RR;
		m_centerOfMassPosition.x = m_Load_FR - m_Load_FL;
		m_centerOfMassPosition.y = ScaleValue(m_centerOfMassPosition.y, 2000.0f, 8000.0f, -1.0f, 1.0f);
		m_centerOfMassPosition.x = ScaleValue(m_centerOfMassPosition.x, 2000.0f, 8000.0f, -1.0f, 1.0f);
	}

	private float ScaleValue(float value, float NowMin, float NowMax, float ScaleMin, float ScaleMax)
	{
		return ScaleMin + (value - NowMin) * (ScaleMax - ScaleMin) / (NowMax - NowMin);
	}
}
