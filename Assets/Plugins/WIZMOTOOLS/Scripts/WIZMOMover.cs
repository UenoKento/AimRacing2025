using UnityEngine;
using System.Collections;

[RequireComponent(typeof(WIZMOController))]
[AddComponentMenu("WIZMO/WIZMOMover")]
public class WIZMOMover : MonoBehaviour
{
	private WIZMOController ctrl = null;

	//Tracking object
	public GameObject trackingObject;
	public Vector3 worldScaleMeter = new Vector4(1.0f,1.0f,1.0f);

    public enum MoverUpdateMethod
    {
		Update = 0,
		FixedUpdate = 1
	}
	public MoverUpdateMethod updateMethod;

	//Axis Processing
	[Range(-1.0f, 1.0f)]
	public float rollForce = 0.0f;
	[Range(-1.0f, 1.0f)]
	public float pitchForce = 0.0f;
	[Range(-1.0f, 1.0f)]
	public float yawForce = 0.0f;
	[Range(-1.0f, 1.0f)]
	public float heaveForce = 0.0f;
	[Range(-1.0f, 1.0f)]
	public float swayForce = 0.0f;
	[Range(-1.0f, 1.0f)]
	public float surgeForce = 0.0f;

    private Vector3 previousPos;
	private float previousYaw;
	private Vector3 previousVec;

	private class Status6DOF
	{
		//Savitzky-Golay Smooth algorithm
		const int SAVITZKY_GOLAY_SMOOTH_MAXCOUNT = 8;
		float[] smoothPoint = new float[SAVITZKY_GOLAY_SMOOTH_MAXCOUNT]{ 0.41667f, 0.33333f, 0.25f, 0.16667f, 0.08333f, 0.0f, -0.08333f, -0.16667f };
		int smoothPointCurrent;

		float[] s_smoothRoll = new float[SAVITZKY_GOLAY_SMOOTH_MAXCOUNT];
		float[] s_smoothPitch = new float[SAVITZKY_GOLAY_SMOOTH_MAXCOUNT];
		float[] s_smoothYaw = new float[SAVITZKY_GOLAY_SMOOTH_MAXCOUNT];
		float[] s_smoothHeave = new float[SAVITZKY_GOLAY_SMOOTH_MAXCOUNT];
		float[] s_smoothSway = new float[SAVITZKY_GOLAY_SMOOTH_MAXCOUNT];
		float[] s_smoothSurge = new float[SAVITZKY_GOLAY_SMOOTH_MAXCOUNT];

		public Status6DOF()
		{
			smoothPointCurrent = 0;
		}

		public void MaxStatusUpdate(float roll, float pitch, float yaw, float heave, float sway, float surge)
        {
			s_smoothRoll[smoothPointCurrent] = roll;
			s_smoothPitch[smoothPointCurrent] = pitch;
			s_smoothYaw[smoothPointCurrent] = yaw;
			s_smoothHeave[smoothPointCurrent] = heave;
			s_smoothSway[smoothPointCurrent] = sway;
			s_smoothSurge[smoothPointCurrent] = surge;

			++smoothPointCurrent;
			if (smoothPointCurrent >= SAVITZKY_GOLAY_SMOOTH_MAXCOUNT)
				smoothPointCurrent = 0;
		}

		public float Roll()
        {
			float result = 0.0f;
			int index = smoothPointCurrent;
			for (int i = 0; i < SAVITZKY_GOLAY_SMOOTH_MAXCOUNT; ++i)
			{
				result += s_smoothRoll[index] * smoothPoint[i];
				++index;
				if (index >= SAVITZKY_GOLAY_SMOOTH_MAXCOUNT) index = 0;
			}

			return result;
		}
		public float Pitch()
		{
			float result = 0.0f;
			int index = smoothPointCurrent;
			for (int i = 0; i < SAVITZKY_GOLAY_SMOOTH_MAXCOUNT; ++i)
			{
				result += s_smoothPitch[index] * smoothPoint[i];
				++index;
				if (index >= SAVITZKY_GOLAY_SMOOTH_MAXCOUNT) index = 0;
			}

			return result;
		}
		public float Yaw()
		{
			float result = 0.0f;
			int index = smoothPointCurrent;
			for (int i = 0; i < SAVITZKY_GOLAY_SMOOTH_MAXCOUNT; ++i)
			{
				result += s_smoothYaw[index] * smoothPoint[i];
				++index;
				if (index >= SAVITZKY_GOLAY_SMOOTH_MAXCOUNT) index = 0;
			}

			return result;
		}
		public float Heave()
		{
			float result = 0.0f;
			int index = smoothPointCurrent;
			for (int i = 0; i < SAVITZKY_GOLAY_SMOOTH_MAXCOUNT; ++i)
			{
				result += s_smoothHeave[index] * smoothPoint[i];
				++index;
				if (index >= SAVITZKY_GOLAY_SMOOTH_MAXCOUNT) index = 0;
			}

			return result;
		}
		public float Sway()
		{
			float result = 0.0f;
			int index = smoothPointCurrent;
			for (int i = 0; i < SAVITZKY_GOLAY_SMOOTH_MAXCOUNT; ++i)
			{
				result += s_smoothSway[index] * smoothPoint[i];
				++index;
				if (index >= SAVITZKY_GOLAY_SMOOTH_MAXCOUNT) index = 0;
			}

			return result;
		}
		public float Surge()
		{
			float result = 0.0f;
			int index = smoothPointCurrent;
			for (int i = 0; i < SAVITZKY_GOLAY_SMOOTH_MAXCOUNT; ++i)
			{
				result += s_smoothSurge[index] * smoothPoint[i];
				++index;
				if (index >= SAVITZKY_GOLAY_SMOOTH_MAXCOUNT) index = 0;
			}

			return result;
		}
	}

