/**
 * @file    VehicleController2024.cs
 * @brief   パーツを管理し車の動力伝達をする
 * @author  22CU0219 鈴木友也
 * @date    2024/05/17  作成
 *          2024/09/19  最終更新
 */

using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody))]
public class VehicleController : MonoBehaviour
{
	[Space]
	[SerializeField]
	List<WheelController2024> m_wheelControllers;

    [SerializeField,ShowInInspector]
    float m_KPH;

    [Space]
    [SerializeField]
    Car_Engine m_engine;

    [Space]
    [SerializeField]
    Clutch m_clutch;

    [Space]
    [SerializeField]
    Transmission m_mission;

    [Space]
    [SerializeField]
    Differential m_differential;

    [Space]
    [SerializeField]
    Brake m_brake;

    [Space]
    [SerializeField]
    Steering m_steering;

    Rigidbody m_rigidbody;

	[SerializeField]
	UI_StarCountDown startCountDown;


    // Input
    float m_steerInput;
    float m_accelInput = 0f;
    float m_brakeInput = 0f;
    float m_clutchInput = 1f;

    #region プロパティ
    public Rigidbody Rigidbody => m_rigidbody;
    public Transmission Transmission => m_mission;  
    public Car_Engine Engine => m_engine;  
    public float KPH => Mathf.Clamp(m_KPH, 0f, float.PositiveInfinity);
    public float EngineRPM => m_engine.RPM;
    public int ActiveGear => m_mission.ActiveGear;
    public float Steering
    {
        get => m_steerInput;
        set => m_steerInput = value;
    }
    public float Accel
    {
       get => m_accelInput;
       set => m_accelInput = value;
    }
    public float Brake
    {
        get => m_brakeInput;
        set => m_brakeInput = value;
    }
    public float Clutch
    {
        get => m_clutchInput;
        set => m_clutchInput = value;
    }
	#endregion

	

	// Start is called before the first frame update
	void Start()
    {
        m_mission.Initialize();

        TryGetComponent<Rigidbody>(out m_rigidbody);
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        // カウント終わったら走り出す
        StartUnlock();

		// ギア切り替え
		m_mission.FixedUpdate(m_engine.RPM,m_KPH);

        // 駆動トルク(トランスミッション) = エンジントルク * 現在のギア比
        float driveTorque = m_clutch.ClutchTorque * m_mission.CurrentGearRatio;

        // プロペラシャフトの速度を計算
        float shaftVelocity = 0f;

        // 各ホイールの処理
       foreach (WheelController2024 wheel in m_wheelControllers)
        {
            // ステアリング角を設定
            wheel.SteerAngle = m_steering.CalcSteerAngle(m_steerInput, wheel.IsRightSide);

			// ブレーキトルクを計算
			float brakeTorque = m_brake.GetBrakeTorque(m_brakeInput, wheel.IsFrontSide);

            // ディファレンシャルでトルク分配
            float wheelDriveTorque = m_differential.GetDriveTorque(driveTorque, wheel.IsRightSide);

            wheel.UpdateTotalForce(wheelDriveTorque, brakeTorque);

            if (wheel.IsDrive)
            {
                // 駆動輪の速度からシャフトの速度を計算する
                shaftVelocity = m_differential.GetShaftVelocity(wheel.WheelAngularVelocity, wheel.Inertia, wheel.IsRightSide);
            }
        }

        // ギアの入力側の値を計算する
        float ClutchInputSide = shaftVelocity * m_mission.CurrentGearRatio;

        // ニュートラルだったとき
        if (m_mission.CurrentGearRatio == 0f)
        {
            // クラッチの出力
            ClutchInputSide = m_engine.RPM * CarPhysics.RPM2Rad;
        }

        // クラッチのインプット設定
        m_clutch.ClutchInput = m_clutchInput;
        m_clutch.GearChanging = m_mission.IsGearChanging;

        // クラッチトルクの更新
        m_clutch.FixedUpdate(ClutchInputSide, m_engine.RPM * CarPhysics.RPM2Rad, m_mission.CurrentGearRatio);

        // エンジンの回転数の更新
        m_engine.InjectionCut = m_mission.IsGearChanging;
        m_engine.FixedUpdate(m_accelInput, m_clutch.ClutchTorque);

        // 車速の計算
        m_KPH =  m_rigidbody.linearVelocity.magnitude * 3600f / 1000f;
        //Debug.Log("Yaw::" + m_rigidbody.angularVelocity.y);
    }

    public void ChangeMissionType()
    {
        if     (m_mission.Type == Transmission.TransmissionType.Manual)
                m_mission.Type =  Transmission.TransmissionType.Automatic;
        else if(m_mission.Type == Transmission.TransmissionType.Automatic)
		    	m_mission.Type =  Transmission.TransmissionType.Manual;
	}

    // カウント0になったら動かす
    public void StartUnlock()
    {
        if(startCountDown._count <= 0)
        {
            PullUp(false);
		}
	}

    public void PullUp(bool _active)
    {
        RigidbodyConstraints constraints;

        if(_active)
        {
            constraints = RigidbodyConstraints.FreezePositionX |
                          //RigidbodyConstraints.FreezeRotationY |
                          RigidbodyConstraints.FreezePositionZ;
		}
        else
        {
			constraints = RigidbodyConstraints.None;
		}

		m_mission.IsPullUp = _active;
		m_clutch.IsPullUp = _active;
		m_rigidbody.constraints = constraints;
	}

    void SwitchingTrueTraction()
    {
		foreach (WheelController2024 wheel in m_wheelControllers)
        {
            bool temp = wheel.TrueTraction;
            wheel.TrueTraction = !temp;
        }
	}

	// 土田　追記 06/10
    // GUIの表示
	private void OnGUI()
	{
		GUIStyle style = new GUIStyle(GUI.skin.label);
		style.fontSize = 24;    // 文字の大きさ
		style.normal.textColor = Color.white; // 文字の色
		int y = 10; // Y座標を加算して表示位置をずらす

		GUI.Label(new Rect(10, y, 500, 50), "Speed: " + m_KPH.ToString("F2") + " km/h", style);
		y += 30;
		GUI.Label(new Rect(10, y, 400, 50), "Engine RPM: " + m_engine.RPM.ToString("F2") + " rpm", style);
		y += 30;
		GUI.Label(new Rect(10, y, 400, 50), "Current Gear: " + m_mission.ActiveGear.ToString(), style);
		y += 30;
		GUI.Label(new Rect(10, y, 400, 50), "Clutch Torque: " + m_clutch.ClutchTorque.ToString("F2") + " Nm", style);
		y += 30;

		GUI.Label(new Rect(10, y, 400, 30), $"Accel Input: {m_accelInput:F2}", style);
		y += 30;
		GUI.Label(new Rect(10, y, 400, 30), $"Brake Input: {m_brakeInput:F2}", style);
		y += 30;
		GUI.Label(new Rect(10, y, 400, 30), $"Clutch Input: {m_clutchInput:F2}", style);
		y += 30;
		GUI.Label(new Rect(10, y, 400, 30), $"Steer Input: {m_steerInput:F2}", style);
		y += 30;

		// ステアリング角の表示
		foreach (WheelController2024 wheel in m_wheelControllers)
		{
			if (wheel.IsFrontSide)
			{
				string side = wheel.IsRightSide ? "Right" : "Left";
				GUI.Label(new Rect(10, y, 400, 30), $"{side} Front Steer Angle: {wheel.SteerAngle:F2} deg", style);
				y += 30;
			}
		}
	}

}
