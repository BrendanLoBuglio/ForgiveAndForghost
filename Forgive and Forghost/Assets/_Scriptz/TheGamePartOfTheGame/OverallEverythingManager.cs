using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverallEverythingManager : MonoBehaviour
{
	[Header("Build Settings")]
	public int mainMenuSceneIndex;
	public int victorySceneIndex;

	[Header("Other Settings")]
	public float holdEscapeForHowLong;

	protected float _holdEscapeCounter;
	protected bool _countingEscape;

	void Awake()
	{
		if (_myPrivateSelf != null)
		{
			if (_myPrivateSelf != this)
			{
				Destroy(gameObject);
			}
		}
		else
		{
			_myPrivateSelf = this;
			DontDestroyOnLoad(gameObject);
		}
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			_countingEscape = true;
			_holdEscapeCounter = 0;
		}

		if (_countingEscape)
		{
			if (Input.GetKey(KeyCode.Escape))
			{
				_holdEscapeCounter += Time.unscaledDeltaTime;

				if (_holdEscapeCounter >= holdEscapeForHowLong)
				{
					_countingEscape = false;

					if (SceneManager.GetActiveScene().buildIndex == mainMenuSceneIndex)
					{
						Application.Quit();
					}
					else
					{
						SceneManager.LoadScene(mainMenuSceneIndex);
					}
				}
			}
		}
	}

	private static OverallEverythingManager _myPrivateSelf;
	public static OverallEverythingManager s
	{
		get
		{
			if (_myPrivateSelf == null)
			{
				_myPrivateSelf = Object.FindObjectOfType<OverallEverythingManager>();
				DontDestroyOnLoad(_myPrivateSelf.gameObject);
			}

			return _myPrivateSelf;
		}
	}
}
