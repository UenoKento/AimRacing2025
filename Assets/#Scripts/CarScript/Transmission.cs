/**
 * @file    Transmission2024.cs
 * @brief   トランスミッション全般を管理する
 * @author  22CU0219 鈴木友也
 * @date    2024/05/17  作成
 *          2024/09/10  最終更新
 */
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Transmission
{
    public enum TransmissionType
    {
        Automatic,
        Manual
    }

    [SerializeField]
    TransmissionType m_type;

    [Range(-1, 7)]
    [SerializeField] // 現在のギア
    int m_currentGear;

    [SerializeField]　// ギア比のリスト
    List<float> m_gearRatioList = new List<float>();

    [SerializeField]
    float m_reverseGearRatio;
    [SerializeField]　// このスピード以下にならないとリバースに入らない
    float m_reverseChangeSpeed = 5f;
    [SerializeField]
    float m_banShiftDownRPM; // このRPM以上だとギアを下げられない
    [SerializeField]
    float m_shiftUpInterval = 1f;
    [SerializeField]
    float m_shiftDownInterval = 0.5f;
    [SerializeField]
    float m_gearChangingTime = 0.3f;
    // ギア切り替えの時間　
    [SerializeField,ShowInInspector]
    bool m_isGearChanging; 

    float m_lastShiftChangeTime = 0f;

    [Header("AT Settings")]
    [SerializeField]
    List<float> m_shiftUpSpeed = new List<float>();　// 単位[Km/h]
    [SerializeField,Range(0f,1f)]
    float m_speedInfluence; // スピードの影響度(0.5の場合、必要な速度の半分でシフトアップする)
    [SerializeField]
    float m_shiftUpEngineRPM;
    [SerializeField]
    float m_shiftDownEngineRPM;
    [SerializeField]
    float m_shiftDownEngineRPM_1st; // 1速に下げるためのRPM
    
    

    float m_engineRPM;
    bool m_isPullUp = false;


    #region プロパティ
    // AT/MT 切り替え
    public TransmissionType Type
    {
        get => m_type; 
        set => m_type = value;
    }

    // 現在のギア比
    public float CurrentGearRatio
    {
        get
        {
            if (m_currentGear > -1)
                return m_gearRatioList[m_currentGear];
            else
                return -m_reverseGearRatio;
        }
    }

    public int ActiveGear
    {
        get => m_currentGear;
    }

    public bool IsPullUp
    {
        set => m_isPullUp = value;
    }

    public bool IsGearChanging => m_isGearChanging;
    #endregion

    /// <summary>
    /// 初期化処理
    /// </summary>
    public void Initialize()
    {

    }

    public void FixedUpdate(float _engineRPM, float _speed)
    {
        m_engineRPM = _engineRPM;

        switch (m_type)
        {
            case TransmissionType.Automatic:
                AutomaticShift(_speed);
				break;

    //        case TransmissionType.Manual:
				//ManualShift();
				//break;
        }

		if (m_gearChangingTime <= Time.time - m_lastShiftChangeTime)
            m_isGearChanging = false;
        else
            m_isGearChanging = true;
    }

    void AutomaticShift(float _speed)
    {
        // NとRの時は処理しない
        if (m_currentGear <= 0)
			return;

        // シフトアップの条件
        // ・エンジンRPMがシフトアップRPMを超えている
        // ・現在の車速がスピード * 影響度の値を超えている
        // ・シフトアップ待機時間を過ぎている
        if (m_engineRPM >= m_shiftUpEngineRPM &&
            _speed > m_shiftUpSpeed[m_currentGear] * m_speedInfluence &&
            m_shiftUpInterval <= Time.time - m_lastShiftChangeTime)
        {
            ShiftUp();
        }

        // シフトダウンの条件
        // ・エンジンRPMがシフトダウンRPMを下回っている
        // ・1速ではない
        //   ※ギアが１の時に下げると0に入ってしまう
        // ・現在の車速がスピード * 影響度の値を超えている
        // ・シフトダウン待機時間を過ぎている
        else if (m_engineRPM < m_shiftDownEngineRPM && 
                m_currentGear != 1 &&
                _speed < m_shiftUpSpeed[m_currentGear - 1] * m_speedInfluence &&
                m_shiftDownInterval <= Time.time - m_lastShiftChangeTime)
        {
            // 2速の時は別のシフトダウンRPMを使う
            if(m_currentGear != 2)
            {
                ShiftDown();
            }
            else if(m_engineRPM < m_shiftDownEngineRPM_1st)
            {
                ShiftDown();
            }
        }
    }

    void ManualShift()
    {
		if (Input.GetButtonDown("ShiftUp"))
			ShiftUp();

		if (Input.GetButtonDown("ShiftDown"))
			ShiftDown();
	}

	public void ShiftUp()
    {
        // 停止中ならギアチェンジを無効化
        //if (m_isPullUp)
        //    return;

		// 経過時間(現在の時間 - 保持した時間)が待機時間を上回っていたら
		if (m_gearChangingTime <= Time.time - m_lastShiftChangeTime || m_currentGear <= 0)
		{
			m_currentGear++;
			// ギア切り替え時の時間を保持
			m_lastShiftChangeTime = Time.time;
		}

        // ギア比リストの要素数をオーバーフローしないように
        if (m_currentGear >= m_gearRatioList.Count)
            m_currentGear = m_gearRatioList.Count - 1;
    }

    public void ShiftDown()
    {
        // 停止中ならギアチェンジを無効化
        //if (m_isPullUp)
        //    return;

        if (m_banShiftDownRPM < m_engineRPM)
            return;


        // 経過時間(現在の時間 - 保持した時間)が待機時間を上回っていたら
        if (m_gearChangingTime <= Time.time - m_lastShiftChangeTime || m_currentGear <= 1)
		{
			m_currentGear--;
			// ギア切り替え時の時間を保持
			m_lastShiftChangeTime = Time.time;
		}

		// -1を下回らないように補正        (後でClampに変更)
		if (m_currentGear < -1)
            m_currentGear = -1;
    }

    public void ShiftTo(int _gear)
    {

    }
}
