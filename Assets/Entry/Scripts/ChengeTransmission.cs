using UnityEngine;
using UnityEngine.UI;


public class ChengeTransmission : TwoChoiceNew
{
    private float TimeCounter = 0;
    public static int TransmissionMode = 1;

    // Update is called once per frame
    void Update()
    {
        TimeCounter++;
        if (TimeCounter >= 60)
        {
            WaitChoice();
        }
    }

    public static int GetTransmission()
    {
        return TransmissionMode;
    }
}
