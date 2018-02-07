using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STUPIDSINWAVE : MonoBehaviour {
	[Header("Constant Settings")]
	public float frequency;
	public float amplitude;

	[Header("LerpySettings")]
	public float minFrequency;
	public float maxFrequency;
	public float minAmplitude;
	public float maxAmplitude;

	Vector3 _originalLocalPos;

	void Start ()
	{
		_originalLocalPos = transform.localPosition;
	}
	
	void Update ()
	{
		transform.localPosition = _originalLocalPos + new Vector3(0, Mathf.Sin(Time.time * frequency) * amplitude, 0);
	}

	public void UpdateSinWave(float lerpValue)
	{
		frequency = Mathf.Lerp(minFrequency, maxFrequency, lerpValue);
		amplitude = Mathf.Lerp(minAmplitude, maxAmplitude, lerpValue);

		// TODO DO THIS IN PREFAB, ALSO GET RID OF PRE-SET VALUES FOR XZ MAGNITUDE IN GHOSTY CLOTH CUZ IT AFFECTED THE TITLE SCREEN
	}
}
