using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[DefaultExecutionOrder(-100)]

public class G29 : MonoBehaviour
{
    public LogitechGSDK.DIJOYSTATE2ENGINES rec;
    //public AIM./*DesktopInputManager.*/InputController inputController;

    public float accel;

    // Start is called before the first frame update
    void Start()
    {
        //ReConnect();

        Debug.Log("SteeringInit:" + LogitechGSDK.LogiSteeringInitialize(false));
    }

    private void Update()
    {
        if (LogitechGSDK.LogiUpdate() && LogitechGSDK.LogiIsConnected(0))
        {
            //CONTROLLER STATE
            //���͊m�F
            rec = LogitechGSDK.LogiGetStateUnity(0);
        }
        
        InputPedal();
    }
    public void ReConnect()
    {
        rec = LogitechGSDK.LogiGetStateUnity(0);
    }
    void InputPedal()
    {
        // �A�N�Z���ƃu���[�L�ɐU�蕪��
        float newAccel = 0;
        float newBrake = 0;

        //AIM.InputHandleController.GetHandleAccelAndBrake(ref newAccel, ref newBrake);

        //// �n���h���g�p
        //if (isConnectHandle)
        //{
        //    // LogicoolSDK�g�p
        //    if (vc.wheelInput.isG29)
        //    {
        //        newAccel = vc.wheelInput.inputAxel;
        //        newBrake = vc.wheelInput.inputBrake;
        //    }
        //    else
        //    {
        //        AIM.InputHandleController.GetHandleAccelAndBrake(ref newAccel, ref newBrake);
        //    }
        //    newAccel = accelCurve.Evaluate(newAccel).Round(inputDigit);
        //    newBrake = brakeCurve.Evaluate(newBrake).Round(inputDigit);
        //}
        // �R���g���[���[�g�p
        //else if (isConectController)
        //{
        //    newBrake = Input.GetAxis("L_Trigger").Round(inputDigit);
        //    newAccel = Input.GetAxis("R_Trigger").Round(inputDigit);
        //}
        //// �L�[�{�[�h�g�p
        //else
        //{
        //    newAccel = Input.GetAxis("KeyboardAccel").Round(inputDigit);
        //    newBrake = Input.GetAxis("KeyboardBrake").Round(inputDigit);
        //}
        //// �N���b�`
        //if (!vc.transmission.automaticClutch)
        //{
        //    try
        //    {
        //        vc.input.Clutch = Input.GetAxis("Clutch").Round(inputDigit);
        //    }
        //    catch
        //    {
        //        vc.transmission.automaticClutch = true;
        //        vc.input.Clutch = 0.0f;
        //    }
        //}

        //// �S�[����u���[�L��������
        //if (GoalJudgment.isGoal)
        //{
        //    newAccel = 0;
        //    newBrake = 1;
        //}

        //// ����
        //vc.input.Vertical = newAccel - newBrake;
        //brakeValue = newBrake;

        //Debug.Log(Input.GetAxis("Accel"));

        accel = Input.GetAxis("Accel");
    }
}
