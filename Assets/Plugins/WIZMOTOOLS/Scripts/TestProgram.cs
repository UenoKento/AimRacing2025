using UnityEngine;
using System.Collections;

public class TestProgram : MonoBehaviour {

	private WIZMOController axis;
	private float updateTimes;
	private int g_count;
	public float updatetime;

	private float[] prog1 = { -1.0f, 1.0f, -1.0f, 1.0f, -1.0f, 1.0f, -1.0f, 1.0f, -1.0f, 1.0f, -1.0f, 1.0f, -1.0f, 1.0f, -1.0f, 1.0f, -1.0f, 1.0f };
	private int g_prog2;

	// Use this for initialization
	void Start () {
		axis = this.GetComponent<WIZMOController>();
		g_count = 0;
		g_prog2 = 0;
		updateTimes = updatetime;
	}
	
	// Update is called once per frame
	void Update () {
		updateTimes -= Time.deltaTime;
		if(updateTimes < 0.0f) {
			switch(g_prog2) {
				case 0:
					axis.heave = 0.0f;
					axis.surge = prog1[g_count++];
					axis.sway = 0.0f;
					axis.yaw = 0.0f;
					break;
				case 1:
					axis.heave = prog1[g_count++];
					axis.surge = 0.0f;
					axis.sway = 0.0f;
					axis.yaw = 0.0f;
					break;
				case 2:
					axis.heave = 0.0f;
					axis.surge = 0.0f;
					axis.sway = prog1[g_count++];
					axis.yaw = 0.0f;
					break;
				case 3:
					axis.heave = 0.0f;
					axis.surge = 0.0f;
					axis.sway = 0.0f;
					axis.yaw = prog1[g_count++];
					break;
			}

			if (g_count >= prog1.Length)
			{
				g_count = 0;
				g_prog2++;
				if (g_prog2 > 3)
					g_prog2 = 0;
			}

			updateTimes = updatetime;
		}
	}
}
