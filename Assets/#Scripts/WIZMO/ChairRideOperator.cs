/**
 * @file    ChairRideOperator.cs
 * @brief   �֎q�̏�~�ʒu��ݒ肷��N���X     
 * @author  22CU0225 �L�c�B��
 * @date    2024/09/06 �쐬
 */
using UnityEngine;

/// <summary>
/// new���Ȃ���WIZMO����ł���
/// </summary>
[System.Serializable]
public class ChairRideOperator
{
    // ��Ԉʒu
    public void Ride(WIZMOController _controller)
    {
        _controller.accel = 0.1f;
        _controller.speed1_all = 0.1f;
        _controller.roll = 0f;
        _controller.pitch = 0f;
        _controller.yaw = 0f;
        _controller.heave = 1f;
        _controller.sway = 0f;
        _controller.surge = 0f;
    }

    public void Drive(WIZMOController _controller)
	{
		_controller.accel = 0.1f;
		_controller.speed1_all = 0.1f;
		_controller.roll = 0f;
		_controller.pitch = 0f;
		_controller.yaw = 0f;
		_controller.heave = 0.5f;
		_controller.sway = 0f;
		_controller.surge = 0f;
	}

	// �~�Ԉʒu
	public void RideOff(WIZMOController _controller)
    {
        _controller.accel = 0.1f;
        _controller.speed1_all = 0.1f;
        _controller.roll = 0f;
        _controller.pitch = 0f;
        _controller.yaw = -1f;
        _controller.heave = 1f;
        _controller.sway = 0f;
        _controller.surge = 0f;
    }
}
