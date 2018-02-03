using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpSheet : MonoBehaviour
{
	public void SetActive(bool active)
	{
		if (active)
		{
			if (OverallEverythingManager.s.WeAreInALevel())
			{
				gameObject.SetActive(true);
			}
		}
		else
		{
			gameObject.SetActive(false);
		}
	}
}
