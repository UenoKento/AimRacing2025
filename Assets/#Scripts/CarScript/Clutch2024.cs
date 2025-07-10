/**
 * @file    Clutch2024.cs
 * @brief   エンジンとプロペラシャフトを繋ぐクラッチ
 * @author  22CU0219 鈴木友也
 * @date    2024/07/19  作成
 * 
 */

using UnityEngine;

[System.Serializable]
public class Clutch2024
{
    [SerializeField]
    bool m_clutchAuto = true;
    [SerializeField]
    float m_clutchStiffness;            // 剛性[rad/s]
    [SerializeField, ShowInInspector]
    float m_Calclate_ClutchMaxTorque;   //計算したクラッチの現在の許容トルク
    [SerializeField, ShowInInspector]
    float m_CrimpingForce;              //クラッチの圧着力
    [SerializeField, Range(0f, 0.9f)]
    float m_clutchDamping;              // クラッチの振動を減衰させる
    [SerializeField]
    Vector2 m_lockRange;                // クラッチを完全に繋げるRPM
    [SerializeField]
    float m_DesignTorque;               //設計トルク(最大エンジントルクの1.5~2.0倍)
    [SerializeField]
    float m_frictionCoef;               //クラッチの摩擦係数(0.55)
    [SerializeField]
    float m_ClutchOD;                   //クラッチの外径[m](0.35)
    [SerializeField]
    float m_ClutchID;                   //クラッチの内径[m](0.25)
    [SerializeField]
    float m_ClutchSurface;              //クラッチの摩擦面数(ツインクラッチなので4)

    [SerializeField, ShowInInspector]
    bool m_isGearChanging;
    [SerializeField, ShowInInspector]
    bool m_isPullUp;
    [SerializeField, ShowInInspector]
    float m_clutchLock;
    [SerializeField, ShowInInspector]
    float m_OutputTorque;
    [SerializeField, ShowInInspector]
    float m_clutchSlip;
    [SerializeField, ShowInInspector]
    float m_clutchAngularVelocity;
    [SerializeField, ShowInInspector]
    float m_engineAngularVelocity;
    [SerializeField, ShowInInspector]
    float m_clutchInput;

    float m_gearRatio;

    #region プロパティ
    public float ClutchTorque => m_OutputTorque;

    public bool IsPullUp
    {
        get => m_isPullUp;
        set => m_isPullUp = value;
    }

    public bool GearChanging
    {
        get => m_isGearChanging;
        set => m_isGearChanging = value;
    }

    public bool AutoClutch
    {
        get => m_clutchAuto;
        set => m_clutchAuto = value;
    }

    public float ClutchInput
    {
        set => m_clutchInput = value;
    }

    #endregion


    public void FixedUpdate(in float _shaftAngularVelocity, in float _engineAngularVelocity, in float _gearRatio)
    {
        m_clutchAngularVelocity = _shaftAngularVelocity;
        m_engineAngularVelocity = _engineAngularVelocity;
        m_gearRatio = _gearRatio;



        m_OutputTorque = CalcClutchTorque();
    }

    /// <summary>
    /// トルクの計算
    /// </summary>
    float CalcClutchTorque()
    {
        // オートクラッチ
        if (m_clutchAuto) ClutchLockAuto();


        //圧着力の計算
        float Rm = (m_ClutchOD + m_ClutchID) / 4;
        float DiskForce = m_frictionCoef * Rm * m_ClutchSurface;
        m_CrimpingForce = m_DesignTorque / DiskForce * m_clutchInput;

        //クラッチの最大許容トルクの計算
        m_Calclate_ClutchMaxTorque = DiskForce * m_CrimpingForce;

		// クラッチの滑り速度
		//if (m_clutchAngularVelocity < 0)
		//    m_clutchAngularVelocity = 0;

      //m_pclutchslip
		m_OutputTorque = m_clutchAngularVelocity * m_clutchInput;

        //float prevClutchTorque = m_OutputTorque;
        // クラッチトルク = クラッチの接続量(今はなし) * クラッチの滑り量 * 剛性
        //m_OutputTorque = m_clutchSlip;
        // トルクの制限する(されている場合はクラッチが滑っていると同義)
        m_OutputTorque = Mathf.Clamp(m_OutputTorque, -m_Calclate_ClutchMaxTorque, m_Calclate_ClutchMaxTorque);

        // クラッチの値の振動を減衰
        //m_clutchTorque += ((prevClutchTorque - m_clutchTorque) * m_clutchDamping);

        return m_OutputTorque;
    }

    /// <summary>
    /// オートクラッチの計算
    /// </summary>
    void ClutchLockAuto()
    {
        // ニュートラル時は接続を切る
        if (m_gearRatio == 0f)
        {
            m_clutchLock = 0f;
            return;
        }

        float engineRPM = m_engineAngularVelocity * CarPhysics.Rad2RPM;


        float lockRPM_Max = Mathf.Max(m_lockRange.x, m_lockRange.y);
        float lockRPM_Min = Mathf.Min(m_lockRange.x, m_lockRange.y);

        // 範囲内で現在のエンジンRPMによってロック率を0～1に正規化する
        m_clutchLock = Mathf.InverseLerp(lockRPM_Min, lockRPM_Max, engineRPM);

    }
}

// クラッチの入力が0.1f以上ある時はペダルの値で上書き
// ※クラッチの入力は1.0(離)～0.0(踏)
//多分いらないかも
//if (m_clutchInput <= 0.9f)
//    m_clutchLock = m_clutchInput;
//
// ギアチェンジ時のクラッチ切り
// プルアップ時はクラッチ切り
//if (m_isGearChanging || m_isPullUp)
//    m_clutchLock = 0f;
