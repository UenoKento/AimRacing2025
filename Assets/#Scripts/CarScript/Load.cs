using UnityEngine;

[AddComponentMenu("AIM/Load")]
public class Load : MonoBehaviour
{
	// �ԑ�
	[Header("Vehicle")]
	[SerializeField] 
	private GameObject m_vehicle = null;

	// ���[���Z���^�[ (�n�ʂ܂ł̏d�S���� + �^�C�����a/2)
	[SerializeField] 
	private float m_rollcenterLangth = 0.3f + -0.3343f / 2.0f;

	// WheelControlelr
	[Header("Wheel")]
	[SerializeField] 
	private WheelController2024 m_wheelController_FR;       
	[SerializeField] 
	private WheelController2024 m_wheelController_FL;        
	[SerializeField] 
	private WheelController2024 m_wheelController_RR;        
	[SerializeField] 
	private WheelController2024 m_wheelController_RL;        

	[Header("Ray")]
	[SerializeField] 
	private LayerMask m_layerMask = Physics.IgnoreRaycastLayer;
	[SerializeField] 
	private float m_maxDistance = 1;

	// �����l
	[Header("Limit")]
	[SerializeField] 
	private float m_minLoadPer = 0f;
	[SerializeField] 
	private float m_maxLoadPer = 1f;
	[Space]
	[SerializeField] 
	private float m_minAcceleration = 0.1f;
	[SerializeField] 
	private float m_maxAcceleration = 4.0f;
	[Space]
	[SerializeField] 
	private float m_minCentrifugalForce = 0.1f;
	[SerializeField] 
	private float m_maxCentrifugalForce = 4.0f;
	[Space]
	[SerializeField] 
	private float m_minAngle = 10.0f;             
	[SerializeField] 
	private float m_maxAngle = 45.0f;             

	[Header("Load")]
	[SerializeField]
	[Range(0, 1)] 
	private float m_load_FR;        // ���z�׏d FR
	[SerializeField]
	[Range(0, 1)] 
	private float m_load_FL;        // ���z�׏d FL
	[SerializeField]
	[Range(0, 1)] 
	private float m_load_RR;        // ���z�׏d RR
	[SerializeField]
	[Range(0, 1)] 
	private float m_load_RL;        // ���z�׏d RL

	[Space]
	[SerializeField]
	[Range(0, 1)] 
	private float m_Front_LoadRatio;	// �O�׏d����
	[SerializeField]
	[Range(0, 1)] 
	private float m_load_Rear;          // ��׏d����

	// VehicleController
	private VehicleController m_vehicleController;

	// �Ԃ̍���
	private Rigidbody m_vehicleRigitbody;   // �Ԃ̍���

	// Ray
	private float m_fly = -1;           // �G��Ă��Ȃ��Ƃ�
	private Vector3 m_graundNomal;      // hit�n�_�̒n�ʂ̖@���x�N�g��

	// ����
	private float m_wheelBase;          // �z�C�[���x�[�X
	private float m_treadFront;         // �O�փg���b�h
	private float m_treadRear;          // ��փg���b�h

	private float m_distace_Rear;       // ��ւ܂ł̒���
	private float m_langeHight;         // �n�ʂ���̍���

	// ���x
	private Vector3 m_velocity;                 // ���x
	private Vector3 m_prevVelocity;             // �O���x

	// �ԏd
	private float m_weight;

	// �d�S�ɂ������
	private float m_accel;          // ������
	private float m_centrifugal;    // ���S��

	private float m_forwardSpeed;

	#region �v���p�e�B
	public float Loat_Front => m_Front_LoadRatio;
	public float Loat_Rear => m_load_Rear;
	public float Loat_Right => m_load_FR;
	public float Loat_Left => m_load_FL;

	public float ForwardSpeed => m_forwardSpeed;

	public float Accel => m_accel;
	public float Centrifugal => m_centrifugal;
	#endregion

