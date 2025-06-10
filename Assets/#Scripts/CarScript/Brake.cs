using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Brake
{
    [SerializeField,Range(0f,1f)]
    float m_frontBrakeBias;
    [SerializeField]
    float m_maxBrakeTorque;
    [SerializeField]
    bool m_onHandBrake;

    public float GetBrakeTorque(in float _brakeInput, bool _isFront)
    {
        float totalBrakeTorque = m_maxBrakeTorque * _brakeInput;
        float frontBrakeTorque;
        float rearBrakeTorque;

        // ハンドブレーキをオンにすると後輪に全ブレーキ力がかかる
        if(m_onHandBrake)
        {
            frontBrakeTorque = 0f;
            rearBrakeTorque = totalBrakeTorque;
        }
        else
        {
            frontBrakeTorque = totalBrakeTorque * m_frontBrakeBias;
            rearBrakeTorque = totalBrakeTorque - frontBrakeTorque;
        }

        return (_isFront) ? frontBrakeTorque : rearBrakeTorque;
    }
}
