using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorTrigger : MonoBehaviour
{
	[Header("References")]
	public RailGenerator myGenerator;

	void OnTriggerEnter(Collider other)
	{
		GeneratorTrigger otherGeneratorTrigger = other.GetComponent<GeneratorTrigger>();

		if (otherGeneratorTrigger != null)
		{
			Debug.LogFormat("hi i'm {0} and my trigger has been entered by {1}", myGenerator.name, otherGeneratorTrigger.myGenerator.name);
			otherGeneratorTrigger.myGenerator.RegisterOverlappingGenerator(myGenerator);
		}
	}
}
