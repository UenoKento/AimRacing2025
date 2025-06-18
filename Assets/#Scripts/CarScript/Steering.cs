using UnityEngine;

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
    float m_wheelBase = 2.59f;      //ホイールベース
    [SerializeField]
    float m_treadWidth;             //トレッド幅
    [SerializeField]
    float m_steeringGearRatio;      //ステアとホイールのギア比
    [SerializeField,Range(0f,450f)]
    float m_maxSteerAngle;          //ステアのMax角度
    [SerializeField, Range(180f, 450f)]
    float m_steeringRange = 450f;   //ステアの倍率

    /// <summary>
    /// ホイールのステア角を計算する
    /// </summary>
    /// <param name="_steerInput">-1~1の間のハンドルの入力</param>
    /// <param name="_isRight">右側のホイールかどうか</param>
    public float CalcSteerAngle(in float _steerInput, bool _isRight)
    {
        //ステアリングの角度計算
		float steerAngle = _steerInput * m_steeringRange / m_steeringGearRatio;

        //上限の設定
        //if(m_maxSteerAngle <= Mathf.Abs(steerAngle))
        //{
        //    steerAngle = m_maxSteerAngle * Mathf.Sign(steerAngle);
        //}

        //
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

    //ホイールの
    float CalcAckermanAngle(float _steerAngle,bool _isRight)
    {
        _steerAngle *= Mathf.Deg2Rad;
		float angleR = Mathf.Atan(m_wheelBase * Mathf.Tan(_steerAngle) / (m_wheelBase + 0.5f * m_treadWidth * Mathf.Tan(_steerAngle))) * Mathf.Rad2Deg;
		float angleL = Mathf.Atan(m_wheelBase * Mathf.Tan(_steerAngle) / (m_wheelBase - 0.5f * m_treadWidth * Mathf.Tan(_steerAngle))) * Mathf.Rad2Deg;

        if(_steerAngle > 0f)
        {
            angleL = CalcAckermanOutsideAngle(angleR, _steerAngle);
        }
        else
        {
			angleR = CalcAckermanOutsideAngle(angleL, _steerAngle);
		}



        if (_isRight)
        {
            return angleR;
        }
        else
        {
            return angleL;
        }
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
