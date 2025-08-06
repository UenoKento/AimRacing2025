using System.Collections;
using UnityEngine;

[System.Serializable]
public class Car_Engine
{
    // そのRPMで生成できる最大のトルクのマップ(エンジン特性グラフ(トルクのみ))
    [SerializeField]
    AnimationCurve m_torqueCurve;

    [SerializeField, ShowInInspector]
    float m_angularVelocity;    // エンジンのフライホイールの角速度
    [SerializeField, ShowInInspector]
    float m_engineRPM;          // 現在のエンジン回転数
    [SerializeField, ShowInInspector]
    float m_effectiveTorque;    // 実効トルク値
    [SerializeField, ShowInInspector]
    float m_ReactionTorque;    // 実効トルク値
    [SerializeField]
    float m_inertia = 0.2f;     // エンジンの慣性モーメント
    float RotationalVolume_Crankshaft;  //クランクシャフトの回転量


    [Header("Engine_Design")]
    public float Cylinders = 1;
    public float IdleRPM;
    public float CellMotorVelocity;
    public float RPM_Offset;
    bool Pressed_Button = false;
    [SerializeField]
    PID pid;

    [Header("Throttle")]
    public float MaxThrottle;
    public float m_Throttle;
    public float AdjustThrottle;

    [Header("Friction")]
    [SerializeField]
    float m_frictionAtIdle = 50f;  // アイドル時の摩擦トルク(最小摩擦トルク)
    [SerializeField]
    float m_frictionCoef = 0.012f; // エンジン内摩擦係数
    [SerializeField]
    float m_InertiaFrictionCoef = 0.015f;   // エンジン内慣性摩擦係数
    [SerializeField]
    public float Friction_Torque;           //摩擦トルク


    [Header("Rev Limiter")]
    [SerializeField]
    float m_overRevRPM;// レブリミッターが起動するRPM
    [SerializeField]
    float m_limitRPM = 9000f;    // 回転数の限界
    bool m_InjectionCut_Rev = false;
    bool m_injectionCut = false;


    #region プロパティ
    public float RPM
    {
        get => m_engineRPM;
    }

    public float EngineTorque
    {
        get => m_effectiveTorque;
    }

    // 角運動量(クラッチの計算に使用)
    public float AngularMomentum
    {
        get => m_angularVelocity * m_inertia;
    }

    // オーバーレブRPM
    public float OverRevRPM
    {
        get => m_overRevRPM;
        set => m_overRevRPM = value;
    }

    // インジェクションカット(スロットルを0にする)
    public bool InjectionCut
    {
        get => m_injectionCut;
        set => m_injectionCut = value;
    }
    #endregion


    //Test
    float One_Cycle = 4 * Mathf.PI;
    public float DeltaTime_Ratio;

    Car_Engine()
    {
        pid = new PID(0.0024f, 0.0f, 0.0f);
    }

    public void FixedUpdate(float _Throttle, float _ReactionTorque)
    {
        m_Throttle = _Throttle;
        m_ReactionTorque = _ReactionTorque;

        //摩擦トルクの計算
        Calclate_FrictionTorque();

        //セルモーターの始動
        //Move_CellMotor();

        //摩擦・駆動輪トルクの反映
        AddBackTorque();

        //RPM表示に対応させる
        m_engineRPM = m_angularVelocity * CarPhysics.Rad2RPM;

        //アイドリングのスロットル開度の調整
        FeedBack_Throttle();

        //発火サイクルの演算
        Calclate_IgnitionCycle();

        m_effectiveTorque = m_angularVelocity * m_inertia;

        //_rb.angularVelocity = new Vector3(0.0f, 0.0f, m_angularVelocity);

    }

    void Calclate_FrictionTorque()
    {
        //最小摩擦トルクの掛かる方向
        float Engine_IdlefrictionRatio;

        //停止中は抵抗無しに
        if (m_angularVelocity == 0)
        {
            Engine_IdlefrictionRatio = 0;
        }
        else
        {
            Engine_IdlefrictionRatio = m_frictionAtIdle / Mathf.Abs(m_frictionAtIdle);
        }

        // 摩擦トルク = (最小摩擦トルク * エンジンの回転方向) + (摩擦係数 * RPM) + (慣性抵抗係数 * RPM^2)
        Friction_Torque =
            (m_frictionAtIdle * Engine_IdlefrictionRatio) +
            (m_frictionCoef * m_angularVelocity) +
            (m_InertiaFrictionCoef * Mathf.Pow(m_angularVelocity, 2f));

    }

    void AddBackTorque()
    {
        //摩擦・駆動輪トルクをエンジン回転数に反映
        m_angularVelocity -= (((Friction_Torque + m_ReactionTorque) / m_inertia)) * (Time.fixedDeltaTime * DeltaTime_Ratio);

        //逆回転しないよう制限
        m_angularVelocity = Mathf.Clamp(m_angularVelocity, 0.0f, 100000.0f);
    }



    void FeedBack_Throttle()
    {
        //PIDフィードバック処理の実行(目標値まで上げる)
        AdjustThrottle = pid.Compute_IdleThrottle(m_engineRPM, IdleRPM);

        //スロットル量の制限
        AdjustThrottle = Mathf.Clamp(AdjustThrottle, 0.0f, 1000.0f);
    }

    void Calclate_IgnitionCycle()
    {
        //移動量の加算・制限
        RotationalVolume_Crankshaft += m_angularVelocity * (Time.fixedDeltaTime * DeltaTime_Ratio);
        RotationalVolume_Crankshaft = Mathf.Clamp(RotationalVolume_Crankshaft, 0.0f, 10000.0f);

        //指定された量回転したら、発火工程の演算をする

        while (RotationalVolume_Crankshaft > (One_Cycle / Cylinders) && m_angularVelocity > 0)
        {
            //回転したぶん引く
            RotationalVolume_Crankshaft -= One_Cycle / Cylinders;

            //回転数の制限
            RevLimitter();

            //回転数の追加(限界の回転数になっていなければ)
            if (!m_InjectionCut_Rev) AddangularVelocity();
        }
    }

    void AddangularVelocity()
    {
        //スロットル開度の制限
        MaxThrottle = Mathf.Clamp01(m_Throttle + AdjustThrottle);

        //トルクの計算・反映
        float Torque = (m_torqueCurve.Evaluate(m_angularVelocity * CarPhysics.Rad2RPM) / Cylinders) * MaxThrottle;
        m_angularVelocity += Torque / m_inertia * (Time.fixedDeltaTime * DeltaTime_Ratio);
    }


    public IEnumerator Move_CellMotor()
    {
        //セルモーターを回す
        //if (!Pressed_Button) { return; }

        while(m_engineRPM < IdleRPM)
        {
			m_angularVelocity += CellMotorVelocity * Time.deltaTime;

            yield return null;
		}

	}

    void RevLimitter()
    {
        if (m_engineRPM > m_limitRPM && !m_InjectionCut_Rev)
        {
            m_InjectionCut_Rev = true;
        }
        else if (m_engineRPM < m_overRevRPM && m_InjectionCut_Rev)
        {
            m_InjectionCut_Rev = false;
        }
    }

}
