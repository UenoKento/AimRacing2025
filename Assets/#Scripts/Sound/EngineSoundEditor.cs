using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineSoundEditor : MonoBehaviour
{
	FMOD.Studio.EventInstance Engine;
	[SerializeField]AnimationCurve curve = AnimationCurve.Linear(0,0,10,8000);

	void Start()
	{
		Engine = FMODUnity.RuntimeManager.CreateInstance("event:/Engine");
	}

	public IEnumerator RevUpToEngine()
	{
		float time = 0;
		Engine.start();
		while(true)
		{
			while (time < curve.keys[curve.keys.Length - 1].time)
			{
				Engine.setParameterByName("RPM", curve.Evaluate(time));
				time += Time.deltaTime;
				yield return null;
			}
			time = 0;
		}
	}

	public void SetVolume(float value){ Engine.setVolume(value); }
}