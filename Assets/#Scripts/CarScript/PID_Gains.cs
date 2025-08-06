using System;

[Serializable]
public struct PID_Gains
{
    /// <summary>
    /// ���Q�C��
    /// </summary>
    public float P;

    /// <summary>
    /// �ϕ��Q�C��
    /// </summary>
    public float I;

    /// <summary>
    /// �����Q�C��
    /// </summary>
    public float D;

    public PID_Gains(float p, float i, float d)
    {
        P = p;
        I = i;
        D = d;
    }
}
