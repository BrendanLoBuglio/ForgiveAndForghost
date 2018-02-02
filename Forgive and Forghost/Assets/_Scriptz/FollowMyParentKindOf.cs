using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMyParentKindOf : MonoBehaviour
{
	[Header("References")]
	public Transform positionParent;

	void Awake()
	{
		transform.SetParent(null);
	}

	void Update()
	{
		transform.position = positionParent.position;
	}
}
