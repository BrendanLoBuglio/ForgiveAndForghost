using UnityEngine;
using System.Collections;
using _Scriptz.TheGamePartOfTheGame;

public class CameraFacingBillboard : MonoBehaviour
{
	protected Camera m_Camera;

	void Start()
	{
		if (GameplayManager.singleton.aerialCamera != null)
		{
			m_Camera = GameplayManager.singleton.aerialCamera;
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