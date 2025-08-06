
static public class CarPhysics
{
    // AngularVelocity[rad/sec] to RPM[N/min]
    public static readonly float Rad2RPM = 30f / UnityEngine.Mathf.PI;
    // RPM[N/min] to AngularVelocity[rad/sec]
    public static readonly float RPM2Rad = 1f / Rad2RPM;

    // RangeClamp01
    public static float RangeClamp01(float _value,float _rangeA, float _rangeB)
    {
        return UnityEngine.Mathf.Clamp01((_value - _rangeA) / (_rangeB - _rangeA));
    }
}
