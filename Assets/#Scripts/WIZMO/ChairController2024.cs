/**
 * @file    ChairController2024.cs
 * @brief   �֎q�̐���p�̃N���X     
 * @author  22CU0225 �L�c�B��
 * @date    2024/07/24 �쐬
 *          2024/08/28 ShiftShock�̍쐬
 *          2024/09/20 Yaw�̓����쐬
 * 
 * ����
 * 
 * �N���X�쐬�҂Ɋւ���
 * ������������ ���@�S�S�P������H
 * ------       ���@21cu0203_�r�c�A������H
 * ======       ���@22CU0225 �L�c�B��
 * 
 */

/* WIZMOController �́@�ϐ�����
 * ���я��͂�����ɏ�����          2024�g�p�֐�
 * Roll                                 �Z
 * Pitch                                �Z
 * Yaw                                  �Z
 * Heave                                �Z
 * Sway                                 �~
 * Surge                                �~
 * Speed                                �Z
 * Accel                                �Z
 */

using System.IO;
using UnityEngine;
using UnityEngine.Rendering;

public class ChairController2024 : MonoBehaviour
{
    #region �Q�Ƃ���N���X
    // WIZMO
    [SerializeField]
    private WIZMOController m_controller;
    // Vehicle
    private GameObject m_vehicleObject;
    // VehicleController
    private VehicleController m_vehiclecontroller;
    // Load
    private LoadTransfer m_load = new LoadTransfer();
	#endregion

	//Axis Processing
	[SerializeField,Range(-1.0f, 1.0f)]
	private float m_roll = 0.0f;
	[SerializeField, Range(-1.0f, 1.0f)]
	private float m_pitch = 0.0f;
	[SerializeField, Range(-1.0f, 1.0f)]
	private float m_yaw = 0.0f;
	[SerializeField, Range(-1.0f, 1.0f)]
	private float m_heave = 0.0f;
	[SerializeField, Range(-1.0f, 1.0f)]
	private float m_sway = 0.0f;
	[SerializeField, Range(-1.0f, 1.0f)]
	private float m_surge = 0.0f;
    [SerializeField, Range(0f, 1.0f)]
	private float m_speed = 0.0f;
    [SerializeField, Range(0f, 1.0f)]
	private float m_accel = 0.0f;

    private float m_maxForce = 3f;              // ������͂̍ő�l Debug�̒l���炨���悻�̒l
    
    // �{�^��
    [Space]
    [SerializeField]
    private bool m_isChairOperatingKey = false;  // �N��
    [SerializeField, ShowInInspector]
    private bool m_isChairWeakForce = false;     // �֎q�̐���



    [Header("Shift")]
    // shift
    [SerializeField]
    private float m_finalReductionRatio = 3.462f;   // �ŏI������
    [SerializeField]
    private float m_driveWheelRadius = 0.334f;      // �쓮�^�C���̔��a
    private float m_wheelCircumference;             // �^�C���̉~��
    private int m_beforeGear;                       // �ۑ������M�A
	
    [SerializeField]
	private float m_smoothTime = 0.7f;          // ����
    private float m_smoothMaxSpeed = 5000;      // �X�s�[�h
    private float m_currentVelocityRoll;        // �x���V�e�BRoll
    private float m_currentVelocityPitch;       // �x���V�e�BPitch
    private float m_currentVelocityYaw;         // �x���V�e�BYaw

    // ���ꂼ���K�����銄��
    [Header("Ratio")]
    [SerializeField]
    private float m_accelRatio = 1f;            // �����͓K������
    [SerializeField]
    private float m_centrifugalRatio = 1f;      // ���S�͓K������
    [SerializeField]
    private float m_yawRateRatio = 1f;          // ���[���[�g�K������  
    [Space]
    //[SerializeField]
    //private float m_gasPedalInputRatio = 1f;    // �A�N�Z�����͓K������
    [SerializeField]
    private float m_brakePedalInputRatio = 1f;  // �u���[�L���͓K������
    [SerializeField]
    private float m_shiftShockRatio = 1f;       // �V�t�g�V���b�N�K������

    [Header("Engine")]
    [SerializeField]
    private float m_frequency = 0.16f;          // �U���̎��g��

