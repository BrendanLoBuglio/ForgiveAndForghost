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
	public float minXZMagnitude = 30;
	public float maxXZMagnitude = 80;

	protected float _currentXZMagnitude;

	void Start()
	{
		_currentXZMagnitude = minXZMagnitude;
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
		Vector3 vectorToTransform = Vector3.zero;

		if (_currentXZMagnitude > 0)
		{
			float x = Random.Range(-1f, 1f);
			float z = Random.Range(-1f, 1f);
			Vector3 xz = new Vector3(x, 0, z) * _currentXZMagnitude;
			vectorToTransform = xz;
		}

		vectorToTransform = vectorToTransform + (Vector3.down * gravity);

		Vector3 down = ghostyTopLevel.TransformVector (vectorToTransform);
		ghostyCloth.externalAcceleration = down;
	}

	public void UpdateXZValue(float lerpValue)
	{
		_currentXZMagnitude = Mathf.Lerp(minXZMagnitude, maxXZMagnitude, lerpValue);
	}
}
