using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCurve_One : MonoBehaviour
{
	[SerializeField] AnimationCurve curve;
	public float TimeMax { get { return curve.keys[curve.keys.Length - 1].time; } }
	public float EndValue { get { return curve.keys[curve.keys.Length - 1].value; } }
	public float Evaluate(float time) { return curve.Evaluate(time); }
}
