using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.InputSystem;

public class GameStateManager_OP : GameStateManagerBase
{
    [SerializeField]
    AsyncSceneChanger m_sceneChanger;
	[SerializeField]
	FadeManager m_fadeManager;
	[SerializeField]
	PlayableDirector m_director;

	bool m_triger = false;

	AxisPressdOnce m_pressedOnce = new AxisPressdOnce();

	bool m_changed = false;

	// 椅子
	WIZMOController m_WIZMOController;
	ChairRideOperator m_gettingOn = new ChairRideOperator();

	public override void Initialize()
    {
        // 次のシーンのロードを開始
        m_sceneChanger.StartLoad();
		m_fadeManager.PlayFadeIn();
		Music.Play("Music_OP");
		//SoundManager.Instance.PlayBGM(SoundManager.BGM_Type.OP);
		m_director.Play();
		m_WIZMOController = GameManager.Instance.WIZMO;
	}

    public override void StateUpdate()
    {
		if(!m_changed)
		{
			if (m_director != null)
				m_director.time = Music.CurrentMeter.GetSecondsFromTiming(Music.Just) + Music.SecFromJust;
			else
				Debug.LogError("StateOP m_director is Null!");
		}
			

		if (!m_fadeManager.FadeInComplete)
		{
			//Debug.Log("Op:FadeComplete false");
			return;
		}

		// 入力時
		if (m_pressedOnce.PressedOnce || m_triger)
		{
			if(!m_changed)
			{
				// フェードアウト開始
				m_fadeManager.PlayFadeOut();
				Music.SetHorizontalSequence("End");
				SoundManager.Instance.PlaySE(SoundManager.SE_Type.OPPushSE);
				//Debug.Log("OP::pressedOnce true");
				m_changed = true;
			}
			// 椅子を乗車位置にセッティング
			if (m_WIZMOController != null)
				m_gettingOn.Ride(m_WIZMOController);

		}
		else
		{
			//Debug.Log("OP::pressedOnce flase");
		}

		// フェードアウト終了時
		if(m_fadeManager.FadeOutComplete)
		{
			// シーン切り替え
			OnChangeScene();
		}

		//Debug.Log("StateUpdate[OP]");
    }

	public void OnPedal(InputAction.CallbackContext _context)
	{
		var value = _context.ReadValue<float>();
		value = 1 - (value + 1) / 2;
		m_pressedOnce.AxisCheck(value);
	}

	public void OnTriger()
    {
		m_triger = true;
    }

	public void OnChangeScene()
    {
		
        m_sceneChanger.ChangeScene();
		Debug.Log("ChangeScene[from OP]");
    }
}