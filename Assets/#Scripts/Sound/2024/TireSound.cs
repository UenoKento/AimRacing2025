using FMODUnity;
using UnityEngine;

public class TireSound : MonoBehaviour
{
    [SerializeField]
    WheelController2024 m_wheelController;

    [SerializeField]
    EventReference m_eventName;

    FMOD.Studio.EventInstance m_tireEventInst;

    [SerializeField]
    Vector2 m_volRange;
    [SerializeField]
    Vector2 m_pitchRange;
    [SerializeField,ShowInInspector]
    float m_slipParam;
    [SerializeField, ShowInInspector]
    float m_lat;

    float m_overrideSlip;

	#region �v���p�e�B
    public float OverrideSlip
    {
        set => m_overrideSlip = value;
    }

	#endregion

	void Reset()
	{
        TryGetComponent(out m_wheelController);
	}

	void Awake()
    {
        // �C���X�^���X�쐬
        m_tireEventInst = FMODUnity.RuntimeManager.CreateInstance(m_eventName);
        RuntimeManager.AttachInstanceToGameObject(m_tireEventInst, m_wheelController.transform);

        m_tireEventInst.start();
    }

    void FixedUpdate()
    {
        // �X���b�v�x���͏c�����̃X���b�v���x�Ɖ������̑��x�̃x�N�g���̑傫���Ō��߂�
        Vector2 slipCircle = Vector2.zero;
        slipCircle.x = m_wheelController.SlipRatio;
        slipCircle.y = m_wheelController.LatSpeed;

		// �X���b�v�p�����[�^��ݒ�
		m_slipParam = slipCircle.magnitude;

        if (m_overrideSlip > float.Epsilon)
            m_slipParam = m_overrideSlip;

		m_lat = m_wheelController.LatSpeed;

		float volume = Mathf.InverseLerp(m_volRange.x, m_volRange.y, m_slipParam);
        float pitch = Mathf.InverseLerp(m_pitchRange.x, m_pitchRange.y, m_slipParam);

        // �n�ʂɐڐG���Ă��Ȃ��Ƃ��͐؂�
        if (!m_wheelController.IsGround)
            volume = 0f;

        m_tireEventInst.setParameterByName("SLIP_VOL", volume);
        m_tireEventInst.setParameterByName("SLIP_PITCH", pitch);
	}

    void OnDestroy()
    {
        m_tireEventInst.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        m_tireEventInst.release();
    }
}
