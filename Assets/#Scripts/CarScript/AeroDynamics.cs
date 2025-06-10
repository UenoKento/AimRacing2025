/**
 * @file    AearoDynamics.cs
 * @brief   空力を計算してAddForceする
 * @author  22CU0219 鈴木友也
 * @date    2024/07/21  作成
 *          2024/08/08  最終更新
 */

using UnityEngine;

public class AearoDynamics : MonoBehaviour
{
    [SerializeField]
    Rigidbody m_vehicleRigidbody;
    [SerializeField]
    float m_rho = 1.293f;   // 空気密度ρ(基準状態:1.293[kg/m^3])

    [Header("AirDrag")]
    [SerializeField,Range(0.04f,1f)]
    float m_dragCoeff;      // 空気抵抗係数(物体の形状によって決まる)
    [SerializeField]
    float m_frontArea;      // 車体の前方投影面積(m^2)

    [Header("DownForce")]
    [SerializeField]
    float m_downForceCoeff; // Cl*A (揚力係数*ウィング面積)の値
                            // 詳細な数値を確かめるのが難しいので
                            // F1だとCl*Aは5.5くらいなのでそれを基準に設定

	// AddComponent/Reset したときの設定
	void Reset()
	{
		m_vehicleRigidbody = GetComponentInParent<Rigidbody>();
	}

	void Start()
    {
		if (m_vehicleRigidbody == null)
			Debug.LogError("RigidBodyが設定されていません[AeroDynamics]");
	}

    // Update is called once per frame
    void FixedUpdate()
    {
		float forwardSpeed = Vector3.Dot(m_vehicleRigidbody.linearVelocity,m_vehicleRigidbody.transform.forward);

        Vector3 dragForceDir = -m_vehicleRigidbody.transform.forward;
        // 空気抵抗力 = 0.5 * Cd:空気抵抗係数 * A:前方面積 * ρ:空気密度 * v^2:速度
        float dragForce = 0.5f * m_dragCoeff * m_frontArea * m_rho * forwardSpeed * forwardSpeed;

		Vector3 downForceDir = -m_vehicleRigidbody.transform.up;
        // ダウンフォース = 0.5 * Cl:揚力係数 * A:ウィング面積 * ρ:空気密度 * v^2:速度
        float downForce = 0.5f * m_downForceCoeff * m_rho * forwardSpeed * forwardSpeed;

        m_vehicleRigidbody.AddForce(dragForceDir * dragForce + downForceDir * downForce);
    }
}
