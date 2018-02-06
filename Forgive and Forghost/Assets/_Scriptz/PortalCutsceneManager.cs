using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCutsceneManager : MonoBehaviour
{
	[Header("References")]
	[SerializeField] protected List<Transform> portalOrbs = new List<Transform>();
	[SerializeField] protected Renderer portalRenderer;

	[Header("Settings")]
	[SerializeField] protected float originalPortalOrbLocalY;
	[SerializeField] protected float animatePortalOrbsTo;
	[SerializeField] protected float animatePortalOrbsTime;
	[SerializeField] protected Ease animatePortalOrbsEase;

	[SerializeField] protected float portalFadeTime;

	protected bool _doingCutscene;

	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.C))
		{
			StartCutscene();
		}
	}

	protected void StartCutscene()
	{
		if (!_doingCutscene)
		{
			_doingCutscene = true;

			for (int i = 0; i < portalOrbs.Count; i++)
			{
				Transform orb = portalOrbs[i];
				orb.DOLocalMoveY(animatePortalOrbsTo, animatePortalOrbsTime).SetEase(animatePortalOrbsEase);
			}

			DOTween.Sequence().AppendInterval(animatePortalOrbsTime).AppendCallback(FadeInPortal);
		}
	}

	protected void FadeInPortal()
	{
		portalRenderer.gameObject.SetActive(true);
		portalRenderer.material.DOFade(0, portalFadeTime).From();
	}
}
