using UnityEngine;
using UnityEngine.UIElements;
using VehiclePhysics;

[System.Serializable]
public class Steering
{
    enum SteeringType
    {
        Ackerman,
        Parallel
    }

    [SerializeField]
    SteeringType m_type = SteeringType.Ackerman;

    [SerializeField]
    float m_wheelBase;
    [SerializeField]
    float m_treadWidth;
    [SerializeField]
    float m_steeringGearRatio;
    [SerializeField, Range(0f, 900f)]
    float m_maxSteerAngle;
    [SerializeField, Range(180f, 900f)]
    float m_steeringRange = 900f;

    /// <summary>
    /// ホイールのステア角を計算する
    /// </summary>
    /// <param name="_steerInput">-1~1の間のハンドルの入力</param>
    /// <param name="_isRight">右側のホイールかどうか</param>
    public float CalcSteerAngle(in float _steerInput, bool _isRight)
    {
        float steerAngle = _steerInput * m_steeringRange / m_steeringGearRatio;
        if (m_maxSteerAngle <= Mathf.Abs(steerAngle))
        {
            steerAngle = m_maxSteerAngle * Mathf.Sign(steerAngle);
        }

        switch (m_type)
        {
            case SteeringType.Ackerman:
                return CalcAckermanAngle(steerAngle, _isRight);

            case SteeringType.Parallel:
                return steerAngle;

            default:
                return 0f;
        }
    }

    float CalcAckermanAngle(float _steerAngle, bool _isRight)
    {
        _steerAngle *= Mathf.Deg2Rad;
        float angleR = Mathf.Atan(m_wheelBase * Mathf.Tan(_steerAngle) / (m_wheelBase + 0.5f * m_treadWidth * Mathf.Tan(_steerAngle))) * Mathf.Rad2Deg;
        float angleL = Mathf.Atan(m_wheelBase * Mathf.Tan(_steerAngle) / (m_wheelBase - 0.5f * m_treadWidth * Mathf.Tan(_steerAngle))) * Mathf.Rad2Deg;

        // 05/18 追加:土田
        // 速度が遅いほどsteerFactorが大きくなるように補正
        //float maxSpeed = 250f; // 最大速度
        //float steerFactor = 1f;
        //steerFactor = Mathf.Clamp01(1f - (m_vehicleController.m_KPH * m_vehicleController.m_KPH) / (maxSpeed * maxSpeed));

        if (_steerAngle > 0f)
        {
            angleL = CalcAckermanOutsideAngle(angleR, _steerAngle);
        }
        else
        {
            angleR = CalcAckermanOutsideAngle(angleL, _steerAngle);
        }

        return _isRight ? angleR : angleL;
    }


    /// <summary>
    /// 内側の切れ角から外側の切れ角を計算する
    /// 2025/06/18 小見川 処理の変更を行いました。
    /// </summary>
    float CalcAckermanOutsideAngle(float _insideAngle, float _steerAngle)
    {
        //修正前の式
        //float ackermanPer = m_treadWidth / m_wheelBase;
        //return _insideAngle - ackermanPer * (_insideAngle - _steerAngle);

        bool IsRight = true;

        if (_insideAngle < 0)
        {
            _insideAngle *= -1;
            IsRight = false;
        }

        float tanA = Mathf.Tan(_insideAngle * Mathf.Deg2Rad);

        float angle = Mathf.Atan(m_wheelBase * tanA / ((m_treadWidth * tanA) + m_wheelBase)) * Mathf.Rad2Deg;

        if (!IsRight) { angle *= -1; }

        return angle;
    }

}
