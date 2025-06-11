using UnityEngine;
using static VehiclePhysics.VPTelemetry;

/**
 * @file    Suspension.cs
 * @brief   �T�X�y���V����(�Ռ������A�׏d�v�Z)�Ɋւ���v�Z
 * @author  23CU0110 ������@���P
 * @date    2025/04/25  �쐬
 *          2025/04/25 �ŏI�X�V
 */

public class Suspension : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    WheelController2024 m_WheelController;  //�z�C�[���R���g���[���[

    //WheelController����擾������
    RaycastHit m_raycastHit;                //���C�L���X�g���
    float m_WheelRadius;                    //�z�C�[�����a

    [SerializeField]
    Rigidbody m_CarRigid;                   //�Ԃ�rigidbody���

    [SerializeField]
    Transform m_Car_Visualtransform;        //���b�V���̈ʒu���



    [Header("Suspension_Settings")]
    [SerializeField]
    float m_spring = 50000.0f;              // �^�[�Q�b�g�ʒu�ɓ��B���邽�߂̃o�l��
    [SerializeField]
    float m_damper = 10000.0f;              // �o�l�̐U���������������(�_���p�[��)
    [SerializeField]
    float m_suspensionDistance = 0.05f;     // �T�X�y���V�����̍ő�L������(���[�J�����W)


    float Spring
    {
        get
        {
            return m_spring;
        }
        set
        {
            // 0�ȏ�ɕ␳
            m_spring = Mathf.Max(0, value);
        }
    }
    float Damper
    {
        get
        {
            return m_damper;
        }
        set
        {
            // 0�ȏ�ɕ␳
            m_damper = Mathf.Max(0, value);
        }
    }

    [Header("SuspensionLoad")]
    [SerializeField, ShowInInspector]
    float m_suspensionLoad;                  // �T�X�y���V��������v�Z�����㉺�׏d


    void Start()
    {
        // �����׏d(�l����)
        m_suspensionLoad = m_CarRigid.mass / 4f * Physics.gravity.magnitude;

        m_WheelRadius = m_WheelController.GetWheelRadius();

    }

    void FixedUpdate()
    {

        m_raycastHit = m_WheelController.GetRayCastHit();

        if (m_WheelController.GetOnGround())
        {
            UpdateSuspension();
        }
    }

    void UpdateSuspension()
    {
        // ���[�J���̉����������[���h�ɕϊ�
         Vector3 down = transform.TransformDirection(Vector3.down);

        // �ԗւ̉�]���l�������ɁA�ԗւ��n�ʂɑ΂��Ăǂ̂��炢�̑����œ����Ă��邩���v�Z����B
        Vector3 velocityAtTouch = m_CarRigid.GetPointVelocity(m_raycastHit.point);

        // �X�v�����O�̈��k���v�Z����
        // �ʒu�̍����T�X�y���V�����̑S�͈͂Ŋ���
        float compression = m_raycastHit.distance / (m_suspensionDistance + m_WheelRadius);
        //Debug.Log("01 compression : " + compression);
        compression = -compression + 1;
        //Debug.Log("02 compression : " + compression);

        // �ŏI�I�ȗ�
        Vector3 force = -down * compression * Spring;
        //Debug.Log("force : " + force);

        // �ڐG�_�̑��x�����[�J����Ԃɕϊ���������
        Vector3 Suspension_LocalVelocity = transform.InverseTransformDirection(velocityAtTouch);
        //Debug.Log("t : " + t);

        // ���[�J��X����сAZ���� = 0
        // �����ŁAt�̓V���b�N�����k/�c�����鑬�x�Ɠ������Ƃ���B
        Suspension_LocalVelocity.z = 0;
        Suspension_LocalVelocity.x = 0;

        // ���[���h��� * ����
        // ���̗͂̓T�X�y���V�����̖��C�ɂ��͂��V�~�����[�g���Ă��܂��B
        Vector3 shockDrag = transform.TransformDirection(Suspension_LocalVelocity) * -Damper;

        // 
        m_CarRigid.AddForceAtPosition(force + shockDrag, transform.position);
        m_suspensionLoad = (force + shockDrag).magnitude;

        m_Car_Visualtransform.position = transform.position + (down * (m_raycastHit.distance - m_WheelRadius));
    }



    public float GetSuspensionDistance()
    {
        return m_suspensionDistance;
    }

    public float GetSuspensionLoad()
    {
        return m_suspensionLoad;
    }

    public void SetSuspensionLoad(float LoadValue)
    {
        m_suspensionLoad = LoadValue;
    }

}
