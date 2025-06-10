using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeedMeterTest : MonoBehaviour
{
    [SerializeField]
    VehicleController2024 vehicleController;
    TextMeshProUGUI tmpro;

    // Start is called before the first frame update
    void Start()
    {
        tmpro = gameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        string str = string.Empty;
        str += vehicleController.KPH.ToString("f0") + " km/h\n";
        str += vehicleController.EngineRPM.ToString("f0") + " r/min\n";

        if(vehicleController.ActiveGear > 0)
            str += vehicleController.ActiveGear + "\n";
        else
        {
            if (vehicleController.ActiveGear == 0)
                str += "N\n";
            else
                str += "R\n";
        }

        str += "Accel :" + vehicleController.Accel.ToString("f2") + "\n";

        str += "Brake :" + vehicleController.Brake.ToString("f2") + "\n";
        str += "Clutch:" + vehicleController.Clutch.ToString("f2") + "\n";

        tmpro.text = str;
    }
}