	// AddComponent/Reset �����Ƃ��̐ݒ�
	private void Reset()
	{
		m_vehicle = gameObject;
		GameObject childWheelColliders = gameObject.transform.Find("WheelColliders").gameObject;
		m_wheelController_FR = childWheelColliders.transform.Find("WheelController_FR").GetComponent<WheelController2024>();
		m_wheelController_FL = childWheelColliders.transform.Find("WheelController_FL").GetComponent<WheelController2024>();
		m_wheelController_RR = childWheelColliders.transform.Find("WheelController_RR").GetComponent<WheelController2024>();
		m_wheelController_RL = childWheelColliders.transform.Find("WheelController_RL").GetComponent<WheelController2024>();
		m_layerMask = 1 << LayerMask.NameToLayer("Road");
	}
	// �ŏ��Ɉ�x�����Ă΂��֐�
	void Start()
	{
		Initialize();

		// �^�C���̋������v�Z
		CalcWheelLenge();
	}
	// �����ݒ�
	private void Initialize()
	{
		#region ������
		m_wheelBase = 0.0f;
		m_treadFront = 0.0f;
		m_treadRear = 0.0f;
		m_distace_Rear = 0.0f;
		m_langeHight = 0.0f;
		m_load_FR = 0.0f;
		m_load_FL = 0.0f;
		m_load_RR = 0.0f;
		m_load_RL = 0.0f;
		m_prevVelocity = Vector3.zero;
		m_graundNomal = Vector3.zero;
		#endregion
		m_vehicle = this.gameObject;
		m_vehicleController = m_vehicle.GetComponent<VehicleController>();
		m_vehicleRigitbody = m_vehicle.GetComponent<Rigidbody>();
		// �ԏd�̐ݒ�
		m_weight = m_vehicleRigitbody.mass * Physics.gravity.magnitude;
	}

	// ���t���[���X�V�����֐�
	void Update()
	{
		UpdateProcess();
	}

	// �؂�ւ���Ƃ��p Inspector�ŕς����悤�ɂ��Ă�����
	//private void FixedUpdate()
	//{
	//    UpdateProcess();
	//}

	private void UpdateProcess()
	{
		#region �Ԃ��������ݒ肳��Ă��邩�m�F
		if (m_vehicle.tag == null)  // Null�`�F�b�N
		{
			Debug.LogError("�׏d�ړ��Ŏg���Ԃ��ݒ肳��Ă��܂���/Load2024.cs/");

			// �׏d���l����
			m_load_FR = m_load_FL = m_load_RR = m_load_RL = m_weight / 4;
			m_wheelController_FR.Load = m_load_FR;
			m_wheelController_FL.Load = m_load_FL;
			m_wheelController_RR.Load = m_load_RR;
			m_wheelController_RL.Load = m_load_RL;
			return;
		}
		#endregion

		// Ray���΂��Ēn�ʂƂ̔��������
		RaycastToGraund();
		// �׏d�ړ��̌v�Z
		LoadTransfre();
		// �׏d�̕��z
		LoadApply();
	}

	#region �����v�Z
	// Ray���΂��Ēn�ʂƂ̔��������
	private void RaycastToGraund()
	{
		// ���݈ʒu�̈ʒu����Alocal�������ɁA���C�Ŏ擾����
		RaycastHit hit;
		Vector3 startpos = m_vehicleRigitbody.worldCenterOfMass;
		Debug.DrawRay(startpos, -transform.up * m_maxDistance, Color.green);
		if (Physics.Raycast(startpos, -transform.up, out hit, m_maxDistance, m_layerMask))
		{
			// hit�n�_�̖@���x�N�g��
			m_graundNomal = hit.normal;
			// �n�ʂ���̍��� (hit�ʒu + �d�S����)
			m_langeHight = hit.distance;
			//Debug.Log("hit�n�_�̖@���x�N�g�� :" + m_graundNomal);
			//Debug.Log("�n�ʂ���̍��� :" + m_langeHight);
		}
		else
		{
			m_langeHight = m_fly;

			Debug.LogError("�n�ʂɐG��Ă��܂���");
		}
	}

