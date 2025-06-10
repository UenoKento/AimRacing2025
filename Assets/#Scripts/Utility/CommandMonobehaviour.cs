using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CUEngine.Pattern;

public class CommandMonobehaviour : StateMonobehavior
{
	[SerializeField]
	private int graceFrame;
	private int frame;

	protected override void Start()
	{
		base.Start();
		frame = graceFrame;
	}

	public override void UpdateGame()
	{
		base.UpdateGame();
		if (stateMove)
		{
			//DebugPrint.Print(stateProcessor.State.getStateName());
			frame = graceFrame;
		}
		if (stateProcessor.State != states[0])
		{
			frame--;
		}
		if (frame <= 0)
		{
			stateProcessor.State = states[0];
		}
	}
}
