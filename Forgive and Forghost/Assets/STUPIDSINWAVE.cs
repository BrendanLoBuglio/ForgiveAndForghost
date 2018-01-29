using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STUPIDSINWAVE : MonoBehaviour {
	public float frequency;
	public float amplitude;

	Vector3 originalPos;
	// Use this for initialization
	void Start () {
		originalPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = originalPos + new Vector3(0, Mathf.Sin(Time.time * frequency) * amplitude, 0);
	}
}
