using UnityEngine;
using static VehiclePhysics.VPTelemetry;

/**
 * @file    Suspension.cs
 * @brief   サスペンション(衝撃減衰、荷重計算)に関する計算
 * @author  23CU0110 小見川　治輝
 * @date    2025/04/25  作成
 *          2025/04/25 最終更新
 */

public class Suspension : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    WheelController2024 m_WheelController;  //ホイールコントローラー

    //WheelControllerから取得する情報
    RaycastHit m_raycastHit;                //レイキャスト情報
    float m_WheelRadius;                    //ホイール半径

    [SerializeField]
    Rigidbody m_CarRigid;                   //車のrigidbody情報

    [SerializeField]
    Transform m_Car_Visualtransform;        //メッシュの位置情報



    [Header("Suspension_Settings")]
    [SerializeField]
    float m_spring = 50000.0f;              // ターゲット位置に到達するためのバネ力
    [SerializeField]
    float m_damper = 10000.0f;              // バネの振動を減衰させる力(ダンパー力)
    [SerializeField]
    float m_suspensionDistance = 0.05f;     // サスペンションの最大伸長距離(ローカル座標)


    float Spring
    {
        get
        {
            return m_spring;
        }
        set
        {
            // 0以上に補正
            m_spring = Mathf.Max(0, value);
        }
    }
    float Damper
    {
        get
        {
            return m_damper;
        }
        set
        {
            // 0以上に補正
            m_damper = Mathf.Max(0, value);
        }
    }

    [Header("SuspensionLoad")]
    [SerializeField, ShowInInspector]
    float m_suspensionLoad;                  // サスペンションから計算した上下荷重


    void Start()
    {
        // 初期荷重(四等分)
        m_suspensionLoad = m_CarRigid.mass / 4f * Physics.gravity.magnitude;

        m_WheelRadius = m_WheelController.GetWheelRadius();

    }

    void FixedUpdate()
    {

        m_raycastHit = m_WheelController.GetRayCastHit();

        if (m_WheelController.GetOnGround())
        {
            UpdateSuspension();
        }
    }

    void UpdateSuspension()
    {
        // ローカルの下方向をワールドに変換
         Vector3 down = transform.TransformDirection(Vector3.down);

        // 車輪の回転を考慮せずに、車輪が地面に対してどのくらいの速さで動いているかを計算する。
        Vector3 velocityAtTouch = m_CarRigid.GetPointVelocity(m_raycastHit.point);

        // スプリングの圧縮を計算する
        // 位置の差をサスペンションの全範囲で割る
        float compression = m_raycastHit.distance / (m_suspensionDistance + m_WheelRadius);
        //Debug.Log("01 compression : " + compression);
        compression = -compression + 1;
        //Debug.Log("02 compression : " + compression);

        // 最終的な力
        Vector3 force = -down * compression * Spring;
        //Debug.Log("force : " + force);

        // 接触点の速度をローカル空間に変換したもの
        Vector3 Suspension_LocalVelocity = transform.InverseTransformDirection(velocityAtTouch);
        //Debug.Log("t : " + t);

        // ローカルXおよび、Z方向 = 0
        // ここで、tはショックが収縮/膨張する速度と等しいとする。
        Suspension_LocalVelocity.z = 0;
        Suspension_LocalVelocity.x = 0;

        // ワールド空間 * 減衰
        // この力はサスペンションの摩擦による力をシミュレートしています。
        Vector3 shockDrag = transform.TransformDirection(Suspension_LocalVelocity) * -Damper;

        // 
        m_CarRigid.AddForceAtPosition(force + shockDrag, transform.position);
        m_suspensionLoad = (force + shockDrag).magnitude;

        m_Car_Visualtransform.position = transform.position + (down * (m_raycastHit.distance - m_WheelRadius));
    }



    public float GetSuspensionDistance()
    {
        return m_suspensionDistance;
    }

    public float GetSuspensionLoad()
    {
        return m_suspensionLoad;
    }

    public void SetSuspensionLoad(float LoadValue)
    {
        m_suspensionLoad = LoadValue;
    }

}
