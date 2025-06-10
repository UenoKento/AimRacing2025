/**
 * @file    Load2024.cs
 * @brief   タイヤにかかる荷重移動を計算する機能        
 * @author  22CU0225豊田達也
 * @date    2024/05/31 作成
 *          2024/06/04 [豊田達也] タイヤと重心までの距離の計算  作成
 *          2024/06/05 [豊田達也] 荷重割合の計算               作成 
 *          2024/06/05 [豊田達也] 荷重分配の計算               作成 
 *          2024/06/06 [豊田達也] 荷重割合の計算               更新  
 *          2024/06/13 [豊田達也] 前後の荷重割合の計算        更新
 *          2024/06/18 [豊田達也] 荷重割合の計算               更新  
 *          2024/06/26 [豊田達也] 傾きの荷重割合の計算        更新  
 *          2024/06/26 [豊田達也] 荷重割合の計算               更新
 *          2024/06/28 [豊田達也] 速度の取得処理               更新
 *          2024/07/03 [豊田達也] 荷重計算の制限追加・Ray調整   更新
 *          2024/07/04 [豊田達也] 荷重計算 速度の取り方       更新
 * @Link    https://dskjal.com/car/math-car-weight.html
 *          https://www.autoexe.co.jp/kijima/column16.html
 *          https://www.autoexe.co.jp/kijima/column13.html
 *          https://suspensionsecrets.co.uk/lateral-and-longitudinal-load-transfer/
 */

// メモ
// Unityの単位はメートル　1 = 1m
// VehicleにAddComponentすること

using UnityEngine;

[AddComponentMenu("AIM/Load")]
public class Load2024 : MonoBehaviour
{
    #region Inspecterに表示

    [Header("Vehicle")]
    [SerializeField] private GameObject m_vehicle = null;

    // ロールセンター (地面までの重心距離 + タイヤ半径/2)
    [SerializeField] private float m_rollcenterLange = 0.3f + -0.334f / 2;

    [Header("wheel")]
    [SerializeField] private WheelController2024 m_wheelController_FR;        // 前輪　右
    [SerializeField] private WheelController2024 m_wheelController_FL;        // 前輪　左
    [SerializeField] private WheelController2024 m_wheelController_RR;        // 後輪　右
    [SerializeField] private WheelController2024 m_wheelController_RL;        // 後輪　左

    [Header("Ray")]
    [SerializeField] private LayerMask m_layerMask = Physics.IgnoreRaycastLayer;     // レイヤーマスク
    [SerializeField] private float m_maxDistance = 1;        // Rayの計測可能な最大距離 長くなくていいので1

    // 制限値
    [Header("Limit")]
    [SerializeField] private float m_minLoadPer = 0f;
    [SerializeField] private float m_maxLoadPer = 1f;
    [Space]
    [SerializeField] private float m_minAcceleration = 0.1f;
    [SerializeField] private float m_maxAcceleration = 4f;
    [Space]
    [SerializeField] private float m_minCentrifugalForce = 0.1f;
    [SerializeField] private float m_maxCentrifugalForce = 4f;
    [Space]
    [SerializeField] private float m_minAngle = 10f;             // 角度使うなら1からが良い　一時的角度無視するために10
    [SerializeField] private float m_maxAngle = 45f;             // 車の限界

    [Header("Load")] // Rangeで視覚化
    [SerializeField][Range(0, 1)] private float m_load_FR;        // 分配荷重 FR
    [SerializeField][Range(0, 1)] private float m_load_FL;        // 分配荷重 FL
    [SerializeField][Range(0, 1)] private float m_load_RR;        // 分配荷重 RR
    [SerializeField][Range(0, 1)] private float m_load_RL;        // 分配荷重 RL
    [Space]
    [SerializeField][Range(0, 1)] private float m_load_Front;         // 前荷重割合
    [SerializeField][Range(0, 1)] private float m_load_Rear;          // 後荷重割合

    #endregion

