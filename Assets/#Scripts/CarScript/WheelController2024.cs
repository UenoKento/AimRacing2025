/**
 * @file    WheelController2024.cs
 * @brief   ホイールから車体に加わる力を計算する
 * @author  22CU0219 鈴木友也
 * @date    2024/05/17  作成
 *          2024/11/18 最終更新
 */
using System.Diagnostics.SymbolStore;
using UnityEngine;
using UnityEngine.UIElements;

public class WheelController2024 : MonoBehaviour
{
    [SerializeField]
    bool m_isRight;     // 左右どちらのホイールなのか
    [SerializeField]
    bool m_isFront;     // 前後どちらのホイールなのか
    [SerializeField]
    bool m_isDrive;
    [SerializeField]
    bool m_isSteer;
    [SerializeField]
    bool m_isIgnoreLoad;
    [SerializeField]
    bool m_trueTraction;  // 計算方法切り替え


    [Space]
    [SerializeField]
    float m_mass;
    [SerializeField]
    float m_radius;
    [SerializeField]
    float m_width;
    [SerializeField]
    LayerMask m_layerMask = Physics.IgnoreRaycastLayer;
    [SerializeField]
    Transform m_visual;
    [SerializeField]
    float m_wheelRPM;

    Rigidbody m_parentRigid;
    RaycastHit m_raycastHit;
    bool m_bOnGround;

    [Header("Property")]
    // 車体に加える力
    [SerializeField,ShowInInspector]
    Vector3 m_totalF;                 // 最終的にAddforceする力
    [SerializeField,ShowInInspector]
    float m_tractionT;                // 牽引力
    [SerializeField,ShowInInspector]
    float m_driveT;                   // 駆動トルク
    float m_brakeT;                   // ブレーキトルク
    float m_longF;                    // 縦力
    float m_latF;                     // 横力
    float m_load;                     // 荷重

    // 速度関連
    Vector3 m_wheelVelocity;
    float m_longSpeed; 
    float m_latSpeed;
    [SerializeField,ShowInInspector]
    float m_longSlipVelocity;   // 縦方向スリップ速度
    [SerializeField]
    float m_angularVelocity;    // 角速度
    float m_inertia;            // 慣性モーメント

    [Header("MagicFormula")]
     [SerializeField, ShowInInspector]
    float m_slipRatio;          // 実際のスリップ率
    float m_diffSlipRatio;      // 微分スリップ率
    [SerializeField,ShowInInspector]
    float m_slipAngle;                         //スリップ角

	// 摩擦
	[SerializeField]
	float m_frictonCoef = 1f;  // 摩擦係数
	[SerializeField,ShowInInspector]
    bool m_isWheelLocked;
    [SerializeField]
    MagicFormula m_longForceCurve;
    [SerializeField]
    MagicFormula m_latForceCurve;
    [SerializeField,Range(0.1f,1f)]
    float m_relaxationLength;
    float m_steerAngle;
   

    [Header("Suspension")]
    // サスペンション関連
    [SerializeField]
    float m_suspensionDistance;             // サスペンションの最大伸長距離(ローカル座標)
    [SerializeField]
    SimpleSpringJoint m_suspensionSpring;   // バネの各種パラメータを設定
    [SerializeField,ShowInInspector]
    float m_suspensionLoad;                  // サスペンションから計算した上下荷重

    // RayCast
    [Header("MultiRaycast")]
    [SerializeField]
    int m_raysNumber = 36;
    [SerializeField]
    float m_raysMaxAngle = 180;
    float m_orgRadius;

    #region プロパティ
    public bool IsGround => m_bOnGround;

    // 前後どちらのホイールか判定用(ブレーキバイアスで使用)
    public bool IsFrontSide  => m_isFront;

    // 左右どちらのホイールか判定用(アッカーマンアングルの計算で使用)
    public bool IsRightSide => m_isRight;

    // 駆動輪か判定用
    public bool IsDrive => m_isDrive;

    // 操舵輪か判定用
    public bool IsSteer => m_isSteer;

    public bool TrueTraction
    {
        get => m_trueTraction;
        set => m_trueTraction = value;
    }

    public float SteerAngle
    {
        get => m_steerAngle;

        set
        {
            // 操舵輪の場合のみset可能
            if (m_isSteer)
                m_steerAngle = value;
        }
    }

  

    public float Radius => m_radius;
    public float Inertia => m_inertia;

    public float Load
    {
        set => m_load = value;
    }

    public float LongForce => m_longF * m_load;

