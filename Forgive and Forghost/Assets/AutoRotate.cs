using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotate : MonoBehaviour
{
	public Vector3 eulerAnglesPerSecond;
	public bool useSin;
	
	void Update ()
	{
		//transform.localEulerAngles += (this.eulerAnglesPerSecond * Time.deltaTime);
		if (this.useSin) {

			transform.localRotation =
				Quaternion.AngleAxis(this.eulerAnglesPerSecond.magnitude * Time.deltaTime * Mathf.Sin((Time.time * 4f)),
					(this.eulerAnglesPerSecond.normalized)) * this.transform.localRotation;
		}
		else {
			transform.localRotation =
				Quaternion.AngleAxis(this.eulerAnglesPerSecond.magnitude * Time.deltaTime,
					(this.eulerAnglesPerSecond.normalized)) * this.transform.localRotation;
		}
	}
}
