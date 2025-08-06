/**
 * @file    LoadTransfer.cs
 * @brief   �d�S�ɂ�����͂��v�Z����@�\        
 * @author  22CU0225�L�c�B��
 * @date    2024/09/01 �쐬

 */
using UnityEngine;

public class LoadTransfer
{
    #region �ϐ�

    [Header("Vehicle")]
    [SerializeField] private GameObject m_vehicle = null;
    // VehicleController
    private VehicleController m_vehicleController;

    // �Ԃ̍���
    private Rigidbody m_vehicleRigitbody;   // �Ԃ̍���

    [Space]
    [SerializeField] private float m_AccleCoefficient = 1f;             // �������p�W��
    [SerializeField] private float m_CentrifugeForceCoefficient = 1f;   // ���S�͗p�W��

    // ���x
    private Vector3 m_velocity;                 // ���x
    private Vector3 m_prevVelocity;             // �O��̑��x

    private float m_forwardSpeed;               // �O�������x

    // �d�S�ɂ������
    private float m_accel;          // ������
    private float m_centrifugal;    // ���S��
    private float m_YawRate;        // ���[���[�g


    #endregion

    #region �v���p�e�B
    public GameObject Vehicle
    {
        set { m_vehicle = value; }
    }

    public float ForwardSpeed => m_forwardSpeed;
    public float Accel => m_accel;
    public float Centrifugal => m_centrifugal;
    public float YawRate => m_YawRate;
    #endregion


    // �����ݒ�
    public void Initialize()
    {
        // ������
        m_prevVelocity = Vector3.zero;
        m_vehicleController = m_vehicle.GetComponent<VehicleController>();
        m_vehicleRigitbody = m_vehicle.GetComponent<Rigidbody>();
    }

    // �X�V
    public void UpdateProcess()
    {
        #region �Ԃ��������ݒ肳��Ă��邩�m�F
        if (m_vehicle.tag == null)  // Null�`�F�b�N
        {
            Debug.LogError("�׏d�ړ��Ŏg���Ԃ��ݒ肳��Ă��܂���/Load2024.cs/");
            return;
        }
		#endregion

		// ���x�̎擾
		m_velocity = m_vehicleRigitbody.linearVelocity;

        // �v�Z
        LongitudinalLoadTransfer();
        LateralLoadTransfer();

        // �擾
        YawRateTransfar();

		// ���x�̕ۑ�
		m_prevVelocity = m_velocity;
    }


    // �����͌v�Z
    private void LongitudinalLoadTransfer()
    {
        // ���ʕ����x�N�g��
        Vector3 forward = m_vehicle.transform.forward;
        // ���ʕ����x�N�g���Ƒ��x�x�N�g���̓��ς𐳖ʕ����x�N�g���Ƃ����đO�����x�x�N�g�����擾
        Vector3 forwardVelocity = Vector3.Dot(m_velocity, forward) * forward;
        m_forwardSpeed = forwardVelocity.magnitude;

        Vector3 prevForwardVelocity = Vector3.Dot(m_prevVelocity, forward) * forward;
        // �����x [G]�@[Vf - V0f / t / g ]
        float acceleration = (forwardVelocity.magnitude - prevForwardVelocity.magnitude) / Time.deltaTime / Physics.gravity.magnitude;
        m_accel = acceleration; // �l�ێ�
        //Debug.Log("�����x" + acceleration);
    }

    // ���S�͌v�Z
    private void LateralLoadTransfer()
    {
        // �E�����x�N�g��
        Vector3 sideway = m_vehicle.transform.right;
        // �E�����x�N�g���Ƒ��x�x�N�g���̓��ς��E�����x�N�g���Ƃ����ĉE���x�x�N�g�����擾
        Vector3 sidewayVelocity = Vector3.Dot(m_velocity, sideway) * sideway;
        Vector3 prevSidewayVelocity = Vector3.Dot(m_prevVelocity, sideway) * sideway;

        // F(���S��) = mass * (�O�̃x�N�g���̑傫���@- ���݂̃x�N�g���̑傫��)^2 / r(�ŏ���]���a(5m))
        float centrifugalForce = 0.0f;
        float minRadius = 5.0f;
        float v0 = sidewayVelocity.magnitude - prevSidewayVelocity.magnitude;
        centrifugalForce = m_vehicleRigitbody.mass * v0 * 2.0f / minRadius;
        m_centrifugal = centrifugalForce;

		//// ���S�́i���S�����x�j [G] [Vs - V0s / t / g ]
		//// �A���O���x���V�e�BY�ō��E���f �܂��@���S�͂Ȃ̂Ŕ��]
		//float centrifugalForce = -1 * Mathf.Sign(m_vehicleRigitbody.angularVelocity.y) * (sidewayVelocity.magnitude - prevSidewayVelocity.magnitude) / Time.deltaTime / Physics.gravity.magnitude;
		//m_centrifugal = centrifugalForce;   // �l�ێ�
		////Debug.Log("���S��" +  centrifugalForce);
	}

	// ���[���[�g�擾
	private void YawRateTransfar()
    {
        m_YawRate = m_vehicleRigitbody.angularVelocity.y;
    }
}