    // 速度関連
    public Vector3 PatchVelocity => m_wheelVelocity;
    public float LongSpeed => m_longSpeed;
    public float LatSpeed => m_latSpeed;
    public float LongSlip => m_longSlipVelocity;
    public float SlipRatio => m_slipRatio;
    public float WheelAngularVelocity => m_angularVelocity;
    public float WheelRPM => m_angularVelocity * CarPhysics.Rad2RPM;


    public MagicFormula LongFrictionCurve => m_longForceCurve;
    public MagicFormula LatFrictionCurve => m_latForceCurve;
#endregion


// Start is called before the first frame update
	void Start()
    {
        // Rigidbodyを取得
        m_parentRigid = GetComponentInParent<Rigidbody>();

        // 慣性モーメントを計算
        m_inertia = m_mass * Mathf.Pow(m_radius,2f) / 2f;

        // 初期荷重(四等分)
		m_load = m_parentRigid.mass / 4f * Physics.gravity.magnitude;

        // 各種初期化
        m_latForceCurve.Initialize();
        m_longForceCurve.Initialize();
		m_driveT = 0f;
        m_angularVelocity = 0f;
	}

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateWheelHit();

        // ステアリングの角度に曲げる
        m_visual.localEulerAngles = transform.localEulerAngles = new Vector3(0, m_steerAngle, 0);

