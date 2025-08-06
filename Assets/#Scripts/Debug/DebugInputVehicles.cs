using System.Collections.Generic;
using UnityEngine;

public class DebugInputVehicles : MonoBehaviour
{
    [SerializeField]
    private List<WheelController2024> m_wheelControllers;

    [SerializeField]
    private VehicleController m_vehicleController;  // 速度参照用

    //[SerializeField]
    //private NewEngine m_engine;

    // デバッグ手順（例: Startメソッドにnullチェックを追加）
    private void Start()
    {
        // nullチェック
        if (m_wheelControllers == null || m_wheelControllers.Count == 0)
            Debug.LogError("WheelController2024 が見つかりません");
        if (m_vehicleController == null)
            Debug.LogError("VehicleController2024 がアタッチされていません");
        //if (m_engine == null)
        //    Debug.LogError("NewEngine が取得できません");
    }

    // 土田
    // 追記 06/10
    // 分離 07/31
    // GUIの表示
    private void OnGUI()
    {
        //GUI
        GUIStyle style = new GUIStyle(GUI.skin.label)
        {
            fontSize = 24, 
            normal = { textColor = Color.white }
        };

        style.fontSize = 24;    // 文字の大きさ
        style.normal.textColor = Color.white; // 文字の色
        int y = 10; // Y座標を加算して表示位置をずらす

        GUI.Label(new Rect(10, y, 500, 50), "Speed: " + m_vehicleController.KPH.ToString("F2") + " km/h", style);
        y += 30;
        //GUI.Label(new Rect(10, y, 400, 50), "Engine RPM: " +  m_engine.RPM.ToString("F2") + " rpm", style);
        y += 30;

        GUI.Label(new Rect(10, y, 400, 30), $"Accel Input: {m_vehicleController.Accel:F2}", style);
        y += 30;
        GUI.Label(new Rect(10, y, 400, 30), $"Brake Input: {m_vehicleController.Brake:F2}", style);
        y += 30;
        GUI.Label(new Rect(10, y, 400, 30), $"Clutch Input: {m_vehicleController.Clutch:F2}", style);
        y += 30;
        GUI.Label(new Rect(10, y, 400, 30), $"Steer Input: {m_vehicleController.Steering:F2}", style);
        y += 30;

        // ステアリング角の表示
        foreach (WheelController2024 wheel in m_wheelControllers)
        {
            if (wheel.IsFrontSide)
            {
                string side = wheel.IsRightSide ? "Right" : "Left";
                GUI.Label(new Rect(10, y, 400, 30), $"{side} Front Steer Angle: {wheel.SteerAngle:F2} deg", style);
                y += 30;
            }
        }
    }
}
