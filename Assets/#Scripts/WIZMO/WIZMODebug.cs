/*
 * @file    WIZMODebug.cs
 * @brief   �֎q��Debug�V�X�e��     
 * @author  22CU0225 �L�c�B��
 * @date    2024/10/03 �쐬
 */
using UnityEngine;
using UnityEngine.SceneManagement;

public class WIZMODebug : MonoBehaviour
{
    // WIZMO
    [SerializeField]
    WIZMOController m_WIZMOController;

    [SerializeField, ShowInInspector]
    private bool m_isEmergencyShutdown = false;   // �ً}��~

    public bool isEmergencyShutdown => m_isEmergencyShutdown;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (EmergencyShutdown()) return;
    }

    //������������������������������������������
    // �֎q�Ƃ̐ڑ��m�F
    //������������������������������������������
    private void CheckChairConnect()
    {
        // �R���g���[���[��������Ȃ��ꍇ�͂��̌�̏��������Ȃ�
        if (m_WIZMOController == null) return;

        // �֎q������ɓ����Ă��邩���`�F�b�N
        if (m_WIZMOController.GetState() != WIZMOController.Running || m_WIZMOController.GetState() != WIZMOController.Initial )
        {
            // ����ɓ����Ă��Ȃ��ꍇ������~
            m_WIZMOController.CloseSIMVR();
        }
    }

    //������������������������������������������
    // �֎q�ً̋}��~�̊֐�
    // �߂�l�F�g���Ă����ꍇ��true�A
    // �@�@�@�@����ȊO��false
    //������������������������������������������
    public bool EmergencyShutdown()
    {
        // �ً}��~
        // �L�[�������ꂽ��A�ً}��~���܂�
        if (Input.GetKeyDown(KeyCode.F11))
        {
            m_isEmergencyShutdown = true;

            m_WIZMOController.CloseSIMVR();
            Debug.LogError("ShutDown");
            return true;
        }

        // �ʏ���
        // �L�[�������ꂽ��A�Đڑ����܂�
        if (Input.GetKeyDown(KeyCode.F12))
        {
            m_isEmergencyShutdown = false;

            // �l��߂�
            m_WIZMOController.OpenSIMVR();
            Debug.LogError("Active");
            return false;
        }

        if (m_isEmergencyShutdown) return true;
        return false;
    }

    // WIZMO LOG
    public string GetState_WIZMO()
    {
        string log;
        int state = m_WIZMOController.GetState();

        switch (state)
        {
            case WIZMOController.CanNotFindUsb:
                log = "USB���ڑ�����Ă��܂��� [0:MachineNotDetected]";
                break;
            case WIZMOController.CanNotFindSimvr:
                log = "SIMVR��������܂��� [1:InaccessibleToTheMachine]";
                break;
            case WIZMOController.CanNotCalibration:
                log = "SIMVR�̌��_���A�Ɏ��s���܂��� [2:ZeroReturnFailure]";
                break;
            case WIZMOController.TimeoutCalibration:
                log = "SIMVR�̌��_���A���ɃG���[���������܂���[3:ErrorReturningToZero]";
                break;
            case WIZMOController.ShutDownActuator:
                log = "�A�N�`���G�[�^����~���ăG���[���������܂��� [4:ShutDown]";
                break;
            case WIZMOController.CanNotCertificate:
                log = "SIMVR�̔F�؂Ɏ��s���܂��� [5:AuthenticationFailure]";
                break;
            case WIZMOController.Initial:
                log = "���������Ă��܂� [6:Intialize]";
                break;
            case WIZMOController.Running:
                log = "���퓮�쒆�ł� [7:Runnning]";
                break;
            case WIZMOController.StopActuator:
                log = "�ꕔ�̃A�N�`���G�[�^����~���Ă܂� [8:StopActuator]";
                break;
            case WIZMOController.CalibrationRetry:
                log = "SIMVR�̍ēx���_���A���ł� [9:InternalDisconnectionError]";
                break;
            default:
                log = "�������Ă��܂��� [-1:�[]";
                break;
        }

        return log;
    }
}
