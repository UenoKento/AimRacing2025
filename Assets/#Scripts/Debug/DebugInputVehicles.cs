using System.Collections.Generic;
using UnityEngine;

public class DebugInputVehicles : MonoBehaviour
{
    [SerializeField]
    private List<WheelController2024> m_wheelControllers;

    [SerializeField]
    private VehicleController m_vehicleController;  // ���x�Q�Ɨp

    //[SerializeField]
    //private NewEngine m_engine;

    // �f�o�b�O�菇�i��: Start���\�b�h��null�`�F�b�N��ǉ��j
    private void Start()
    {
        // null�`�F�b�N
        if (m_wheelControllers == null || m_wheelControllers.Count == 0)
            Debug.LogError("WheelController2024 ��������܂���");
        if (m_vehicleController == null)
            Debug.LogError("VehicleController2024 ���A�^�b�`����Ă��܂���");
        //if (m_engine == null)
        //    Debug.LogError("NewEngine ���擾�ł��܂���");
    }

    // �y�c
    // �ǋL 06/10
    // ���� 07/31
    // GUI�̕\��
    private void OnGUI()
    {
        //GUI
        GUIStyle style = new GUIStyle(GUI.skin.label)
        {
            fontSize = 24, 
            normal = { textColor = Color.white }
        };

        style.fontSize = 24;    // �����̑傫��
        style.normal.textColor = Color.white; // �����̐F
        int y = 10; // Y���W�����Z���ĕ\���ʒu�����炷

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

        // �X�e�A�����O�p�̕\��
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
