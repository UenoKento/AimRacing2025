using UnityEngine;

public class GameStateManager_Splash : GameStateManagerBase
{
    [SerializeField]
    AsyncSceneChanger m_sceneChanger;

    [SerializeField]
    SplashFade splashFade;

	// 椅子
	WIZMOController m_WIZMOController;
	ChairRideOperator m_rideOff = new ChairRideOperator();

	public override void Initialize()
    {
        // 次のシーンのロードを開始
        m_sceneChanger.StartLoad();

		m_WIZMOController = GameManager.Instance.WIZMO;
	}

    public override void StateUpdate()
    {
		// 椅子を乗車位置に移行。
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