	// �^�C���̋������v�Z
	private void CalcWheelLenge()
	{
		// �z�C�[���x�[�X (|FR.localPosition.z| + |RR.localPosition.z|)
		m_wheelBase = Mathf.Abs(m_wheelController_FR.transform.localPosition.z) + Mathf.Abs(m_wheelController_RR.transform.localPosition.z);
		// �O�փg���b�g   (|FR.localPosition.z| + |FL.localPosition.z|)
		m_treadFront = Mathf.Abs(m_wheelController_FR.transform.localPosition.x) + Mathf.Abs(m_wheelController_FL.transform.localPosition.x);
		// ��փg���b�g   (|RR.localPosition.z| + |RL.localPosition.z|)
		m_treadRear = Mathf.Abs(m_wheelController_RR.transform.localPosition.x) + Mathf.Abs(m_wheelController_RL.transform.localPosition.x);
		// �d�S�����ւ܂ł̋���   (|RR.localPosition.z - CoG.Position.z|)
		m_distace_Rear = Mathf.Abs(m_wheelController_RR.transform.localPosition.z - m_vehicleRigitbody.centerOfMass.z);
		//Debug.Log("�z�C�[���x�[�X :" + m_wheelBase);
		//Debug.Log("�O�փg���b�h :" + m_treadFront);
		//Debug.Log("��փg���b�h :" + m_treadRear);
		//Debug.Log("�d�S�����ւ܂ł̋��� :" + m_distace_Rear);
	}
	#endregion

	#region �׏d�v�Z

	// @Link https://dskjal.com/car/math-car-weight.html
	//       https://www.autoexe.co.jp/kijima/column16.html
	//       https://www.autoexe.co.jp/kijima/column13.html
	//       https://suspensionsecrets.co.uk/lateral-and-longitudinal-load-transfer/

	// �׏d�ړ��̌v�Z
	private void LoadTransfre()
	{
		// �n�ʂɐݒu���Ă��Ȃ��Ƃ�
		if (m_langeHight == m_fly)
		{
			// �׏d���[����
			m_load_FR = m_load_FL = m_load_RR = m_load_RL = 0;
			m_wheelController_FR.Load = m_load_FR;
			m_wheelController_FL.Load = m_load_FL;
			m_wheelController_RR.Load = m_load_RR;
			m_wheelController_RL.Load = m_load_RL;
			return;
		}

		// ���x�̎擾
		m_velocity = m_vehicleRigitbody.linearVelocity;

		// �c�����̉׏d�ړ�
		LongitudinalLoadTransfer();
		// �������̉׏d�ړ�
		LateralLoadTransfer();

		// �׏d�������o��
		// �O
		m_load_FR *= m_Front_LoadRatio;
		m_load_FL *= m_Front_LoadRatio;
		// ��
		m_load_RR *= m_load_Rear;
		m_load_RL *= m_load_Rear;

		// ���x�̕ۑ�
		m_prevVelocity = m_velocity;
	}

