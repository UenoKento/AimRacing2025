using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class GameStateManager_End : GameStateManagerBase
{
	[SerializeField]
    AsyncSceneChanger m_sceneChanger;

    [SerializeField]
    TMP_Text ResultTime;

	// 椅子
	WIZMOController m_WIZMOController;
	ChairRideOperator m_rideOff = new ChairRideOperator();

    [SerializeField]
    FadeManager m_FadeManager;

    [SerializeField]
    float m_sceneChangeTime = 14f;
    float m_timeCounter;

    AxisPressdOnce m_pressedOnce = new AxisPressdOnce();

    //[SerializeField]
    //ScoringUIController m_ScoringUIController;

    public override void Initialize()
    {
        // 次のシーンのロードを開始
        m_sceneChanger.StartLoad();

        m_WIZMOController = GameManager.Instance.WIZMO;

        m_FadeManager.PlayFadeIn();

        m_timeCounter = 0;

		// Result表示
		//ScoreManager.Instance.ScoreText = GameManager.Instance.ResultTime;
		//ResultTime.text = GameManager.Instance.ResultTime;
		ResultTime.text = GameManager.Instance.ResultTime;
		//m_ScoringUIController.ShowScoringScreen();
	}

    public override void StateUpdate()
    {
        if(m_WIZMOController != null)
            m_rideOff.RideOff(m_WIZMOController);

        m_timeCounter += Time.deltaTime;

        if(m_timeCounter > m_sceneChangeTime || m_pressedOnce.PressedOnce)
        {
            m_FadeManager.PlayFadeOut();
        }

        // フェードアウト終了時
        if (m_FadeManager.FadeOutComplete)
        {
            OnChangeScene();
        }

        //Debug.Log("StateUpdate[END]");
    }

    public void OnPedal(InputAction.CallbackContext _context)
    {
        var value = _context.ReadValue<float>();
        value = 1 - (value + 1) / 2;
        m_pressedOnce.AxisCheck(value);
    }

    public void OnChangeScene()
    {
        SoundManager.Instance.StopBGM();
        m_sceneChanger.ChangeScene();
        Debug.Log("ChangeScene[from END]");
    }
}