	private Status6DOF Gcalc6Dof = new Status6DOF();
	void Awake()
	{
		ctrl = this.GetComponent<WIZMOController>();
	}

	// Use this for initialization
	protected void Start()
	{
		if (ctrl == null)
			return;

		if (trackingObject != null)
		{
			previousPos = trackingObject.transform.position;
			previousYaw = trackingObject.transform.rotation.eulerAngles.y;
			previousVec = Vector3.zero;
		}
	}

	void LateUpdate()
	{
		if (ctrl == null)
			return;

		if (updateMethod != MoverUpdateMethod.Update)
			return;

		updateProcessing();
	}
	void FixedUpdate()
	{
		if (ctrl == null)
			return;

		if (updateMethod != MoverUpdateMethod.FixedUpdate)
			return;

		updateProcessing();
	}

	private void updateProcessing()
    {

		ctrl.roll = 0.0f;
		ctrl.pitch = 0.0f;
		ctrl.yaw = 0.0f;
		ctrl.heave = 0.0f;
		ctrl.sway = 0.0f;
		ctrl.surge = 0.0f;

		if (trackingObject != null)
		{
			float deltaTime = Time.deltaTime;
			Vector3 curPos = trackingObject.transform.position;
			curPos.x /= worldScaleMeter.x; curPos.y /= worldScaleMeter.y; curPos.z /= worldScaleMeter.z;
			Vector3 vec = (curPos - previousPos) / Time.deltaTime;
			Vector3 vecChange = (vec - previousVec);
			previousPos = curPos;
			previousVec = vec;

			//G calc
			Vector3 surge, sway, heave;
			surge.x = vecChange.x * trackingObject.transform.forward.x;
			surge.y = vecChange.y * trackingObject.transform.forward.y;
			surge.z = vecChange.z * trackingObject.transform.forward.z;
			sway.x = vecChange.x * trackingObject.transform.right.x;
			sway.y = vecChange.y * trackingObject.transform.right.y;
			sway.z = vecChange.z * trackingObject.transform.right.z;
			heave.x = vecChange.x * trackingObject.transform.up.x;
			heave.y = vecChange.y * trackingObject.transform.up.y;
			heave.z = vecChange.z * trackingObject.transform.up.z;

			//G
			float dataSurge = (surge.x + surge.y + surge.z);
			float dataSway = (sway.x + sway.y + sway.z);
			float dataHeave = (heave.x + heave.y + heave.z);

			//YAW_G calc
			float yaws = Mathf.DeltaAngle(trackingObject.transform.rotation.eulerAngles.y, previousYaw);    //+-10度 = +-1.0
			previousYaw = trackingObject.transform.rotation.eulerAngles.y;

			//Roll
			float rolls = Mathf.DeltaAngle(trackingObject.transform.rotation.eulerAngles.z, 0.0f) / 10.0f;  //+-10度
																											//Pitch
			float pitchs = Mathf.DeltaAngle(trackingObject.transform.rotation.eulerAngles.x, 0.0f) / 10.0f; //+-10度

			Gcalc6Dof.MaxStatusUpdate(rolls, pitchs, yaws, dataHeave, dataSway, dataSurge);
			ctrl.roll = ToRoundDown(rolls, 2);
			ctrl.pitch = ToRoundDown(pitchs, 2);
			ctrl.yaw = ToRoundDown(Gcalc6Dof.Yaw(), 2);
			ctrl.heave = ToRoundDown(Gcalc6Dof.Heave(), 2);
			ctrl.sway = ToRoundDown(Gcalc6Dof.Sway(), 2);
			ctrl.surge = ToRoundDown(Gcalc6Dof.Surge(), 2);
		}
		else
		{
			previousPos = Vector3.zero;
			previousYaw = 0.0f;
			previousVec = Vector3.zero;
		}

		//Force power
		ctrl.roll += rollForce;
		ctrl.pitch += pitchForce;
		ctrl.yaw += yawForce;
		ctrl.heave += heaveForce;
		ctrl.sway += swayForce;
		ctrl.surge += surgeForce;
	}

	private static float ToRoundDown(float dValue, int iDigits)
	{
		float dCoef = Mathf.Pow(10, iDigits);

		return dValue > 0 ? Mathf.Floor(dValue * dCoef) / dCoef :
							Mathf.Ceil(dValue * dCoef) / dCoef;
	}

}
