using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostyCloth : MonoBehaviour
{
	[Header("References")]
	public Transform ghostyTopLevel;
	public Cloth ghostyCloth;

	[Header("Settings")]
	public float gravity;

	void Update()
	{
		Vector3 down = ghostyTopLevel.TransformVector (Vector3.down);
		down = down.normalized * gravity;
		ghostyCloth.externalAcceleration = down;
	}
}
