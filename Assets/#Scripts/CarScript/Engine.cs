using System.Collections;
using UnityEngine;

[System.Serializable]
public class Car_Engine
{
    // ����RPM�Ő����ł���ő�̃g���N�̃}�b�v(�G���W�������O���t(�g���N�̂�))
    [SerializeField]
    AnimationCurve m_torqueCurve;

    [SerializeField, ShowInInspector]
    float m_angularVelocity;    // �G���W���̃t���C�z�C�[���̊p���x
    [SerializeField, ShowInInspector]
    float m_engineRPM;          // ���݂̃G���W����]��
    [SerializeField, ShowInInspector]
    float m_effectiveTorque;    // �����g���N�l
    [SerializeField, ShowInInspector]
    float m_ReactionTorque;    // �����g���N�l
    [SerializeField]
    float m_inertia = 0.2f;     // �G���W���̊������[�����g
    float RotationalVolume_Crankshaft;  //�N�����N�V���t�g�̉�]��


    [Header("Engine_Design")]
    public float Cylinders = 1;
    public float IdleRPM;
    public float CellMotorVelocity;
    public float RPM_Offset;
    bool Pressed_Button = false;
    [SerializeField]
    PID pid;

    [Header("Throttle")]
    public float MaxThrottle;
    public float m_Throttle;
    public float AdjustThrottle;

    [Header("Friction")]
    [SerializeField]
    float m_frictionAtIdle = 50f;  // �A�C�h�����̖��C�g���N(�ŏ����C�g���N)
    [SerializeField]
    float m_frictionCoef = 0.012f; // �G���W�������C�W��
    [SerializeField]
    float m_InertiaFrictionCoef = 0.015f;   // �G���W�����������C�W��
    [SerializeField]
    public float Friction_Torque;           //���C�g���N


    [Header("Rev Limiter")]
    [SerializeField]
    float m_overRevRPM;// ���u���~�b�^�[���N������RPM
    [SerializeField]
    float m_limitRPM = 9000f;    // ��]���̌��E
    bool m_InjectionCut_Rev = false;
    bool m_injectionCut = false;


    #region �v���p�e�B
    public float RPM
    {
        get => m_engineRPM;
    }

    public float EngineTorque
    {
        get => m_effectiveTorque;
    }

    // �p�^����(�N���b�`�̌v�Z�Ɏg�p)
    public float AngularMomentum
    {
        get => m_angularVelocity * m_inertia;
    }

    // �I�[�o�[���uRPM
    public float OverRevRPM
    {
        get => m_overRevRPM;
        set => m_overRevRPM = value;
    }

    // �C���W�F�N�V�����J�b�g(�X���b�g����0�ɂ���)
    public bool InjectionCut
    {
        get => m_injectionCut;
        set => m_injectionCut = value;
    }
    #endregion


    //Test
    float One_Cycle = 4 * Mathf.PI;
    public float DeltaTime_Ratio;

    Car_Engine()
    {
        pid = new PID(0.0024f, 0.0f, 0.0f);
    }

    public void FixedUpdate(float _Throttle, float _ReactionTorque)
    {
        m_Throttle = _Throttle;
        m_ReactionTorque = _ReactionTorque;

        //���C�g���N�̌v�Z
        Calclate_FrictionTorque();

        //�Z�����[�^�[�̎n��
        //Move_CellMotor();

        //���C�E�쓮�փg���N�̔��f
        AddBackTorque();

        //RPM�\���ɑΉ�������
        m_engineRPM = m_angularVelocity * CarPhysics.Rad2RPM;

        //�A�C�h�����O�̃X���b�g���J�x�̒���
        FeedBack_Throttle();

        //���΃T�C�N���̉��Z
        Calclate_IgnitionCycle();

        m_effectiveTorque = m_angularVelocity * m_inertia;

        //_rb.angularVelocity = new Vector3(0.0f, 0.0f, m_angularVelocity);

    }

    void Calclate_FrictionTorque()
    {
        //�ŏ����C�g���N�̊|�������
        float Engine_IdlefrictionRatio;

        //��~���͒�R������
        if (m_angularVelocity == 0)
        {
            Engine_IdlefrictionRatio = 0;
        }
        else
        {
            Engine_IdlefrictionRatio = m_frictionAtIdle / Mathf.Abs(m_frictionAtIdle);
        }

        // ���C�g���N = (�ŏ����C�g���N * �G���W���̉�]����) + (���C�W�� * RPM) + (������R�W�� * RPM^2)
        Friction_Torque =
            (m_frictionAtIdle * Engine_IdlefrictionRatio) +
            (m_frictionCoef * m_angularVelocity) +
            (m_InertiaFrictionCoef * Mathf.Pow(m_angularVelocity, 2f));

    }

    void AddBackTorque()
    {
        //���C�E�쓮�փg���N���G���W����]���ɔ��f
        m_angularVelocity -= (((Friction_Torque + m_ReactionTorque) / m_inertia)) * (Time.fixedDeltaTime * DeltaTime_Ratio);

        //�t��]���Ȃ��悤����
        m_angularVelocity = Mathf.Clamp(m_angularVelocity, 0.0f, 100000.0f);
    }



    void FeedBack_Throttle()
    {
        //PID�t�B�[�h�o�b�N�����̎��s(�ڕW�l�܂ŏグ��)
        AdjustThrottle = pid.Compute_IdleThrottle(m_engineRPM, IdleRPM);

        //�X���b�g���ʂ̐���
        AdjustThrottle = Mathf.Clamp(AdjustThrottle, 0.0f, 1000.0f);
    }

    void Calclate_IgnitionCycle()
    {
        //�ړ��ʂ̉��Z�E����
        RotationalVolume_Crankshaft += m_angularVelocity * (Time.fixedDeltaTime * DeltaTime_Ratio);
        RotationalVolume_Crankshaft = Mathf.Clamp(RotationalVolume_Crankshaft, 0.0f, 10000.0f);

        //�w�肳�ꂽ�ʉ�]������A���΍H���̉��Z������

        while (RotationalVolume_Crankshaft > (One_Cycle / Cylinders) && m_angularVelocity > 0)
        {
            //��]�����Ԃ����
            RotationalVolume_Crankshaft -= One_Cycle / Cylinders;

            //��]���̐���
            RevLimitter();

            //��]���̒ǉ�(���E�̉�]���ɂȂ��Ă��Ȃ����)
            if (!m_InjectionCut_Rev) AddangularVelocity();
        }
    }

    void AddangularVelocity()
    {
        //�X���b�g���J�x�̐���
        MaxThrottle = Mathf.Clamp01(m_Throttle + AdjustThrottle);

        //�g���N�̌v�Z�E���f
        float Torque = (m_torqueCurve.Evaluate(m_angularVelocity * CarPhysics.Rad2RPM) / Cylinders) * MaxThrottle;
        m_angularVelocity += Torque / m_inertia * (Time.fixedDeltaTime * DeltaTime_Ratio);
    }


    public IEnumerator Move_CellMotor()
    {
        //�Z�����[�^�[����
        //if (!Pressed_Button) { return; }

        while(m_engineRPM < IdleRPM)
        {
			m_angularVelocity += CellMotorVelocity * Time.deltaTime;

            yield return null;
		}

	}

    void RevLimitter()
    {
        if (m_engineRPM > m_limitRPM && !m_InjectionCut_Rev)
        {
            m_InjectionCut_Rev = true;
        }
        else if (m_engineRPM < m_overRevRPM && m_InjectionCut_Rev)
        {
            m_InjectionCut_Rev = false;
        }
    }

}
