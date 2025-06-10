using System.Collections;
using UnityEngine;

public class GameStateManager_InGame : GameStateManagerBase
{
	[SerializeField]
	VehicleController2024 m_vehicle;

    [SerializeField]
    AsyncSceneChanger m_sceneChanger;

    [SerializeField]
    Line_StartFinish m_finishLine;

	[SerializeField]
	ChairController2024 m_chairController;

    [SerializeField]
	FadeManager m_fadeManager;

	// �h���}�p
	[SerializeField]
	GameObject m_goalImage;

	Coroutine coroutine;

	int _state = 0;
	public override void Initialize()
    {
        // ���̃V�[���̃��[�h���J�n
        //m_sceneChanger.StartLoad();

		//m_fadeManager.PlayFadeIn();

		// �֎q�N��
		m_chairController.InGameMove = true;

		// �Ԓ�~
		m_vehicle.PullUp(true);

		// �~�b�V�����ݒ�
		if (GameManager.Instance.DrivingSettings.isAT)
			m_vehicle.Transmission.Type = Transmission2024.TransmissionType.Automatic;
		else
			m_vehicle.Transmission.Type = Transmission2024.TransmissionType.Manual;

		m_goalImage.SetActive(false);
	}

    public override void StateUpdate()
    {
		// AT/MT�؂�ւ��p�f�o�b�O�L�[ F5
		if (Input.GetKeyDown(KeyCode.F5))
			m_vehicle.ChangeMissionType();

		// �S�[����
        if(m_finishLine.IsChecked)
        {
			if (_state == 0)
			{
				Music.Resume();
				// �S�[��SE
				SoundManager.Instance.PlaySE(SoundManager.SE_Type.Goal);

				_state = 1;

				if (coroutine != null) { return; }

				// �S�[���̕�����\��
				coroutine = StartCoroutine(a(0.0f,1.0f));


			}

			// �֎q��~
			m_chairController.InGameMove = false;
		}
		// �t�F�[�h�A�E�g�I����
		if (m_fadeManager.FadeOutComplete)
		{
			SoundManager.Instance.PlayBGM(SoundManager.BGM_Type.Result, false, 0.5f);
			ChangeState();
		}
		//Debug.Log("StateUpdate[InGame]");
    }

	public void ChangeState()
    {
		m_sceneChanger.ChangeScene();
		Debug.Log("ChangeState[from Ingame]");
    }

	IEnumerator a(float waitTime_1st ,float waitTime_2nd)
	{
		yield return new WaitForSeconds(waitTime_1st);
		m_goalImage.SetActive(true);

		yield return new WaitForSeconds(waitTime_2nd);
		// �t�F�[�h�A�E�g�J�n
		m_fadeManager.PlayFadeOut();

	}
}