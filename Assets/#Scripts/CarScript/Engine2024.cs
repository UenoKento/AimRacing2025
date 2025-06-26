/**
 * @file    Engine2024.cs
 * @brief   エンジンの状態を管理する
 * @author  22CU0219 鈴木友也
 * @date    2024/05/17  作成
 *          2024/07/18  エンジンの計算を車速と切り分け
 *          2024/08/28  レブリミッター機能追加
 *          2024/09/10  最終更新
 */
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Engine2024
{
	// そのRPMで生成できる最大のトルクを意味する
	[SerializeField]
	AnimationCurve m_torqueCurve;

	float m_angularVelocity;    // エンジンのフライホイールの角速度
	[SerializeField, ShowInInspector]
	float m_engineRPM;          // 現在のエンジン回転数
	[SerializeField, ShowInInspector]
	float m_effectiveTorque;    // 実効トルク値
	[SerializeField]
	float m_inertia = 0.2f;     // エンジンの慣性モーメント
	[SerializeField]
	Vector2 m_idleRange;         // アイドルのRPMの範囲
	[SerializeField]
	float m_stollRPM;            // エンストするRPM
	[SerializeField]
	float m_limitRPM = 9500f;    // 回転数の限界

	[Header("Friction")]
	[SerializeField]
	float m_frictionAtIdle = 50f;  // アイドル時の摩擦トルク(最小摩擦トルク)
	[SerializeField, Range(0.01f, 0.1f)]
	float m_frictionCoef = 0.012f; // エンジン内摩擦係数
	[SerializeField, Range(0.01f, 0.1f)]
	float m_viscousFrictionCoef = 0.015f;   // エンジン内粘性摩擦係数


	[Header("Rev Limiter")]
	[SerializeField]
	float m_overRevRPM;// レブリミッターが起動するRPM
	[SerializeField, Range(0f, 0.3f)]
	float m_revLimiterDuration = 0.1f;
	float m_lastRevLimiterTime;

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

	// 初期化処理
	public void Initialize()
	{

	}

	public void FixedUpdate(in float _throttle, in float _reactionTorque)
	{
		// 摩擦トルク = 最小摩擦トルク + 摩擦係数 * RPM + (粘性摩擦係数 * )
		float frictionTorque = m_frictionAtIdle + m_frictionCoef * m_angularVelocity + Mathf.Pow(m_viscousFrictionCoef * m_angularVelocity, 2f);

		// スロットル量が１の時に供給される最大のinitialTorqueを求める
		// InitialTorqueとは燃料の圧縮によって生み出されるトルク
		float maxInitialTorque = (m_torqueCurve.Evaluate(m_engineRPM) + frictionTorque);

		// アイドル時のスロットル量調整
		// アイドルRPM範囲内で
		float idleRPM_Max = Mathf.Max(m_idleRange.x, m_idleRange.y);
		float idleRPM_Min = Mathf.Min(m_idleRange.x, m_idleRange.y);

		// アイドル時は摩擦トルクを0にするために燃料が供給され続ける
		float idleFade = Mathf.InverseLerp(idleRPM_Max, idleRPM_Min, m_engineRPM);

		// エンスト時は無効
		if (m_stollRPM > m_engineRPM)
			idleFade = 0f;

		// アイドル時の摩擦処理
		float idleSupply = frictionTorque / maxInitialTorque * idleFade;



		// 調整されたスロットル量
		float adjustThrottle = _throttle + idleSupply;
		// NaN回避
		if (float.IsNaN(adjustThrottle))
			adjustThrottle = 0f;

		// インジェクションカットがtrueなら切る
		if (m_injectionCut)
			adjustThrottle = 0f;

		// Revリミッターで調整されたスロットル量
		adjustThrottle = CalcRevLimiterThrottle(adjustThrottle);


		//Debug.Log("Adjust::" + adjustThrottle + "idlethrottle::" + idleSupply);

		// 実効トルク
		m_effectiveTorque = (maxInitialTorque * adjustThrottle) - frictionTorque;

		if (m_effectiveTorque < 0 && m_engineRPM <= 0) { m_effectiveTorque = 0; }

		// ホイールからの負荷を計算したトルクから角加速度を求める(a = T / I)
		float m_angularAccele = (m_effectiveTorque - _reactionTorque) / m_inertia;
		m_angularVelocity += m_angularAccele * Time.fixedDeltaTime;
		// 角速度をClamp
		m_angularVelocity = Mathf.Clamp(m_angularVelocity, 0f, m_limitRPM);

		m_engineRPM = m_angularVelocity * CarPhysics.Rad2RPM;
	}

	/// <summary>
	/// レブリミッターのスロットル量計算
	/// </summary>
	/// <param name="_throttle">現在のスロットル量</param>
	/// <returns>調整されたスロットル量</returns>
	float CalcRevLimiterThrottle(in float _throttle)
	{
		// スロットル量調整用
		float revLimiterSupply = 1f;

		if (m_engineRPM < m_overRevRPM)
		{
			// 経過時間
			float elapsedTime = Time.time - m_lastRevLimiterTime;

			// 間隔を越えるまでスロットル量を0に調整する
			if (elapsedTime > m_revLimiterDuration)
				revLimiterSupply = 1f;
			else
				revLimiterSupply = 0f;
		}
		else
		{
			revLimiterSupply = 0f;
			m_lastRevLimiterTime = Time.time;
		}

		return _throttle * revLimiterSupply;
	}
}