	// �c�����̉׏d�ړ�
	private void LongitudinalLoadTransfer()
	{
		#region �v�Z�ɕK�v�ȕϐ�

		// �V�[�^
		// �Ԃ̃s�b�`�����̌X�� 
		// �n�ʂ̖@���x�N�g���ƎԂ̉������̃x�N�g���̊p�x�����߂�
		float theta = Mathf.Abs(Vector3.SignedAngle(-transform.up, -m_graundNomal, Vector3.forward));
		//Debug.Log("�s�b�`�V�[�^" + theta);

		// ���ʕ����x�N�g��
		Vector3 forward = gameObject.transform.forward;
		// ���ʕ����x�N�g���Ƒ��x�x�N�g���̓��ς𐳖ʕ����x�N�g���Ƃ����đO�����x�x�N�g�����擾
		Vector3 forwardVelocity = Vector3.Dot(m_velocity, forward) * forward;
		m_forwardSpeed = forwardVelocity.magnitude;

		Vector3 prevForwardVelocity = Vector3.Dot(m_prevVelocity, forward) * forward;
		// �����x [G]�@[Vf - V0f / t / g ]
		float acceleration = (forwardVelocity.magnitude - prevForwardVelocity.magnitude) / Time.deltaTime / Physics.gravity.magnitude;
		m_accel = acceleration; // �l�ێ�
								//Debug.Log("�����x" + acceleration);
		#endregion

		// �׏d�̑O�㊄���̌v�Z (�X�� - ������)
		m_Front_LoadRatio = LoadToLongitudinalTilt() - LoadToAcceleration();
		m_load_Rear = 1 - (LoadToLongitudinalTilt() - LoadToAcceleration());    // �S�̂̉׏d��100���Ƃ��đO�׏d�̔��]

		//Debug.Log("�O��X��" + LoadToLongitudinalTilt());
		//Debug.Log("������" + LoadToAcceleration());

		// �O��̌X���ɂ��׏d����
		float LoadToLongitudinalTilt()
		{
			// �قڌX�����Ȃ��ꍇ
			if (theta < m_minAngle)
			{
				// ����
				return 0.5f;
			}
			// �X���������𒴂����ꍇ
			else if (theta > m_maxAngle)
			{
				Debug.LogError("�X������");
				return 0f; // �׏d�Ȃ�
			}
			// �X���������ȏ�̎�
			/* �v�Z��
             * L�F�z�C�[���x�[�X[m]
             * lr�F�d�S�����ւ܂ł̋���[m]
             * h�F�n�ʂ���d�S�܂ł̍���[m]
             * ��:�n�ʂ̌X��[rad]
             * 
             * FR = (lr - h��)/L
             */

			// �O�ւɂ�����׏d����
			return ((m_distace_Rear - m_langeHight * (theta * Mathf.Deg2Rad)) / m_wheelBase);
		}

		// �������ɂ��׏d����
		float LoadToAcceleration()
		{
			// �قډ��������Ȃ��ꍇ
			if (Mathf.Abs(acceleration) < m_minAcceleration)
			{
				return 0f;   // �؂�̂�
			}
			// �������������𒴂����ꍇ
			else if (Mathf.Abs(acceleration) > m_maxAcceleration)
			{
				//Debug.Log("�������I�[�o�["+ acceleration);
				// �����l
				return 0.5f;
			}


			/* �v�Z��
             * L�F�z�C�[���x�[�X[m]
             * h�F�n�ʂ���d�S�܂ł̍���[m]
             * Ax�F�c���������x[G]
             * 
             * FrontLoad = h/L * Ax
             */

			// �O�ւɂ�����׏d����
			return (m_langeHight / m_wheelBase) * acceleration;
		}
	}



