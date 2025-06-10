using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    // ���݂̃X�e�[�g
    [SerializeField]
    GameState m_currentGameState;
    [SerializeField,ShowInInspector]
    GameStateManagerBase m_currentStateManager;

	// �v���^�C���̃e�L�X�g
	[SerializeField, ShowInInspector]
	string m_resultTime;

	[SerializeField]
	WIZMOController m_WIZMOController;

    [SerializeField]
    WIZMODebug m_WIZMODebug;

	// �^�]�ݒ�
	[SerializeField,ShowInInspector]
	DrivingSettings m_settings;

    int m_steeringIndex = 0;
    

    bool m_isAllUnloading;
    public bool IsAllUnloading => m_isAllUnloading;

	float m_steerInput;

    #region �v���p�e�B
    public GameState CurrentGameState => m_currentGameState;

	public string ResultTime
	{
		get => m_resultTime;
		set => m_resultTime = value;
	}

    public DrivingSettings DrivingSettings
    {
        get => m_settings;
        set => m_settings = value;
    }

    public bool ShowFPS
    {
        get; set;
    }
    public bool ShowStateWIZMO
    {
        get; set;
    }
    public bool ShowMouce
    {
        get; set;
    }


    public WIZMOController WIZMO => m_WIZMOController;

	#endregion

	// Start is called before the first frame update
	protected override void Awake()
    {
        base.Awake();
	}

	private void OnDestroy()
	{
        //LogitechGSDK.LogiSteeringShutdown();
	}

    private void Start()
    {
        // �}�E�X�J�[�\����\��
        Cursor.visible = false;

		m_settings.isAT = true;


		// �n���R��������
		//Debug.Log("LogiSteeringInit::" + LogitechGSDK.LogiSteeringInitialize(false));
    }

    // Update is called once per frame
    void Update()
    {
        // ���݂̃X�e�[�g�}�l�[�W���[���A�b�v�f�[�g
        m_currentStateManager?.StateUpdate();

		// �C���Q�[�����ȊO�̃t�H�[�X�t�B�[�h�o�b�N
		//if (m_currentStateManager.state != GameState.Ingame)
		//{
		//	float constantMag = Mathf.InverseLerp(0f, 0.8f, Mathf.Abs(m_steerInput)) * Mathf.Sign(m_steerInput);

		//	if (LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(0))
		//		LogitechGSDK.LogiPlayConstantForce(0, (int)(constantMag * 50f));
		//}


        // �f�o�b�O�L�[
        if(ShowFPS)
	        Debug.Log("FPS::[" + 1f / Time.deltaTime + "]");
        if(ShowStateWIZMO)
            Debug.Log("WIZMO::" + m_WIZMODebug.GetState_WIZMO());
        Cursor.visible = ShowMouce;
    }

    public void ChangeState(GameStateManagerBase _newStateManager)
    {
        m_currentStateManager = _newStateManager;
        m_currentGameState = _newStateManager.state;
        _newStateManager.Initialize();
    }

    void UpdateSteerInput(InputAction.CallbackContext _context)
    {
		m_steerInput = _context.ReadValue<float>();
	}

    public void ReLoadingScene(in string _reloadingScene)
    {
        if(_reloadingScene != null)
        {
            if (m_isAllUnloading)
                return;

            StartCoroutine(UnloadAllSceneExceptOne(_reloadingScene));
		}    
    }


    // TODO �֐����ʂ�̏����ɂ���
    IEnumerator UnloadAllSceneExceptOne(string _reloadingScene)
    {
        m_isAllUnloading = true;

        yield return new WaitForSeconds(1f);
        for(int i = 0; i < SceneManager.sceneCount; i++) 
        {
            Scene index = SceneManager.GetSceneAt(i);
            if (gameObject.scene != index)
            {
                SceneManager.UnloadSceneAsync(index);
            }
		}

        m_isAllUnloading = false;

        yield return null;
        SoundManager.Instance.StopBGM();
        SceneManager.LoadSceneAsync(_reloadingScene, LoadSceneMode.Additive);
    }
}
