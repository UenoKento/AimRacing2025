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
    float m_wheelBase;
    [SerializeField]
    float m_treadWidth;
    [SerializeField]
    float m_steeringGearRatio;
    [SerializeField,Range(0f,450f)]
    float m_maxSteerAngle;
    [SerializeField, Range(180f, 450f)]
    float m_steeringRange = 450f;

    /// <summary>
    /// �z�C�[���̃X�e�A�p���v�Z����
    /// </summary>
    /// <param name="_steerInput">-1~1�̊Ԃ̃n���h���̓���</param>
    /// <param name="_isRight">�E���̃z�C�[�����ǂ���</param>
    public float CalcSteerAngle(in float _steerInput, bool _isRight)
    {
        //�X�e�A�����O�̊p�x�v�Z
		float steerAngle = _steerInput * m_steeringRange / m_steeringGearRatio;

        //����̐ݒ�
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
    /// �����̐؂�p����O���̐؂�p���v�Z����
    /// </summary>
    float CalcAckermanOutsideAngle(float _insideAngle,float _steerAngle)
    {
        float ackermanPer = m_treadWidth / m_wheelBase;
        return _insideAngle - ackermanPer * (_insideAngle - _steerAngle);
    }

}
