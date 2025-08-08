using UnityEngine;
using System.Collections;

public class WIZMOExample : MonoBehaviour
{
	public WIZMOController WIZMOSystemObject = null;

	public float settime_update = 2.0f;
	public float nowtime_update = 0.0f;

	private bool dir = false;

	// Use this for initialization
	void Start()
	{
		if (WIZMOSystemObject == null)
			WIZMOSystemObject = this.gameObject.GetComponent<WIZMOController>();
		if (WIZMOSystemObject == null)
			Debug.LogError("WIZMO System object not specified.");
	}

	//エラー処理
	public void MachineErrorEvent(string errorMessage)
	{
		Debug.Log("The machine has encountered an error: " + errorMessage);
		switch (errorMessage)
		{
			case "MachineNotDetected":
				//マシン未検出
				break;
			case "InaccessibleToTheMachine":
				//マシンへのアクセス不可
				break;
			case "ZeroReturnFailure":
				//原点復帰失敗
				break;
			case "ErrorReturningToZero":
				//原点復帰中のエラー
				break;
			case "ShutDown":
				//シャットダウン
				break;
			case "AuthenticationFailure":
				//認証失敗
				break;
			case "InternalDisconnectionError":
				//内部断線エラー
				break;
			case "OverloadError":
				//過負荷エラー
				break;
		}
	}


	//処理
	void Update()
	{
		if (WIZMOSystemObject == null)
			return;

		nowtime_update -= Time.deltaTime;

		if (nowtime_update < 0.0f)
		{
			dir ^= true;
			if (dir)
			{
				WIZMOSystemObject.axisProcesser = false;
				WIZMOSystemObject.axis1 = 0.0f;
				WIZMOSystemObject.axis2 = 0.0f;
				WIZMOSystemObject.axis3 = 0.0f;
				WIZMOSystemObject.axis4 = 0.0f;
				WIZMOSystemObject.axis5 = 0.0f;
				WIZMOSystemObject.axis6 = 0.0f;
			}
			else
			{
				WIZMOSystemObject.axisProcesser = false;
				WIZMOSystemObject.axis1 = 1.0f;
				WIZMOSystemObject.axis2 = 1.0f;
				WIZMOSystemObject.axis3 = 1.0f;
				WIZMOSystemObject.axis4 = 1.0f;
				WIZMOSystemObject.axis5 = 1.0f;
				WIZMOSystemObject.axis6 = 1.0f;
			}
			nowtime_update = settime_update;
		}
	}

}
