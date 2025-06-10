using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameStateManager_Setup : GameStateManagerBase
{
	[SerializeField]
	AsyncSceneChanger m_sceneChanger;

	// 椅子
	WIZMOController m_WIZMOController;
	ChairRideOperator m_rideOff = new ChairRideOperator();

	public override void Initialize()
	{
		// 次のシーンのロードを開始
		m_sceneChanger.StartLoad();

		m_WIZMOController = GameManager.Instance.WIZMO;
	}

	public override void StateUpdate()
	{
		//Debug.Log("StateUpdate[SetUp]");

		// 椅子を乗車位置に移行。
		if(m_WIZMOController != null)
			m_rideOff.RideOff(m_WIZMOController);
	}

	public void OnChangeScene()
	{
		m_sceneChanger.ChangeScene();
		Debug.Log("OnChangeScene");
	}

}
