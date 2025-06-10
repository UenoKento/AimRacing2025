using UnityEngine;
static public class CarPhysics
{
    // AngularVelocity[rad/sec] to RPM[N/min]
    public const float Rad2RPM = 30f / Mathf.PI;
    // RPM[N/min] to AngularVelocity[rad/sec]
    public const float RPM2Rad = Mathf.PI / 30f;

    // RangeClamp01
    public static float RangeClamp01(float _value,float _rangeA, float _rangeB)
    {
        return Mathf.Clamp01((_value - _rangeA) / (_rangeB - _rangeA));
    }
}
