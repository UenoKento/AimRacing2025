using UnityEngine;

public class GameStateManager_Splash : GameStateManagerBase
{
    [SerializeField]
    AsyncSceneChanger m_sceneChanger;

    [SerializeField]
    SplashFade splashFade;

	// �֎q
	WIZMOController m_WIZMOController;
	ChairRideOperator m_rideOff = new ChairRideOperator();

	public override void Initialize()
    {
        // ���̃V�[���̃��[�h���J�n
        m_sceneChanger.StartLoad();

		m_WIZMOController = GameManager.Instance.WIZMO;
	}

    public override void StateUpdate()
    {
		// �֎q����Ԉʒu�Ɉڍs�B
		if (m_WIZMOController != null)
			m_rideOff.RideOff(m_WIZMOController);

		if (splashFade.IsEndAnimation)
            ChangeScene();

		//Debug.Log("StateUpdate[Splash]");
    }

    public void ChangeScene()
    {
        m_sceneChanger.ChangeScene();
        Debug.Log("ChangeScene[from Splash]");
    }
}