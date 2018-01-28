using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostyCloth : MonoBehaviour
{
	[Header("References")]
	public Cloth ghostyCloth;

	[Header("Settings")]
	public float gravity;

	void Update()
	{
		Vector3 down = transform.TransformDirection (-transform.up);
		down = down.normalized * gravity;
		ghostyCloth.externalAcceleration = down;
	}
}
