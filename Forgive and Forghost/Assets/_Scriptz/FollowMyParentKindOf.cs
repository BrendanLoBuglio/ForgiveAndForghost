using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMyParentKindOf : MonoBehaviour
{
	[Header("References")]
	public Transform positionParent;
	protected Quaternion _originalRotation;

	void Awake()
	{
		transform.SetParent(null);
		_originalRotation = transform.rotation;
	}

	void Update()
	{
		transform.position = positionParent.position;
		transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Lerp(_originalRotation, positionParent.rotation, 0.15f), Time.deltaTime * 10f);
	}
}
