using UnityEngine;
using System.Collections;

public class AnotherCameraFacingBillboard : MonoBehaviour
{
	protected Camera m_Camera;

	void Start()
	{
		if (Camera.main != null)
		{
			m_Camera = Camera.main;
		}
	}

	void Update()
	{
		if (m_Camera != null)
		{
			transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.forward,
				m_Camera.transform.rotation * Vector3.up);
		}
	}
}