    [SerializeField]
    private float m_engineRatio = 1f;           // �U���̓K������

    private float m_engineIdleRPM = 1000f;      // ����RPM
    private float m_engineLimitRPM = 10000f;    // �ō�RPM

    // 臒l
    [Header("Threshold")]
    [SerializeField]
    [Range(0, 9000)]
    private float m_minShiftShockRPM = 500.0f;      // RPM臒l
    [Space]
    [SerializeField, Range(0, 1)]
    private float m_thresholdRoll = 0.2f;          // Roll臒l
    [SerializeField, Range(0, 1)]
    private float m_thresholdPitch = 0.2f;          // Pitch臒l
    [SerializeField, Range(0, 0.2f)]
    private float m_thresholdYaw = 0.02f;            // Yaw臒l
    [Space]
    [SerializeField, Range(0, 1)]
    private float m_thresholdClutchInput = 0.8f;    // Clutch臒l

    #region �v���p�e�B
    public bool InGameMove
    {
        set => m_isChairOperatingKey = value;
    }

    public float Speed
    {
        get => m_speed;
        set => m_speed = value;
    }

    public float Accel
    {
        get => m_accel;
        set => m_accel = value;
    }

    public float SmoothTime
    {
        get => m_smoothTime;
        set => m_smoothTime = value;
    }

    public float AccelRatio
    {
        get => m_accelRatio;
        set => m_accelRatio = value;
    }

    public float CentrifugalRatio
    {
        get => m_centrifugalRatio;
        set => m_centrifugalRatio = value;
    }
    
    public float YawRateRatio
    {
        get => m_yawRateRatio;
        set => m_yawRateRatio = value;
    }

    //public float GasPedalRatio
    //{
    //    get => m_gasPedalInputRatio;
    //    set => m_gasPedalInputRatio = value;
    //}

    public float BrakePedalRatio
    {
        get => m_brakePedalInputRatio;
        set => m_brakePedalInputRatio = value;
    }

    public float ShiftShockRatio
    {
        get => m_shiftShockRatio;
        set => m_shiftShockRatio = value;
    }

    public float EngineFrequency
    {
        get => m_frequency;
        set => m_frequency = value;
    }