    #region Inspecterに非表示
    // VehicleController
    private VehicleController2024 m_vehicleController;

    // 車の剛体
    private Rigidbody m_vehicleRigitbody;   // 車の剛体

    // Ray
    private float m_fly = -1;           // 触れていないとき
    private Vector3 m_graundNomal;      // hit地点の地面の法線ベクトル

    // 距離
    private float m_wheelBase;          // ホイールベース
    private float m_treadFront;         // 前輪トレッド
    private float m_treadRear;          // 後輪トレッド

    private float m_distace_Rear;       // 後輪までの長さ
    private float m_langeHight;         // 地面からの高さ
    
    // 速度
    private Vector3 m_velocity;                 // 速度
    private Vector3 m_prevVelocity;             // 前速度

    // 車重
    private float m_weight;

    // 重心にかかる力
    private float m_accel;          // 加減速
    private float m_centrifugal;    // 遠心力

    private float m_forwardSpeed;
    #endregion

    #region プロパティ
    public float Loat_Front => m_load_Front;
    public float Loat_Rear => m_load_Rear;
    public float Loat_Right => m_load_FR;
    public float Loat_Left => m_load_FL;

    public float ForwardSpeed => m_forwardSpeed;

    public float Accel => m_accel;
    public float Centrifugal => m_centrifugal;
    #endregion

    // AddComponent/Reset したときの設定
    private void Reset()
    {
        m_vehicle = gameObject;
        GameObject childWheelColliders = gameObject.transform.Find("WheelColliders").gameObject;
        m_wheelController_FR = childWheelColliders.transform.Find("WheelController_FR").GetComponent<WheelController2024>();
        m_wheelController_FL = childWheelColliders.transform.Find("WheelController_FL").GetComponent<WheelController2024>();
        m_wheelController_RR = childWheelColliders.transform.Find("WheelController_RR").GetComponent<WheelController2024>();
        m_wheelController_RL = childWheelColliders.transform.Find("WheelController_RL").GetComponent<WheelController2024>();
        m_layerMask = 1 << LayerMask.NameToLayer("Road");
    }
    // 最初に一度だけ呼ばれる関数
    void Start()
    {
        Initialize();

        // タイヤの距離を計算
        CalcWheelLenge();
    }
    // 初期設定
    private void Initialize()
    {
        #region 初期化
        m_wheelBase = 0.0f;
        m_treadFront = 0.0f;
        m_treadRear = 0.0f;
        m_distace_Rear = 0.0f;
        m_langeHight = 0.0f;
        m_load_FR = 0.0f;
        m_load_FL = 0.0f;
        m_load_RR = 0.0f;
        m_load_RL = 0.0f;
        m_prevVelocity = Vector3.zero;
        m_graundNomal = Vector3.zero;
        #endregion
        m_vehicle = this.gameObject;
        m_vehicleController = m_vehicle.GetComponent<VehicleController2024>();
        m_vehicleRigitbody = m_vehicle.GetComponent<Rigidbody>();
        // 車重の設定
        m_weight = m_vehicleRigitbody.mass * Physics.gravity.magnitude;
    }

    // 毎フレーム更新される関数
    void Update()
    {
        UpdateProcess();
    }

    // 切り替えるとき用 Inspectorで変えれるようにしてもいい
    //private void FixedUpdate()
    //{
    //    UpdateProcess();
    //}

    private void UpdateProcess()
    {
        #region 車が正しく設定されているか確認
        if (m_vehicle.tag == null)  // Nullチェック
        {
            Debug.LogError("荷重移動で使う車が設定されていません/Load2024.cs/");

            // 荷重を四分割
            m_load_FR = m_load_FL = m_load_RR = m_load_RL = m_weight / 4;
            m_wheelController_FR.Load = m_load_FR;
            m_wheelController_FL.Load = m_load_FL;
            m_wheelController_RR.Load = m_load_RR;
            m_wheelController_RL.Load = m_load_RL;
            return;
        }
        #endregion

        // Rayを飛ばして地面との判定をする
        RaycastToGraund();
        // 荷重移動の計算
        LoadTransfre();
        // 荷重の分配
        LoadApply();
    }

