using TMPro;
using UnityEngine;

// meterの制御

public class MeterController : MonoBehaviour
{
    // 車両
    [SerializeField] private VehicleController m_vehicleController;

    // 車両の状態
    struct VehicleParameter
    {
        public int currentGear;     // 現在のギア
        public int prevGear;        // 前のギア
        public float engineRPM;     // エンジンの回転数
        public float kph;           // 速度
    }
    VehicleParameter m_vehicleParameter;

    // 作り直すかも
    [SerializeField] private TimeKeeper m_timeKeeper;

    // リストにするかも
    [SerializeField] private TextMeshProUGUI m_gearText;    // ギアのテキスト
    [SerializeField] private TextMeshProUGUI m_rpmText;     // エンジン回転数のテキスト
    [SerializeField] private TextMeshProUGUI m_kphText;     // 速度のテキスト

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
        // 車両の状態を取得
        GetVehicleParam();

        // メーターの各パラメータを更新
        UpdateMeterParam();

        m_LEDIndicatorController.Run(m_vehicleParameter.engineRPM);
    }

    /// <summary>
    /// 車両の状態を取得して更新する
    /// </summary>
    private void GetVehicleParam()
    {
        m_vehicleParameter.currentGear = m_vehicleController.ActiveGear;
        m_vehicleParameter.engineRPM = m_vehicleController.EngineRPM;
        m_vehicleParameter.kph = m_vehicleController.KPH;
    }

    /// <summary>
    /// 各パラメータのUIを更新
    /// </summary>
    private void UpdateMeterParam()
    {   
        UpdateGear();
        UpdateRPM();
        UpdateKPH();
    }

    /// <summary>
    /// ギアの更新
    /// </summary>
    private void UpdateGear()
    {
        // ギアが前フレームから変更されていなければreturn
        if (m_vehicleParameter.currentGear == m_vehicleParameter.prevGear) return;

        // 1速以上の時
        if(m_vehicleParameter.currentGear > 0)
        {
            // ギアのテキストを書き換える
            m_gearText.text = m_vehicleParameter.currentGear.ToString();
            m_vehicleParameter.prevGear = m_vehicleParameter.currentGear;
            return;
        }

        // ニュートラルの時
        if (m_vehicleParameter.currentGear == 0)
        {
            // ギアのテキストをNに書き換える
            m_gearText.text = "N";
            m_vehicleParameter.prevGear = m_vehicleParameter.currentGear;
            return;
        }
        
        // Rの時
        if(m_vehicleParameter.currentGear < 0)
        {
            // ギアのテキストをRに書き換える
            m_gearText.text = "R";
            m_vehicleParameter.prevGear = m_vehicleParameter.currentGear;
        }
    }

    /// <summary>
    /// 回転数の更新
    /// </summary>
    private void UpdateRPM()
    {
        // 回転数のテキストを書き換える
        m_rpmText.text = m_vehicleController.EngineRPM.ToString("0000");
    }

    /// <summary>
    /// 速度の更新
    /// </summary>
    private void UpdateKPH()
    {
        // 速度のテキストを書き換える
        m_kphText.text = m_vehicleController.KPH.ToString("000");
    }
}