    public float EngineRatio
    {
        get => m_engineRatio;
        set => m_engineRatio = value;
    }

    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Initialize();

    }

    void Initialize()
    {
        m_vehicleObject = transform.root.gameObject;
        m_vehiclecontroller = m_vehicleObject.GetComponent<VehicleController>();
        if (m_controller == null)
        {
			m_controller = GameManager.Instance.WIZMO;
        }
        // �M�A�擾
        m_beforeGear = m_vehiclecontroller.Transmission.ActiveGear;
        // �^�C���̉~��
        m_wheelCircumference = m_driveWheelRadius * 2 * Mathf.PI;

        m_load.Vehicle = m_vehicleObject;
        m_load.Initialize();
        m_speed = 0.75f;
        m_accel = 0.65f;
        m_accelRatio = 0.9f;
        m_centrifugalRatio = 0.75f;
    }

	private void FixedUpdate()
	{
		if (m_isChairOperatingKey)
		{
			m_load.UpdateProcess();
		}
	}

	// Update is called once per frame
	void Update()
    {
        SwitchChairForce();
        if (m_isChairOperatingKey)
        {     
            UpdateChairValue();
        }
    }

    // ===========================
    //   �֎q�̐�����؂�ւ���
    // ===========================
    private void SwitchChairForce()
    {
        if(Input.GetKeyDown(KeyCode.F10))
        {
            m_isChairWeakForce = !m_isChairWeakForce;
            if(m_isChairWeakForce)
            {
                m_speed = 0.6f;
                m_accel = 0.5f;
                m_accelRatio = 0.6f;
                m_centrifugalRatio = 0.6f;
                m_yawRateRatio = 0;
            }
            else
            {
                m_speed = 0.85f;
                m_accel = 0.75f;
                m_accelRatio = 0.8f;
                m_centrifugalRatio = 0.8f;
                m_yawRateRatio = 1f;
            }
        }
    }

    // ===========================
    //   �֎q�̒l���X�V����֐�
    // ===========================
    private void UpdateChairValue()
    {
		// �׏d�ɂ���]���f
		m_roll = Mathf.SmoothDamp(m_roll, ChairRoll(), ref m_currentVelocityRoll, m_smoothTime, m_smoothMaxSpeed, Time.deltaTime);
        //Debug.Log("Roll:" + m_roll);
        m_pitch = Mathf.SmoothDamp(m_pitch, ChairPitch(), ref m_currentVelocityPitch, m_smoothTime, m_smoothMaxSpeed, Time.deltaTime);
        m_yaw = Mathf.SmoothDamp(m_yaw, ChiarYaw(), ref m_currentVelocityYaw, m_smoothTime, m_smoothMaxSpeed, Time.deltaTime);

        // �V�t�g�V���b�N
        // �M�A�`�F���W������
        if (m_beforeGear != m_vehiclecontroller.ActiveGear)
        {
            // �N���b�`�𓥂�ł��Ȃ�
            if (m_vehiclecontroller.Clutch > m_thresholdClutchInput)
            {
                // �V�t�g�V���b�N�̌v�Z���ʂɊ����������ăs�b�`�ɓ���
                m_pitch += ShiftShock() * m_shiftShockRatio;
                //Debug.Log("shift");
            }
        }
        // �G���W��
        //EngineRatioChanger();
        m_heave = EngineVibration() * m_engineRatio;

        // �l�X�V
        ToWIZMOController();
	}

	// ===========================
	// WIZMOController�ɒl��n��
	// =========================== 
    private void ToWIZMOController()
    {
        // �ی��̈�WIZMO�͈̔͂ɐ���
        m_roll  = Mathf.Clamp(m_roll,  -1f,1f);
        m_pitch = Mathf.Clamp(m_pitch, -1f,1f);
        m_yaw   = Mathf.Clamp(m_yaw,   -1f,1f);
        m_heave = Mathf.Clamp(m_heave, -1f,1f);
        m_sway  = Mathf.Clamp(m_sway,  -1f,1f);
        m_surge = Mathf.Clamp(m_surge, -1f,1f);
        m_speed = Mathf.Clamp(m_speed,  0f,1f);
        m_accel = Mathf.Clamp(m_accel,  0f,1f);


		m_controller.roll  = m_roll;
		m_controller.pitch = m_pitch;
		m_controller.yaw   = m_yaw;
		m_controller.heave = m_heave;
		m_controller.sway  = m_sway;
		m_controller.surge = m_surge;
        m_controller.speed1_all = m_speed;
		m_controller.accel      = m_accel;
	}

	// ===========================
	// ���S�͂̏���(Roll)
	// ===========================
	private float ChairRoll()
    {
        float Centrifugal = 0.0f;

		// ���S�͗ʂƊ������|����m_maxForce�͈̔͂���WIZMO�͈̔͂ɃX�P�[�����O����
		Centrifugal = ScaleValue(m_load.Centrifugal * m_centrifugalRatio, -m_maxForce, m_maxForce, -1f, 1f);
		// ���S�͗ʂ�WIZMO�͈̔͂ɐ���
		Centrifugal = Mathf.Clamp(Centrifugal, -1f, 1f);

		// 臒l�ȉ��̏ꍇ�X�����Ȃ���
		if (Mathf.Abs(Centrifugal) < m_thresholdRoll) Centrifugal = 0;

        return Centrifugal;
    }

    // ===========================
    // �����������̏���(Pitch)
    // ===========================
    private float ChairPitch()
    {
        float Accelaration = 0.0f;

		// �����ʂƊ������|����m_maxForce�͈̔͂���WIZMO�͈̔͂ɃX�P�[�����O����
		Accelaration = ScaleValue(m_load.Accel * m_accelRatio, -m_maxForce, m_maxForce, -1f, 1f);
		// �����ʂ�WIZMO�͈̔͂ɐ���
		Accelaration = Mathf.Clamp(Accelaration, -1, 1);

		// 臒l�ȉ��̏ꍇ�X�����Ȃ���
		if (Mathf.Abs(Accelaration) < m_thresholdPitch) return 0;

        // �A�N�Z���̓��͗ʂɂ���ČX�������Z
        //if(Accel > 0)
        //{
        //    Accel += m_vehiclecontroller.Accel/ m_gasPedalRatio;
        //}
		// �u���[�L�̓��͗ʂƓK���������|����
		if (Accelaration < 0)
        {
            Accelaration *= m_vehiclecontroller.Brake * m_brakePedalInputRatio;
        }

        return Accelaration;
    }

    // ===========================
    // �ԑ̂̉�]�̏���(Yaw)
    // ===========================
    private float ChiarYaw()
    {
        float YawRate = 0;
		// ���[���[�g�ʂƊ������|����m_maxForce�͈̔͂���WIZMO�͈̔͂ɃX�P�[�����O����
		YawRate = ScaleValue(m_load.YawRate * m_yawRateRatio, -m_maxForce, m_maxForce, -1f, 1f);
		// ���[���[�g�ʂ�WIZMO�͈̔͂ɐ���
		YawRate = Mathf.Clamp(YawRate, -1, 1);
        // 臒l�ȉ��̏ꍇ�X�����Ȃ���
        if (Mathf.Abs(YawRate) < m_thresholdYaw) YawRate = 0;

        return YawRate;
    }

    // ==================================
    //  �V�t�g�`�F���W���̔���  
    // ==================================
    private float ShiftShock()
    {
        float result = 0.0f;
        float nowRPM = m_vehiclecontroller.EngineRPM;
        float targetRPM = 0.0f;

        // �V�t�g��̃M�A��RPM�v�Z    RPM = ���x/((�^�C���̉~��*�����ϊ��p60/1000)/(�M�A��* �ŏI������))
        targetRPM = m_vehiclecontroller.KPH / ((m_wheelCircumference * 0.06f) / (m_vehiclecontroller.Transmission.CurrentGearRatio * m_finalReductionRatio));
        result = nowRPM - targetRPM;

        // RPM�̍����������ꍇ�U���𖳂���
        if (Mathf.Abs(result) < m_minShiftShockRPM)
        {
            result = 0.0f;
        }
        // �����擾
        float sign = Mathf.Sign(result);
        // 0�`1�̊����ɂ���
        result = Mathf.InverseLerp(m_engineIdleRPM, m_engineLimitRPM, Mathf.Abs(result));
        //�����߂�
        result *= sign;

        // �M�A�̒l���X�V
        m_beforeGear = m_vehiclecontroller.ActiveGear;

        //Debug.Log("RPM" + nowRPM);
        //Debug.Log("�ڕWRPM" + targetRPM);
        //Debug.Log("�V�t�g�V���b�N" + result);

        // �l��Ԃ�
        return result;
    }

    #region �G���W��

    // ----------------------------------
    // sin�g���g�p�����㉺�̐U����
    // ----------------------------------
    float EngineVibration()
    {
        // �U�������v�Z����
        float f = 1.0f / m_frequency;

        // sin�g���쐬����
        float sin = Mathf.Sin(2 * Mathf.PI * f * Time.fixedTime);

        // �l������������ꍇ
        if (sin > -0.001f && sin < 0.001f)
        {
            if (sin > 0.0f)
            {
                // 0�ɕ␳����
                sin = 0.001f;
            }
            else
            {
                // 0�ɕ␳����
                sin = -0.001f;
            }
        }

        // �l��Ԃ�
        return sin;
    }
	#endregion

	// �����ɉ����ĐU���䗦��ω�������
    private void EngineRatioChanger()
    {
        float Ratio = Mathf.Clamp(m_load.Accel, 0, 1f);

        m_engineRatio = ScaleValue(Ratio, 0, 1, 0, 0.016f);
    }

	// -----------------------------------------
	// �l���w�肵���͈͂ɃX�P�[�����O����֐�
	// ����1 : �X�P�[�����O���s���l
	// ����2 : ���݂̍ŏ��l
	// ����3 : ���݂̍ő�l
	// ����4 : �X�P�[�����O����ŏ��l
	// ����5 : �X�P�[�����O����ő�l
	// -----------------------------------------
	private float ScaleValue(float value, float NowMin, float NowMax, float ScaleMin, float ScaleMax)
    {
        // �l��V�����͈͂ɕϊ�
        return ScaleMin + (value - NowMin) * (ScaleMax - ScaleMin) / (NowMax - NowMin);
    }
}