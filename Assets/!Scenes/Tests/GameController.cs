/*************************************************************************************************
 * ���͏����Q�[�����ɓ`����R���g���[���[�N���X���`
 * 
 * �쐬��	: 23cu0203 �Έ䔹�l
 * �ŏI�X�V	: 2025/06/16
 ************************************************************************************************/
using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;


namespace JEC.Aimracing2025
{
	public class GameController
	{
		[SerializeField] private string[] _bindKeys;		// ���͒l�̖��O

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
