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
    float m_clutchStiffness;        // 剛性[rad/s]
    [SerializeField]
    float m_clutchCapacity;         // クラッチの最大トルクを決定する係数
    [SerializeField]
    float m_engineMaxTorque;        // エンジンで発生する最大トルク(カーブの最大値で問題無)
    [SerializeField,Range(0f,0.9f)]
    float m_clutchDamping;          // クラッチの振動を減衰させる
    [SerializeField]
    Vector2 m_lockRange;      // クラッチを完全に繋げるRPM

    [SerializeField, ShowInInspector]
    bool m_isGearChanging;
    [SerializeField, ShowInInspector]
    bool m_isPullUp;
    [SerializeField,ShowInInspector]
    float m_clutchLock;
    [SerializeField, ShowInInspector]
    float m_clutchTorque;
    [SerializeField, ShowInInspector]
    float m_clutchSlip;
    [SerializeField,ShowInInspector]
    float m_clutchAngularVelocity;
    float m_engineAngularVelocity;
    float m_gearRatio;
    float m_clutchInput;

    #region プロパティ
    public float ClutchTorque => m_clutchTorque;

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

    public void FixedUpdate(in float _shaftAngularVelocity, in float _engineAngularVelocity,in float _gearRatio)
    {
        m_clutchAngularVelocity = _shaftAngularVelocity;
        m_engineAngularVelocity = _engineAngularVelocity;
        m_gearRatio = _gearRatio;

        m_clutchTorque = CalcClutchTorque();
	}

    /// <summary>
    /// トルクの計算
    /// </summary>
    float CalcClutchTorque()
    {
        // オートクラッチ
        if(m_clutchAuto)
            ClutchLockAuto();

        // クラッチの入力が0.1f以上ある時はペダルの値で上書き
        // ※クラッチの入力は1.0(離)～0.0(踏)
        if(m_clutchInput <= 0.9f)
            m_clutchLock = m_clutchInput;

        // ギアチェンジ時のクラッチ切り
        // プルアップ時はクラッチ切り
        if (m_isGearChanging || m_isPullUp)
            m_clutchLock = 0f;

        // クラッチの滑り速度
        m_clutchSlip = m_engineAngularVelocity - m_clutchAngularVelocity;
        if (m_gearRatio == 0f)
            m_clutchSlip = 0f;


        float prevClutchTorque = m_clutchTorque;
        // クラッチトルク = クラッチの接続量 * クラッチの滑り量 * 剛性
        m_clutchTorque = m_clutchLock * m_clutchSlip * m_clutchStiffness; 
        // トルク量を制限
        m_clutchTorque = Mathf.Clamp(m_clutchTorque, -m_engineMaxTorque * m_clutchCapacity, m_engineMaxTorque * m_clutchCapacity);

        // クラッチの値の振動を減衰
        m_clutchTorque = m_clutchTorque + ((prevClutchTorque - m_clutchTorque) * m_clutchDamping);
        

        return m_clutchTorque;
    }

    /// <summary>
    /// オートクラッチの計算
    /// </summary>
    void ClutchLockAuto()
    {
        float engineRPM = m_engineAngularVelocity * CarPhysics.Rad2RPM;

        
        float lockRPM_Max = Mathf.Max(m_lockRange.x, m_lockRange.y);
        float lockRPM_Min = Mathf.Min(m_lockRange.x, m_lockRange.y);

        // 範囲内で現在のエンジンRPMによってロック率を0～1に正規化する
        m_clutchLock = Mathf.InverseLerp(lockRPM_Min,lockRPM_Max,engineRPM);

        // ニュートラル時は接続を切る
        if (m_gearRatio == 0f)
            m_clutchLock = 0f;
    }
}
