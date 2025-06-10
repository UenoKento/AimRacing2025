using System.Collections.Generic;
using UnityEngine;

public class LaunchController : MonoBehaviour
{
	[SerializeField]
	VehicleController2024 m_vehicle;

	[SerializeField]
	List<TireSound> m_tiresound;

	[SerializeField]
	float m_launchRPM;

	[SerializeField]
	bool m_active = false;

	[SerializeField]
	UI_Meter m_meter;
	float m_flashingRPM;
	float m_overRevRPM;

	public bool Active 
	{
		get => m_active; 
		set => m_active = value;
	}

	private void Start()
	{
		// バックアップ
		m_flashingRPM = m_meter.FlashingRPM;
		m_overRevRPM = m_vehicle.Engine.OverRevRPM;

		// ローンチRPMを代入
		m_meter.FlashingRPM = m_launchRPM;
		m_vehicle.Engine.OverRevRPM = m_launchRPM;
	}

	private void FixedUpdate()
	{
		if(m_vehicle.KPH > 50f)
		{
			m_active = false;
			m_meter.FlashingRPM = m_flashingRPM;
			m_vehicle.Engine.OverRevRPM = m_overRevRPM;
		}

		
		foreach(TireSound Wheel in m_tiresound)
		{
			if (m_active)
			{
				Wheel.OverrideSlip = 4f * Mathf.InverseLerp(60f, 0f, m_vehicle.KPH);
				//Debug.Log("Slip +" + 4f * Mathf.InverseLerp(60f, 0f, m_vehicle.KPH));
			}
			else
				Wheel.OverrideSlip = 0f;
		}
	}
}