    #region 距離計算
    // Rayを飛ばして地面との判定をする
    private void RaycastToGraund()
    {
        // 現在位置の位置から、local下方向に、レイで取得する
        RaycastHit hit;
        Vector3 startpos =  m_vehicleRigitbody.worldCenterOfMass;
        Debug.DrawRay(startpos, -transform.up* m_maxDistance, Color.green);
        if (Physics.Raycast(startpos, -transform.up, out hit, m_maxDistance, m_layerMask))
        {
            // hit地点の法線ベクトル
            m_graundNomal = hit.normal;
            // 地面からの高さ (hit位置 + 重心高さ)
            m_langeHight = hit.distance;
            //Debug.Log("hit地点の法線ベクトル :" + m_graundNomal);
            //Debug.Log("地面からの高さ :" + m_langeHight);
        }
        else
        {
            m_langeHight = m_fly;
            Debug.LogError("地面に触れていません");
        }        
    }

    // タイヤの距離を計算
    private void CalcWheelLenge()
    {
        // ホイールベース (|FR.localPosition.z| + |RR.localPosition.z|)
        m_wheelBase = Mathf.Abs(m_wheelController_FR.transform.localPosition.z) + Mathf.Abs(m_wheelController_RR.transform.localPosition.z);
        // 前輪トレット   (|FR.localPosition.z| + |FL.localPosition.z|)
        m_treadFront = Mathf.Abs(m_wheelController_FR.transform.localPosition.x) + Mathf.Abs(m_wheelController_FL.transform.localPosition.x);
        // 後輪トレット   (|RR.localPosition.z| + |RL.localPosition.z|)
        m_treadRear = Mathf.Abs(m_wheelController_RR.transform.localPosition.x) + Mathf.Abs(m_wheelController_RL.transform.localPosition.x);
        // 重心から後輪までの距離   (|RR.localPosition.z - CoG.Position.z|)
        m_distace_Rear = Mathf.Abs(m_wheelController_RR.transform.localPosition.z - m_vehicleRigitbody.centerOfMass.z);
        //Debug.Log("ホイールベース :" + m_wheelBase);
        //Debug.Log("前輪トレッド :" + m_treadFront);
        //Debug.Log("後輪トレッド :" + m_treadRear);
        //Debug.Log("重心から後輪までの距離 :" + m_distace_Rear);
    }
    #endregion

    #region 荷重計算

    // @Link https://dskjal.com/car/math-car-weight.html
    //       https://www.autoexe.co.jp/kijima/column16.html
    //       https://www.autoexe.co.jp/kijima/column13.html
    //       https://suspensionsecrets.co.uk/lateral-and-longitudinal-load-transfer/

    // 荷重移動の計算
    private void LoadTransfre()
    {
        // 地面に設置していないとき
        if (m_langeHight == m_fly)
        {
            // 荷重をゼロに
            m_load_FR = m_load_FL = m_load_RR = m_load_RL = 0;
            m_wheelController_FR.Load = m_load_FR;
            m_wheelController_FL.Load = m_load_FL;
            m_wheelController_RR.Load = m_load_RR;
            m_wheelController_RL.Load = m_load_RL;
            return;
        }

        // 速度の取得
        m_velocity = m_vehicleRigitbody.linearVelocity;

        // 縦方向の荷重移動
        LongitudinalLoadTransfer();
        // 横方向の荷重移動
        LateralLoadTransfer();

        // 荷重割合を出す
        // 前
        m_load_FR *= m_load_Front;
        m_load_FL *= m_load_Front;
        // 後
        m_load_RR *= m_load_Rear;
        m_load_RL *= m_load_Rear;

        // 速度の保存
        m_prevVelocity = m_velocity;
    }

