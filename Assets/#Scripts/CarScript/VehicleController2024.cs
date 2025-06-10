/**
 * @file    VehicleController2024.cs
 * @brief   パーツを管理し車の動力伝達をする
 * @author  22CU0219 鈴木友也
 * @date    2024/05/17  作成
 *          2024/09/19  最終更新
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody))]
public class VehicleController2024 : MonoBehaviour
{
	[Space]
	[SerializeField]
	List<WheelController2024> m_wheelControllers;

    [SerializeField,ShowInInspector]
    float m_KPH;

    [Space]
    [SerializeField]
    Engine2024 m_engine;

    [Space]
    [SerializeField]
    Clutch2024 m_clutch;

    [Space]
    [SerializeField]
    Transmission2024 m_mission;

    [Space]
    [SerializeField]
    Differential2024 m_differential;

    [Space]
    [SerializeField]
    Brake m_brake;

    [Space]
    [SerializeField]
    Steering m_steering;

    Rigidbody m_rigidbody;

    // Input
    float m_steerInput;
    float m_accelInput = 0f;
    float m_brakeInput = 0f;
    float m_clutchInput = 1f;

    #region プロパティ
    public Rigidbody Rigidbody => m_rigidbody;
    public Transmission2024 Transmission => m_mission;  
    public Engine2024 Engine => m_engine;  
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
        m_engine.Initialize();
        m_mission.Initialize();

        TryGetComponent<Rigidbody>(out m_rigidbody);
    }


    // Update is called once per frame
    void FixedUpdate()
    {
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

        // トランスミッションのギア比を掛け合わせる
        shaftVelocity *= m_mission.CurrentGearRatio;

        // クラッチのインプット設定
        m_clutch.ClutchInput = m_clutchInput;
        m_clutch.GearChanging = m_mission.IsGearChanging;

        // クラッチトルクの更新
        m_clutch.FixedUpdate(shaftVelocity, m_engine.RPM * CarPhysics.RPM2Rad, m_mission.CurrentGearRatio);

        m_engine.InjectionCut = m_mission.IsGearChanging;
        // エンジンの回転数の更新
        m_engine.FixedUpdate(m_accelInput, m_clutch.ClutchTorque);


        // 車速の計算
        m_KPH =  m_rigidbody.linearVelocity.magnitude * 3600f / 1000f;
        //Debug.Log("Yaw::" + m_rigidbody.angularVelocity.y);
    }

    public void ChangeMissionType()
    {
        if (m_mission.Type == Transmission2024.TransmissionType.Manual)
            m_mission.Type = Transmission2024.TransmissionType.Automatic;
        else if(m_mission.Type == Transmission2024.TransmissionType.Automatic)
			m_mission.Type = Transmission2024.TransmissionType.Manual;
	}

    public void PullUp(bool _active)
    {
        RigidbodyConstraints constraints;

        if(_active)
        {
            constraints = RigidbodyConstraints.FreezePositionX |
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
}
