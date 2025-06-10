using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GameStateManager_Menu : GameStateManagerBase
{
	[SerializeField]
	AsyncSceneChanger m_sceneChanger;

	[SerializeField]
	UnityEvent m_onSteerRightEvent;
	[SerializeField]
	UnityEvent m_onSteerLeftEvent;

	[SerializeField]
	AxisPressdOnce m_pressedOnce = new AxisPressdOnce();
	// �E���̓���͈�
	[SerializeField,Range(0.1f,1f)]
	float m_steerRange;

	[SerializeField]
	FadeManager m_fadeManager;

	[SerializeField, ShowInInspector]
	DrivingSettings m_selectedSettings;

	bool m_isSelected = false;

	bool m_triger = false;

	public override void Initialize()
	{
		SoundManager.Instance.PlayBGM(SoundManager.BGM_Type.Entry);
		m_fadeManager.PlayFadeIn();
		m_sceneChanger.StartLoad();
		m_selectedSettings.isAT = true;
	}

	public override void StateUpdate()
	{
		if (!m_fadeManager.FadeInComplete) return;
		// ���͎�
		if (m_pressedOnce.PressedOnce || m_triger)
		{
			// �t�F�[�h�A�E�g�J�n
			m_fadeManager.PlayFadeOut();
		}
		// �t�F�[�h�A�E�g�I����
		if (m_fadeManager.FadeOutComplete)
		{
			// �V�[���؂�ւ�
			OnChangeScene();
			SoundManager.Instance.StopBGM();
		}

		//Debug.Log("StateUpdate[Menu]");
	}

	public void OnTriger()
    {
		m_triger = true;

    }

	public void OnChangeScene()
	{
		m_sceneChanger.ChangeScene();
		GameManager.Instance.DrivingSettings = m_selectedSettings;
		Debug.Log("ChangeState[from Menu]");
	}

	public void OnPedal(InputAction.CallbackContext _context)
	{
		var value = _context.ReadValue<float>();
		value = 1 - (value + 1) / 2;

		// �I�΂��܂ŃA�N�Z��������
		if(m_isSelected)
			m_pressedOnce.AxisCheck(value);
	}

	public void OnSelect(InputAction.CallbackContext _context)
	{
		var value = _context.ReadValue<float>();

		// �A�N�Z�������܂ꂽ�珈�����Ȃ�
		if (m_pressedOnce.PressedOnce)
			return;

		if (value > m_steerRange)
		{
			m_onSteerRightEvent.Invoke();
			m_isSelected = true;
			m_selectedSettings.isAT = false;
		}
		else if (value < -m_steerRange)
		{
			m_onSteerLeftEvent.Invoke();
			m_isSelected = true;
			m_selectedSettings.isAT = true;
		}
	}

}
