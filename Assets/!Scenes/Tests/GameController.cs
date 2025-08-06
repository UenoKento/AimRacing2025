/*************************************************************************************************
 * 入力情報をゲーム内に伝えるコントローラークラスを定義
 * 
 * 作成者	: 23cu0203 石井隼人
 * 最終更新	: 2025/06/16
 ************************************************************************************************/
using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;


namespace JEC.Aimracing2025
{
	public class GameController
	{
		[SerializeField] private string[] _bindKeys;		// 入力値の名前

		public float HandleRate { get; private set; }
		public float AccelRate { get; private set; }
		public float BrakeRate { get; private set; }

		public event Action OnGearUp;
		public event Action OnGearDown;


		public void UpdateSystem()
		{

		}

	} // GameController

} // JEC.Aimracing
