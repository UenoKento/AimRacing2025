using UnityEngine;
using UnityEngine.Playables;

public class GameStateManager_Movie : GameStateManagerBase
{
    [SerializeField]
    AsyncSceneChanger m_sceneChanger;

    [SerializeField]
    PlayableDirector m_moviePlay;

	// 椅子
	WIZMOController m_WIZMOController;
	ChairRideOperator m_drive = new ChairRideOperator();

    [SerializeField]
    VehicleController m_vehicle;

    [SerializeField]
    FadeManager m_fadeManager;

    [SerializeField]
    UI_StarCountDown m_startAction;

    [SerializeField]
    DriveSound m_driveSound;

    bool m_isSkipped = false;

	public override void Initialize()
    {
        // 次のシーンのロードを開始
        m_sceneChanger.StartLoad();
		SoundManager.Instance.StopBGM();
		//SoundManager.Instance.PlayBGM(SoundManager.BGM_Type.InGame);

		m_WIZMOController = GameManager.Instance.WIZMO;

		m_fadeManager.PlayFadeIn();

        m_moviePlay.Play();

        Music.Play("BGM_Map");

        m_vehicle.PullUp(true);

        m_startAction.enabled = false;
        m_driveSound.enabled = false;

		if (m_WIZMOController != null)
			m_drive.Drive(m_WIZMOController);

	}

	public override void StateUpdate()
    {
        // ムービースキップ
        if(Input.GetKeyDown(KeyCode.PageDown))
        {
            if(!m_isSkipped)
            {
                m_moviePlay.time = m_moviePlay.duration - 0.1;
                m_isSkipped = true;
            }
        }

        if(!m_isSkipped)
            m_moviePlay.time = Music.MusicalTime;

       if(m_moviePlay.time >= m_moviePlay.duration)
        {
            GameStateManagerBase ingameState = FindAnyObjectByType<GameStateManager_InGame>();
            GameManager.Instance.ChangeState(ingameState);
            m_startAction.enabled = true; 
            m_driveSound.enabled = true;
		}
    }

    public void OnChangeScene()
    {
       
    }
}