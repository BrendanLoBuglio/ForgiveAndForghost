using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class WelcomeToHell : MonoBehaviour
{
	/*# Config #*/
	[SerializeField] private AudioClip _likeIveBeenThereBefore_c;

	/*# Cache #*/
	private AudioSource _beenThereBeforeSource;
	
	private void Awake()
	{
		this._beenThereBeforeSource = this.GetComponent<AudioSource>();
		this._beenThereBeforeSource.clip = this._likeIveBeenThereBefore_c;
		this._beenThereBeforeSource.Play();
	}
}
