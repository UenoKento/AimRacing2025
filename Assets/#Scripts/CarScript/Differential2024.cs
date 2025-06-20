﻿/**
 * @file    Differential2024.cs
 * @brief   差動ギア(左右駆動輪のトルク分配をする)
 * @author  22CU0219 鈴木友也
 * @date    2024/07/19  作成
 * 
 */

using UnityEngine;

[System.Serializable]
public class Differential2024
{
    enum DifferentialType
    { 
        None = 0,       // ディファレンシャル無し(トルク分配は常に左右一定)
        Open,           // オープンデフ(高回転している方に多く分配)
        Lock,           // デフロック(左右の回転を同じにする)
        LimitedSlip,    // LSD(左右の回転差を一定の割合で止める)
    }


    // 最終減速比
    [SerializeField]
    float m_differentialGearRatio;      // 最終減速比(ディファレンシャル比)
    [SerializeField]
    DifferentialType m_differentialType; // ディファレンシャルの種類
    [SerializeField, ShowInInspector]
    float m_lockRatio;                  // ロック率(100%だと左右の回転差が無くなる)
    [SerializeField,Range(0f,1f)]
    float m_customLockRatio_LSD;              // LSDのロック率
    float m_wheelAngularVelocity_Left;  // 駆動輪の角速度 左
    float m_wheelAngularVelocity_Right; // 駆動輪の角速度 右
    float m_wheelInertia;               // 駆動輪の慣性(左右の慣性は等しいこととする)

    /// <summary>
    /// 駆動輪に渡す駆動トルクを取得
    /// </summary>
    public float GetDriveTorque(in float _inputTorque,bool _isRight)
    {
        // プロペラシャフトのトルクと最終減速比を掛け合わせて2で割ったものを分配トルクとする
        float outputTorque = _inputTorque * m_differentialGearRatio * 0.5f;

        // 左右の角速度の差を2で割ったもの
        // 左駆動輪の角速度からこれを引いて、右駆動輪の角速度にこれを足せば左右の角速度は同じになる
        float halfDifferenceVelocity = (m_wheelAngularVelocity_Left - m_wheelAngularVelocity_Right) * 0.5f;

        // 角速度からトルクへの変換を行う
        /*--計算式------------------------------
         * 
         *  角加速度 = 角速度 / 単位時間
         *  トルク = 角加速度 * 慣性
         *  
         *--------------------------------------
         */

        // これをそのまま駆動トルクとして渡すと左右の差が無くなる
        float maxLockingTorque = halfDifferenceVelocity / Time.fixedDeltaTime * m_wheelInertia;

        // デフの種類によってロック率を変える
        switch (m_differentialType)
        {
            case DifferentialType.None:
                m_lockRatio = 0f;
                break;

            case DifferentialType.Open:
                halfDifferenceVelocity *= -1;
                m_lockRatio = 1f;
                break;

            case DifferentialType.Lock:
                m_lockRatio = 1f;
                break;

            case DifferentialType.LimitedSlip:
                m_lockRatio = m_customLockRatio_LSD;
                break;
        }

        // 左右の回転差を考慮して分配トルクを計算
        float leftTorque = outputTorque - (maxLockingTorque * m_lockRatio);
        float rightTorque = outputTorque + (maxLockingTorque * m_lockRatio);

        return (_isRight) ? rightTorque : leftTorque;
    }
   
    /// <summary>
    /// 現在の駆動輪のホイールの速度からシャフトの回転数を求める
    /// </summary>
    public float GetShaftVelocity(in float _driveWheelAngularVelocity,in float _wheelInertia , bool _isRight)
    {
        if (_isRight)
            m_wheelAngularVelocity_Right = _driveWheelAngularVelocity;
        else
            m_wheelAngularVelocity_Left = _driveWheelAngularVelocity;

        // 慣性は左右等しいとする
        m_wheelInertia = _wheelInertia;

        // シャフトの速度は左右駆動輪の角速度の平均 * 最終減速比
        return (m_wheelAngularVelocity_Right + m_wheelAngularVelocity_Left) * 0.5f * m_differentialGearRatio;
    }
}
