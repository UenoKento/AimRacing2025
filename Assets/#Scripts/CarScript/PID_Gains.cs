using System;

[Serializable]
public struct PID_Gains
{
    /// <summary>
    /// ”ä—áƒQƒCƒ“
    /// </summary>
    public float P;

    /// <summary>
    /// Ï•ªƒQƒCƒ“
    /// </summary>
    public float I;

    /// <summary>
    /// ”÷•ªƒQƒCƒ“
    /// </summary>
    public float D;

    public PID_Gains(float p, float i, float d)
    {
        P = p;
        I = i;
        D = d;
    }
}
