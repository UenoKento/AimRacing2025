using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class DebugDevice : SingletonMonoBehaviour<DebugDevice>
{
    [SerializeField]
    float m_refreshInterval = 3f;

    const int m_maxDeviceNum = 30;

    string logText = "";
    GUIStyle guiStyle = new GUIStyle();

    bool m_showLogInGame = false;

    float m_lastRefreshTime;

    #region �v���p�e�B
    public bool ShowLogInGame
    {
        get => m_showLogInGame;
        set => m_showLogInGame = value;
    }
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // ���O�̃e�L�X�g���X�^�C���ɐݒ�
        guiStyle.fontSize = 24;
        guiStyle.normal.textColor = Color.green;

        m_showLogInGame = false;
    }

    private void OnGUI()
    {
        // �Q�[����ʒ��Ƀ��O��\���iWindows�̃r���h���̂ݗL�����G�f�B�^�Ŏ��s���Ă��Ȃ��ꍇ�̂ݗL������0�L�[�ŕ\��/��\����؂�ւ��j
#if UNITY_STANDALONE_WIN
        if (m_showLogInGame)
        {
            GUI.Label(new Rect(Screen.width - 256, 10, 256, Screen.height), logText, guiStyle);
        }
#endif
    }


    // Update is called once per frame
    void Update()
    {
        if(m_showLogInGame && Time.time - m_lastRefreshTime > m_refreshInterval)
        {
            RefreshText();
            m_lastRefreshTime = Time.time;
        }
    }

    /// <summary>
    /// InputSystem�̃f�o�C�X��񋓂�text�ɑ��
    /// </summary>
    void RefreshText()
    {
        logText = "";

        // InputSystem����f�o�C�X���擾
        var devices = InputSystem.devices;
        // �C���f�b�N�X�ƃf�o�C�X�̖��O��logText�ɒǉ�
        for (int i = 0; i < devices.Count; ++i)
        {
            logText += (i + ":" + devices[i].name + "\n");
        }

        // �\�����郍�O�̍s����MaxLogLines�𒴂�����A�Â����O���폜
        string[] logLines = logText.Split('\n');
        if (logLines.Length > m_maxDeviceNum)
        {
            logText = string.Join("\n", logLines, logLines.Length - m_maxDeviceNum, m_maxDeviceNum);
        }
    }
}
