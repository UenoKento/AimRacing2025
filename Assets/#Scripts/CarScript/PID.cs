using UnityEngine;

public class PID
{

    public PID_Gains Gains;

    float intError;
    float prevError;

    public PID(float p, float i, float d)
    {
        Gains = new PID_Gains(p, i, d);
    }

    //PID������g�p���A�K�v�ȃX���b�g���J�x�����߂�
    public float Compute_IdleThrottle(float CurrentRPM,float IdleRPM)
    {
        //�����v�Z
        float Error = IdleRPM - CurrentRPM;

        //�ϕ��v�Z
        intError += Error * Time.fixedDeltaTime;

        //�����v�Z
        float DiffError = (Error - prevError) / Time.fixedDeltaTime;

        float IdleThrottle = 
            Gains.P * Error +
            Gains.I * intError +
            Gains.D * DiffError;

        return IdleThrottle;
    }

}