        if (m_bOnGround)
        {
            UpdateSuspension();

            UpdateVelocity();
        }
        else
        {
            m_visual.position = transform.position + (-transform.up * m_suspensionDistance);
        }

    }

    /// <summary>
    /// 縦横方向にかかる力の計算(VehicleControllerから駆動トルクを渡す)
    /// </summary>
    public void UpdateTotalForce(in float _driveTorque, in float _brakeTorque)
    {
        if (!m_bOnGround)
            return;

        // 駆動トルクは駆動輪のみ加算
        if (m_isDrive)
        {
            m_driveT = _driveTorque;
        }
        m_brakeT = _brakeTorque;

        CalcTotalForce();

		// 車にかかっている力を可視化
		Debug.DrawRay(m_visual.transform.position, transform.forward * m_longF, Color.cyan);
		Debug.DrawRay(m_visual.transform.position, transform.right * m_latF, Color.red);


		// 複合タイヤ力計算
		CalcCombineForce(out m_longF,out m_latF);

        if(m_trueTraction)
            m_tractionT = m_longF * m_load * m_radius;

        // 駆動トルクとブレーキトルクから角速度を更新
        UpdateAngularVelocity();

        // ホイールロック時にスリップ速度のSign(-1 or 1)を代入するだけで
        // ブレーキ時に坂道でビタっと止まることが出来る
        if (m_isWheelLocked)
            m_longF = Mathf.Sign(m_longSlipVelocity);


        // 総力の計算
        m_totalF += transform.forward * m_longF * m_load;
        m_totalF += transform.right * m_latF * m_load;

		// RigidbodyにAddForceする
		if (!float.IsNaN(m_totalF.magnitude))
            m_parentRigid.AddForceAtPosition(m_totalF, m_raycastHit.point, ForceMode.Force);

        //Debug.Log(gameObject.name + "::" + m_longF);
    }

    /// <summary>
    /// サスペンション更新
    /// 07/05 22CU0235 諸星大和 作成
    /// </summary>
    void UpdateSuspension()
    {
        // ローカルの下方向をワールドに変換
        Vector3 down = transform.TransformDirection(Vector3.down);
       
        // 車輪の回転を考慮せずに、車輪が地面に対してどのくらいの速さで動いているかを計算する。
        Vector3 velocityAtTouch = m_parentRigid.GetPointVelocity(m_raycastHit.point);

        // スプリングの圧縮を計算する
        // 位置の差をサスペンションの全範囲で割る
        float compression = m_raycastHit.distance / (m_suspensionDistance + m_radius);
        //Debug.Log("01 compression : " + compression);
        compression = -compression + 1;
        //Debug.Log("02 compression : " + compression);

        // 最終的な力
        Vector3 force = -down * compression * m_suspensionSpring.Spring;
        //Debug.Log("force : " + force);

        // 接触点の速度をローカル空間に変換したもの
        Vector3 t = transform.InverseTransformDirection(velocityAtTouch);
        //Debug.Log("t : " + t);

        // ローカルXおよび、Z方向 = 0
        // ここで、tはショックが収縮/膨張する速度と等しいとする。
        t.z = 0;
        t.x = 0;

        // ワールド空間 * 減衰
        // この力はサスペンションの摩擦による力をシミュレートしています。
        Vector3 shockDrag = transform.TransformDirection(t) * -m_suspensionSpring.Damper;

        // 
        m_parentRigid.AddForceAtPosition(force + shockDrag, transform.position);
        m_suspensionLoad = (force + shockDrag).magnitude;

        m_visual.position = transform.position + (down * (m_raycastHit.distance - m_radius));
    }


    /// <summary>
    /// レイを1本飛ばして地面との当たり判定を取る
    /// </summary>
    void UpdateWheelHit()
    {
        // 地面との当たり判定の更新
        m_bOnGround = Physics.Raycast(new Ray(transform.position, -transform.up), out m_raycastHit, m_radius + m_suspensionDistance, m_layerMask);


        //RaycastExpansion();

        // 地面に飛ばすレイを可視化
        //Debug.DrawRay(transform.position, -transform.up * m_radius, Color.red, 3f);
    }


    /// <summary>
    /// 縦横方向の速度を更新
    /// </summary>
    void UpdateVelocity()
    {
        // レイが当たった場所の速度を取得
        m_wheelVelocity = m_parentRigid.GetPointVelocity(m_raycastHit.point);

        m_longSpeed = Vector3.Dot(m_wheelVelocity, transform.forward);
        m_latSpeed   = Vector3.Dot(m_wheelVelocity, transform.right);
		m_longSlipVelocity = m_radius * m_angularVelocity - m_longSpeed;
	}

    /// <summary>
    /// ホイールの角速度(回転速度)を更新
    /// </summary>
    void UpdateAngularVelocity()
    {
        
        float fixAngularVelocity = m_angularVelocity; // 一時計算用の変数

        // 加速力
        // 角加速度 = 角速度( = 速度/半径) / 単位時間
        float tractionAngularAccel = m_longSlipVelocity / m_radius / Time.fixedDeltaTime;
        // トルク = 角加速度 * 慣性モーメント
        if(!m_trueTraction)
            m_tractionT = tractionAngularAccel * m_inertia;

        float totalT = m_driveT - m_tractionT;
        float angularAccle = totalT / m_inertia;
        fixAngularVelocity += angularAccle * Time.fixedDeltaTime;

        // 回転方向
		float prevRotationDirection = Mathf.Sign(fixAngularVelocity);
        if (fixAngularVelocity < float.Epsilon)
            prevRotationDirection = 0f;

		// 減速力
		m_brakeT = Mathf.Sign(fixAngularVelocity) * m_brakeT;
		// 転がり抵抗
		float rollresistT = Mathf.Sign(fixAngularVelocity) * 0.015f * m_load * m_radius;

        // 角速度を減少させる角加速度
        float angularDecele = (m_brakeT + rollresistT) / m_inertia;
        fixAngularVelocity -= angularDecele * Time.fixedDeltaTime;
            
        float fixRotationDirection = Mathf.Sign(fixAngularVelocity);
        if (fixAngularVelocity < float.Epsilon)
            fixRotationDirection = 0f;

        // 0通過チェック(減速力によってホイールの回転方向が変わったかチェック)
        if (prevRotationDirection != fixRotationDirection)
        {
            // ブレーキを踏んだ時逆方向に回転しないように0を代入
            m_angularVelocity = 0f;
           
            // 角速度を0に戻すとスリップ率がある程度の値のままな現象を確認
            m_diffSlipRatio = 0f; 
            // →ブレーキを踏んだとき前後に動いてしまうので、ここで0を代入する

            m_isWheelLocked = true;
        }
        else
        {
            m_angularVelocity = fixAngularVelocity;
            m_isWheelLocked = false;
        }

        m_wheelRPM = m_angularVelocity * CarPhysics.Rad2RPM;
     
    }

    /// <summary>
    /// Addforceする力を計算
    /// </summary>
    void CalcTotalForce()
    {
        m_totalF = Vector3.zero;

        if (m_isIgnoreLoad)
            m_load = m_suspensionLoad;

        //----------------------------------------------------------------------------
        // 縦力の計算
        //----------------------------------------------------------------------------
        // 低速時での発散を回避するスリップ率計算[参考:SAE950311]
        float delta = m_longSlipVelocity - Mathf.Abs(m_longSpeed) * m_diffSlipRatio;
        delta /= m_relaxationLength; // 文献では定数B

        m_diffSlipRatio += delta * Time.fixedDeltaTime;
        //m_diffSlipRatio = CalcRK4();
        

        // スリップ率が1000%を振り切れることがあるため100%でClamp
        //if (m_diffSlipRatio > 1f)
        //    m_diffSlipRatio = 1f;

        // 振動を減衰する発振周期
        float tau = 0.02f; // 調整の必要があるかもしれないが現状問題ない
        // 高速時は不安定になるため無効化
        if (Mathf.Abs(m_longSpeed) > 5f)
            tau = 0f;

        // スリップ率に代入
        m_slipRatio = m_diffSlipRatio + tau * delta;


        // 荷重をかける前の縦力
        m_longF = m_longForceCurve.Evaluate(m_slipRatio);


        //----------------------------------------------------------------------------
        // 横力の計算
        //----------------------------------------------------------------------------
        // スリップ角
        m_slipAngle = Mathf.Atan(-m_latSpeed / Mathf.Abs(m_longSpeed)) * Mathf.Rad2Deg;
        if (float.IsNaN(m_slipAngle))
            m_slipAngle = 0f;

        // 荷重を掛ける前の横力
        m_latF = m_latForceCurve.Evaluate(m_slipAngle);

    }


    /// <summary>
    /// ルンゲクッタ法4次で  <br/>
    /// f(x,t) = x*t
    /// </summary>>
    float CalcRK4()
    {
        // ローカル関数
        // それほどパフォーマンスには影響はない
        // f(t,y) = (Vsx - |Vx| * y) / B * t 
        float calcDiffSR(float _t,float _y) 
            => ((m_longSlipVelocity - Mathf.Abs(m_longSpeed) * _y) / m_relaxationLength) * _t;

        float dt = Time.deltaTime;
        float h = Time.fixedDeltaTime;

        float k1 = calcDiffSR(dt, m_diffSlipRatio);
        float k2 = calcDiffSR(dt + h / 2,m_diffSlipRatio + h / 2 * k1);
        float k3 = calcDiffSR(dt + h / 2, m_diffSlipRatio + h / 2 * k2);
        float k4 = calcDiffSR(dt + h, m_diffSlipRatio + h * k3);
        return m_diffSlipRatio + (k1 + 2 * k2 + 2 * k3 + k4) * h / 6;
    }

    /// <summary>
    /// 複合力を計算、縦横力を摩擦円に調整する
    /// </summary>
    void CalcCombineForce(out float _outlongF, out float _outLatF)
    {
        // 各スリップのピーク値を取得
        float slipRatio_peak = m_longForceCurve.PeakSlipRatio;
        float slipAngle_peak = m_latForceCurve.PeakSlipAngle;

        // 正規化
        float slipRatio_normalized = m_slipRatio / slipRatio_peak;
        float slipAngle_normalized = m_slipAngle / slipAngle_peak;

        // 三平方の定理です
        // 複合スリップ = √(スリップ率_nor)^2 + (スリップ角_nor)^2
        float combineSlip = Mathf.Sqrt(Mathf.Pow(slipRatio_normalized, 2f) + Mathf.Pow(slipAngle_normalized, 2f));
        // 0除算回避
        if (float.Epsilon > combineSlip)
            combineSlip = float.Epsilon;

        // 修正されたスリップ率とスリップ角
        float fixSlipRatio = combineSlip * slipRatio_peak;
        float fixSlipAngle = combineSlip * slipAngle_peak;

        _outlongF = m_longForceCurve.Evaluate(fixSlipRatio) * (slipRatio_normalized / combineSlip);
        _outLatF = m_latForceCurve.Evaluate(fixSlipAngle) * (slipAngle_normalized / combineSlip);
    }

	void CalcCombineForce_SAE(out float _outLongF, out float _outLatF)
    {
	    // https://www.sae.org/publications/technical-papers/content/2023-01-0684/

		// 各スリップのピーク値を取得
		float slipRatio_peak = m_longForceCurve.PeakSlipRatio;
		float slipAngle_peak = m_latForceCurve.PeakSlipAngle;

		// 正規化
		float slipRatio_norm = m_slipRatio / slipRatio_peak;
		float slipAngle_norm = m_slipAngle / slipAngle_peak;

		// 複合スリップ
		float combineSlip = Mathf.Sqrt(m_slipRatio * m_slipRatio + m_slipAngle * m_slipAngle);
		// 0除算回避
		if (float.Epsilon > combineSlip)
			combineSlip = float.Epsilon;
		// 正規化複合スリップ
		float combineSlip_norm = Mathf.Sqrt(slipRatio_norm * slipRatio_norm + slipAngle_norm * slipAngle_norm);
		
		_outLongF = m_slipRatio / (slipRatio_peak * combineSlip) * m_longForceCurve.Evaluate(combineSlip_norm * slipRatio_peak);
        _outLatF = m_slipAngle / (slipAngle_peak * combineSlip) * m_latForceCurve.Evaluate(combineSlip_norm * slipAngle_peak);

	}

   

	#region RaycastExpansion
	/// <summary>
	/// レイキャストを円形に飛ばす
	/// </summary>
	void RaycastExpansion()
    {

        float radiusOffset = 0.0f;

        for (int i = 0; i <= m_raysNumber; i++)
        {
            // Rayを飛ばす角度を計算
            // Quaternion.AngleAxis(タイヤ角, タイヤの上方向ベクトル)
            // * Quaternion.AngleAxis(現在のレイのナンバー * (レイの最大角 / レイの数)
            // + ((180.0f - レイの最大角) / 2.0f), 右方向ベクトル) * 正面方向ベクトル;
            Vector3 rayDirection
                = Quaternion.AngleAxis(m_steerAngle, transform.up)
                * Quaternion.AngleAxis(i * (m_raysMaxAngle / m_raysNumber)
                + ((180.0f - m_raysMaxAngle) / 2.0f), transform.right) * transform.forward;

            // 現在のレイの方向にタイヤの中心から半径の長さ分、判定を取る
            if (Physics.Raycast(transform.position, rayDirection, out RaycastHit hit, m_radius, m_layerMask))
            {
                Debug.DrawLine(transform.position, hit.point, Color.red);

                // レイの原点から衝突点までの距離を計算
                radiusOffset = Mathf.Max(radiusOffset, m_radius - hit.distance);
            }
            Debug.DrawRay(transform.position, rayDirection * m_orgRadius, Color.green);
        }
        // 新しい半径を計算した分線形補間する
        m_radius = Mathf.LerpUnclamped(m_radius, m_orgRadius + radiusOffset, Time.deltaTime * 10.0f);
    }
    #endregion

    #region Gizmo
    /// <summary>
    /// 選択時のみ呼び出されるGizmo描画用関数
    /// </summary>
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        var forwardOffset = transform.forward * 0.07f;
        var springOffset = transform.up * m_radius;
        Gizmos.DrawLine(transform.position - forwardOffset, transform.position + forwardOffset);
        Gizmos.DrawLine(transform.position - springOffset - forwardOffset, transform.position - springOffset + forwardOffset);
        Gizmos.DrawLine(transform.position, transform.position - springOffset);

        // タイヤを描画
        DrawWheelGizmo(m_radius * transform.lossyScale.x, m_width, m_visual.position, transform.up, transform.forward, transform.right);

        // サスペンションを描画
        DrawSuspensionGizmo();
    }

    /// <summary>
    /// ホイールの形のGizmoを描画
    /// </summary>
    void DrawWheelGizmo(float radius, float width, Vector3 position, Vector3 up, Vector3 forward, Vector3 right)
    {
        Gizmos.color = Color.green;

        var halfWidth = width / 2.0f;
        float theta = 0.0f;
        float x = radius * Mathf.Cos(theta);
        float y = radius * Mathf.Sin(theta);
        Vector3 pos = position + up * y + forward * x;
        Vector3 newPos;

        for (theta = 0.0f; theta <= Mathf.PI * 2; theta += Mathf.PI / 12.0f)
        {
            x = radius * Mathf.Cos(theta);
            y = radius * Mathf.Sin(theta);
            newPos = position + up * y + forward * x;

            Gizmos.DrawLine(pos - right * halfWidth, newPos - right * halfWidth);

            Gizmos.DrawLine(pos + right * halfWidth, newPos + right * halfWidth);

            Gizmos.DrawLine(pos - right * halfWidth, pos + right * halfWidth);

            Gizmos.DrawLine(pos - right * halfWidth, newPos + right * halfWidth);

            pos = newPos;
        }


        
    }

    /// <summary>
    /// サスペンションのGizmoを描画
    /// </summary>
    void DrawSuspensionGizmo()
    {
        Gizmos.color = Color.green;
       
        if (!m_bOnGround)
            Gizmos.color = Color.cyan;

        Gizmos.DrawLine(
            transform.position - transform.up * m_radius,
            transform.position + (transform.up * m_suspensionDistance));

        Gizmos.DrawSphere(transform.position + transform.up * m_suspensionDistance, 0.1f);
    }
    #endregion
}