    // 縦方向の荷重移動
    private void LongitudinalLoadTransfer()
    {
        #region 計算に必要な変数

        // シータ
        // 車のピッチ方向の傾き 
        // 地面の法線ベクトルと車の下向きのベクトルの角度を求める
        float theta = Mathf.Abs(Vector3.SignedAngle(-transform.up, -m_graundNomal, Vector3.forward));
        //Debug.Log("ピッチシータ" + theta);

        // 正面方向ベクトル
        Vector3 forward = gameObject.transform.forward;
        // 正面方向ベクトルと速度ベクトルの内積を正面方向ベクトルとかけて前方速度ベクトルを取得
        Vector3 forwardVelocity = Vector3.Dot(m_velocity, forward) * forward;
        m_forwardSpeed = forwardVelocity.magnitude;

        Vector3 prevForwardVelocity = Vector3.Dot(m_prevVelocity, forward) * forward;
        // 加速度 [G]　[Vf - V0f / t / g ]
        float acceleration = (forwardVelocity.magnitude - prevForwardVelocity.magnitude) / Time.deltaTime / Physics.gravity.magnitude;
        m_accel = acceleration; // 値保持
        //Debug.Log("加速度" + acceleration);
        #endregion

        // 荷重の前後割合の計算 (傾き - 加減速)
        m_load_Front = LoadToLongitudinalTilt() - LoadToAcceleration();
        m_load_Rear = 1 - (LoadToLongitudinalTilt() - LoadToAcceleration());    // 全体の荷重を100％として前荷重の反転

        //Debug.Log("前後傾き" + LoadToLongitudinalTilt());
        //Debug.Log("加減速" + LoadToAcceleration());

        // 前後の傾きによる荷重割合
        float LoadToLongitudinalTilt()
        {
            // ほぼ傾きがない場合
            if(theta < m_minAngle)
            {
                // 半分
                return 0.5f;
            }
            // 傾きが制限を超えた場合
            else if(theta > m_maxAngle)
            {
                Debug.LogError("傾きすぎ");
                return 0f; // 荷重なし
            }
            // 傾きが制限以上の時
            /* 計算式
             * L：ホイールベース[m]
             * lr：重心から後輪までの距離[m]
             * h：地面から重心までの高さ[m]
             * θ:地面の傾き[rad]
             * 
             * FR = (lr - hθ)/L
             */

            // 前輪にかかる荷重割合
            return ((m_distace_Rear - m_langeHight * (theta * Mathf.Deg2Rad)) / m_wheelBase);
        }

        // 加減速による荷重割合
        float LoadToAcceleration()
        {
            // ほぼ加減速がない場合
            if(Mathf.Abs(acceleration) < m_minAcceleration)
            {
                return 0f;   // 切り捨て
            }
            // 加減速が制限を超えた場合
            else if (Mathf.Abs(acceleration) > m_maxAcceleration)
            {
                //Debug.Log("加減速オーバー"+ acceleration);
                // 仮数値
                return 0.5f;
            }
            

            /* 計算式
             * L：ホイールベース[m]
             * h：地面から重心までの高さ[m]
             * Ax：縦方向加速度[G]
             * 
             * FrontLoad = h/L * Ax
             */

            // 前輪にかかる荷重割合
            return (m_langeHight / m_wheelBase) * acceleration;
        }
    }



