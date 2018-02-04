using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostyCloth : MonoBehaviour
{
	[Header("References")]
	public Transform ghostyTopLevel;
	public Cloth ghostyCloth;
	public List<CapsuleCollider> delayedCapsuleColliders;

	[Header("Settings")]
	public float gravity;

	void Start()
	{
		Invoke("TurnOnDelayedColliders", 1f);
	}

	void TurnOnDelayedColliders()
	{
		if (delayedCapsuleColliders != null && delayedCapsuleColliders.Count > 0)
		{
			List<CapsuleCollider> totalColliders = new List<CapsuleCollider>(ghostyCloth.capsuleColliders);
			totalColliders.AddRange(delayedCapsuleColliders);
			ghostyCloth.capsuleColliders = totalColliders.ToArray();
		}
	}

	void Update()
	{
		Vector3 down = ghostyTopLevel.TransformVector (Vector3.down);
		down = down.normalized * gravity;
		ghostyCloth.externalAcceleration = down;
	}
}
