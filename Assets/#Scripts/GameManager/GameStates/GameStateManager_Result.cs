using UnityEngine;
using UnityEngine.InputSystem;

public class GameStateManager_Result: GameStateManagerBase
{
    [SerializeField]
    AsyncSceneChanger m_sceneChanger;

	[SerializeField]
	Line_StartFinish m_finishLine;

	[SerializeField]
    GameObject m_resultPrefab;

    [SerializeField]
    ScoringUIController m_ScoringUIController;

	AxisPressdOnce m_pressedOnce = new AxisPressdOnce();

	[SerializeField, ShowInInspector]
    bool m_allowChangeScene = false;

    public override void Initialize()
    {
		SoundManager.Instance.PlayBGM(SoundManager.BGM_Type.Result);

        // ResultオブジェクトをActive化
        m_resultPrefab.SetActive(true);
        ScoreManager.Instance.ScoreText = GameManager.Instance.ResultTime;
        // Result表示
        m_ScoringUIController.ShowScoringScreen();
    }

    public override void StateUpdate()
    {
		if (m_finishLine.IsChecked)
		{
			// 入力時
			if (m_pressedOnce.PressedOnce)
			{
				// シーン切り替え
				OnChangeScene();
			}
		}
		Debug.Log("StateUpdate[Result]");
    }

    public void OnChangeScene()
    {
        //if(m_allowChangeScene)
        //{
			m_sceneChanger.ChangeScene();
			Debug.Log("OnChangeScene");
		//}
    }

	public void OnPedal(InputAction.CallbackContext _context)
	{
		var value = _context.ReadValue<float>();
		value = 1 - (value + 1) / 2;
		m_pressedOnce.AxisCheck(value);
	}
}