    // 横方向の荷重移動
    private void LateralLoadTransfer()
    {
        #region 計算に必要な変数

        // シータ
        // 車のロール方向の傾きの取得
        float theta = Mathf.Abs(Vector3.SignedAngle(-transform.up, -m_graundNomal, Vector3.left));
        //Debug.Log("ロールシータ" + theta);

        // 右方向ベクトル
        Vector3 sideway = gameObject.transform.right;
        // 右方向ベクトルと速度ベクトルの内積を右方向ベクトルとかけて右速度ベクトルを取得
        Vector3 sidewayVelocity = Vector3.Dot(m_velocity, sideway) * sideway;
        Vector3 prevSidewayVelocity = Vector3.Dot(m_prevVelocity, sideway) * sideway;

        // 遠心力（向心加速度） [G] [Vs - V0s / t / g ]
        // コントローラーの入力によって左右判断 また　遠心力なので反転
        float centrifugalForce = -1 * Mathf.Sign(m_vehicleController.Steering) * (sidewayVelocity.magnitude - prevSidewayVelocity.magnitude) / Time.deltaTime / Physics.gravity.magnitude;
        m_centrifugal = centrifugalForce;   // 値保持
        //Debug.Log("遠心力" +  centrifugalForce);
        #endregion

        // FRにかかる荷重
        m_load_FR = LoadToLateralTilt(m_treadFront) - CentrifugalForceLoad(m_treadFront);
        // FLにかかる荷重
        m_load_FL = 1 - (LoadToLateralTilt(m_treadFront) - CentrifugalForceLoad(m_treadFront));     // 全体の荷重を100％として右荷重の反転

        // RRにかかる荷重
        m_load_RR = LoadToLateralTilt(m_treadRear) - CentrifugalForceLoad(m_treadRear);
        // RLにかかる荷重
        m_load_RL = 1 - (LoadToLateralTilt(m_treadRear) - CentrifugalForceLoad(m_treadRear));

        //Debug.Log("左右　前 傾き" + LoadToLateralTilt(m_treadFront));
        //Debug.Log("左右　前 遠心力" + CentrifugalForceLoad(m_treadFront));
        //Debug.Log("左右　後 傾き" + LoadToLateralTilt(m_treadRear));
        //Debug.Log("左右　後 遠心力" + CentrifugalForceLoad(m_treadRear));

        // 左右の傾きによる荷重割合
        float LoadToLateralTilt(float _tread)
        {
            // ほぼ傾きがない場合
            if (theta < m_minAngle)
            {
                // 半分
                return 0.5f;
            }
            // 傾きが制限を超えた場合
            else if (theta > m_maxAngle)
            {
                Debug.LogError("傾きすぎ");
                return 0f; // 荷重なし
            }

            /* 計算式
             * t：トレッド[m]
             * h：地面から重心までの高さ[m]
             * θ：車の傾き[°]
             * 
             * FR/RR =  (t/2-h*tanθ) /t
             */

            return (_tread / 2 - m_langeHight * (theta * Mathf.Deg2Rad)) / _tread;
        }

        // 遠心力による荷重割合
        float CentrifugalForceLoad(float _tread)
        {
            // ほぼ遠心力がない場合
            if (Mathf.Abs(centrifugalForce) < m_minCentrifugalForce)
            {
                return 0f; // 切り捨て
            }
            // 遠心力が制限を超えた場合
            else if (Mathf.Abs(centrifugalForce) > m_maxCentrifugalForce)
            {
                //Debug.Log("遠心力オーバー"+ centrifugalForce);
                // 仮数値
                return 0.5f;
            }

            /* 計算式
             * t：トレッド[m]
             * h：ロールセンターから重心までの高さ[m]
             * Ay：横方向加速力（遠心力）[G]
             * 
             * FR/RR = (Ay*h)/ t
             * 
             */

            return centrifugalForce * m_rollcenterLange / _tread;
        }
    }
    #endregion

    // 荷重の分配
    private void LoadApply()
    {
        // 重量に割合をかけて値を渡す
        m_wheelController_FR.Load = m_weight * Mathf.Clamp(m_load_FR, m_minLoadPer, m_maxLoadPer);
        m_wheelController_FL.Load = m_weight * Mathf.Clamp(m_load_FL, m_minLoadPer, m_maxLoadPer);
        m_wheelController_RR.Load = m_weight * Mathf.Clamp(m_load_RR, m_minLoadPer, m_maxLoadPer);
        m_wheelController_RL.Load = m_weight * Mathf.Clamp(m_load_RL, m_minLoadPer, m_maxLoadPer);
    }
}