	// �������̉׏d�ړ�
	private void LateralLoadTransfer()
	{
		#region �v�Z�ɕK�v�ȕϐ�

		// �V�[�^
		// �Ԃ̃��[�������̌X���̎擾
		float theta = Mathf.Abs(Vector3.SignedAngle(-transform.up, -m_graundNomal, Vector3.left));
		//Debug.Log("���[���V�[�^" + theta);

		// �E�����x�N�g��
		Vector3 sideway = gameObject.transform.right;
		// �E�����x�N�g���Ƒ��x�x�N�g���̓��ς��E�����x�N�g���Ƃ����ĉE���x�x�N�g�����擾
		Vector3 sidewayVelocity = Vector3.Dot(m_velocity, sideway) * sideway;
		Vector3 prevSidewayVelocity = Vector3.Dot(m_prevVelocity, sideway) * sideway;

		// ���S�́i���S�����x�j [G] [Vs - V0s / t / g ]
		// �R���g���[���[�̓��͂ɂ���č��E���f �܂��@���S�͂Ȃ̂Ŕ��]
		float centrifugalForce = -1 * Mathf.Sign(m_vehicleController.Steering) * (sidewayVelocity.magnitude - prevSidewayVelocity.magnitude) / Time.deltaTime / Physics.gravity.magnitude;
		m_centrifugal = centrifugalForce;   // �l�ێ�
											//Debug.Log("���S��" +  centrifugalForce);
		#endregion

		// FR�ɂ�����׏d
		m_load_FR = LoadToLateralTilt(m_treadFront) - CentrifugalForceLoad(m_treadFront);
		// FL�ɂ�����׏d
		m_load_FL = 1 - (LoadToLateralTilt(m_treadFront) - CentrifugalForceLoad(m_treadFront));     // �S�̂̉׏d��100���Ƃ��ĉE�׏d�̔��]

		// RR�ɂ�����׏d
		m_load_RR = LoadToLateralTilt(m_treadRear) - CentrifugalForceLoad(m_treadRear);
		// RL�ɂ�����׏d
		m_load_RL = 1 - (LoadToLateralTilt(m_treadRear) - CentrifugalForceLoad(m_treadRear));

		//Debug.Log("���E�@�O �X��" + LoadToLateralTilt(m_treadFront));
		//Debug.Log("���E�@�O ���S��" + CentrifugalForceLoad(m_treadFront));
		//Debug.Log("���E�@�� �X��" + LoadToLateralTilt(m_treadRear));
		//Debug.Log("���E�@�� ���S��" + CentrifugalForceLoad(m_treadRear));

		// ���E�̌X���ɂ��׏d����
		float LoadToLateralTilt(float _tread)
		{
			// �قڌX�����Ȃ��ꍇ
			if (theta < m_minAngle)
			{
				// ����
				return 0.5f;
			}
			// �X���������𒴂����ꍇ
			else if (theta > m_maxAngle)
			{
				Debug.LogError("�X������");
				return 0f; // �׏d�Ȃ�
			}

			/* �v�Z��
             * t�F�g���b�h[m]
             * h�F�n�ʂ���d�S�܂ł̍���[m]
             * �ƁF�Ԃ̌X��[��]
             * 
             * FR/RR =  (t/2-h*tan��) /t
             */

			return (_tread / 2 - m_langeHight * (theta * Mathf.Deg2Rad)) / _tread;
		}

		// ���S�͂ɂ��׏d����
		float CentrifugalForceLoad(float _tread)
		{
			// �قډ��S�͂��Ȃ��ꍇ
			if (Mathf.Abs(centrifugalForce) < m_minCentrifugalForce)
			{
				return 0f; // �؂�̂�
			}
			// ���S�͂������𒴂����ꍇ
			else if (Mathf.Abs(centrifugalForce) > m_maxCentrifugalForce)
			{
				//Debug.Log("���S�̓I�[�o�["+ centrifugalForce);
				// �����l
				return 0.5f;
			}

			/* �v�Z��
             * t�F�g���b�h[m]
             * h�F���[���Z���^�[����d�S�܂ł̍���[m]
             * Ay�F�����������́i���S�́j[G]
             * 
             * FR/RR = (Ay*h)/ t
             * 
             */

			return centrifugalForce * m_rollcenterLangth / _tread;
		}
	}
	#endregion

	// �׏d�̕��z
	private void LoadApply()
	{
		// �d�ʂɊ����������Ēl��n��
		m_wheelController_FR.Load = m_weight * Mathf.Clamp(m_load_FR, m_minLoadPer, m_maxLoadPer);
		m_wheelController_FL.Load = m_weight * Mathf.Clamp(m_load_FL, m_minLoadPer, m_maxLoadPer);
		m_wheelController_RR.Load = m_weight * Mathf.Clamp(m_load_RR, m_minLoadPer, m_maxLoadPer);
		m_wheelController_RL.Load = m_weight * Mathf.Clamp(m_load_RL, m_minLoadPer, m_maxLoadPer);
	}
}
