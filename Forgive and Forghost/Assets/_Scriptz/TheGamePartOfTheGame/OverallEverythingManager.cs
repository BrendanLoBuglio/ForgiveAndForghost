using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OverallEverythingManager : MonoBehaviour
{
	[Header("Build Settings")]
	public int mainMenuSceneIndex;
	public int victorySceneIndex;

	[Header("Other Settings")]
	public float holdEscapeForHowLong;

	[Header("Escape Screen References")]
	public GameObject escapeScreenObject;
	public Image escapeScreenCircleFill;

	protected float _holdEscapeCounter;
	protected bool _countingEscape;
	protected bool _allowedToHoldEscape = true;

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

		escapeScreenObject.SetActive(false);
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			Time.timeScale = (Time.timeScale == 1 ? 2 : 1);
		}

		if (_allowedToHoldEscape)
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				_countingEscape = true;
				_holdEscapeCounter = 0;
				escapeScreenCircleFill.fillAmount = 0f;
				escapeScreenObject.SetActive(true);
			}
			else if (Input.GetKeyUp(KeyCode.Escape))
			{
				escapeScreenObject.SetActive(false);
			}

			if (_countingEscape)
			{
				if (Input.GetKey(KeyCode.Escape))
				{
					_holdEscapeCounter += Time.unscaledDeltaTime;

					escapeScreenCircleFill.fillAmount = Mathf.Clamp01(_holdEscapeCounter / holdEscapeForHowLong);

					if (_holdEscapeCounter >= holdEscapeForHowLong)
					{
						_countingEscape = false;
						_allowedToHoldEscape = false;
						escapeScreenObject.SetActive(false);
						Invoke("AllowMeToHoldEscapeAgain", 0.2f);

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
	}

	protected void AllowMeToHoldEscapeAgain()
	{
		_allowedToHoldEscape = true;
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
