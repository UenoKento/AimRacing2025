using TMPro;
using UnityEngine;

// meter�̐���

public class MeterController : MonoBehaviour
{
    // �ԗ�
    [SerializeField] private VehicleController m_vehicleController;

    // �ԗ��̏��
    struct VehicleParameter
    {
        public int currentGear;     // ���݂̃M�A
        public int prevGear;        // �O�̃M�A
        public float engineRPM;     // �G���W���̉�]��
        public float kph;           // ���x
    }
    VehicleParameter m_vehicleParameter;

    // ��蒼������
    [SerializeField] private TimeKeeper m_timeKeeper;

    // ���X�g�ɂ��邩��
    [SerializeField] private TextMeshProUGUI m_gearText;    // �M�A�̃e�L�X�g
    [SerializeField] private TextMeshProUGUI m_rpmText;     // �G���W����]���̃e�L�X�g
    [SerializeField] private TextMeshProUGUI m_kphText;     // ���x�̃e�L�X�g

    [SerializeField] LED_IndicatorController m_LEDIndicatorController;

    void Start()
    {
        if (m_vehicleController == null) Debug.Log("Null : Missing vehicleController (used in MeterController)");
        if (m_gearText == null) Debug.Log("Null : Missing gearText (UI : used in meter)");
        if (m_rpmText == null) Debug.Log("Null : Missing rpmText (UI : used in meter)");
        if (m_kphText == null) Debug.Log("Null : Missing kphText (UI : used in meter)");

        m_LEDIndicatorController.Init();
    }

    void Update()
    {
        // �ԗ��̏�Ԃ��擾
        GetVehicleParam();

        // ���[�^�[�̊e�p�����[�^���X�V
        UpdateMeterParam();

        m_LEDIndicatorController.Run(m_vehicleParameter.engineRPM);
    }

    /// <summary>
    /// �ԗ��̏�Ԃ��擾���čX�V����
    /// </summary>
    private void GetVehicleParam()
    {
        m_vehicleParameter.currentGear = m_vehicleController.ActiveGear;
        m_vehicleParameter.engineRPM = m_vehicleController.EngineRPM;
        m_vehicleParameter.kph = m_vehicleController.KPH;
    }

    /// <summary>
    /// �e�p�����[�^��UI���X�V
    /// </summary>
    private void UpdateMeterParam()
    {   
        UpdateGear();
        UpdateRPM();
        UpdateKPH();
    }

    /// <summary>
    /// �M�A�̍X�V
    /// </summary>
    private void UpdateGear()
    {
        // �M�A���O�t���[������ύX����Ă��Ȃ����return
        if (m_vehicleParameter.currentGear == m_vehicleParameter.prevGear) return;

        // 1���ȏ�̎�
        if(m_vehicleParameter.currentGear > 0)
        {
            // �M�A�̃e�L�X�g������������
            m_gearText.text = m_vehicleParameter.currentGear.ToString();
            m_vehicleParameter.prevGear = m_vehicleParameter.currentGear;
            return;
        }

        // �j���[�g�����̎�
        if (m_vehicleParameter.currentGear == 0)
        {
            // �M�A�̃e�L�X�g��N�ɏ���������
            m_gearText.text = "N";
            m_vehicleParameter.prevGear = m_vehicleParameter.currentGear;
            return;
        }
        
        // R�̎�
        if(m_vehicleParameter.currentGear < 0)
        {
            // �M�A�̃e�L�X�g��R�ɏ���������
            m_gearText.text = "R";
            m_vehicleParameter.prevGear = m_vehicleParameter.currentGear;
        }
    }

    /// <summary>
    /// ��]���̍X�V
    /// </summary>
    private void UpdateRPM()
    {
        // ��]���̃e�L�X�g������������
        m_rpmText.text = m_vehicleController.EngineRPM.ToString("0000");
    }

    /// <summary>
    /// ���x�̍X�V
    /// </summary>
    private void UpdateKPH()
    {
        // ���x�̃e�L�X�g������������
        m_kphText.text = m_vehicleController.KPH.ToString("000");
    }
}

