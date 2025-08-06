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

    //PID制御を使用し、必要なスロットル開度を求める
    public float Compute_IdleThrottle(float CurrentRPM,float IdleRPM)
    {
        //差分計算
        float Error = IdleRPM - CurrentRPM;

        //積分計算
        intError += Error * Time.fixedDeltaTime;

        //微分計算
        float DiffError = (Error - prevError) / Time.fixedDeltaTime;

        float IdleThrottle = 
            Gains.P * Error +
            Gains.I * intError +
            Gains.D * DiffError;

        return IdleThrottle;
    }

}
