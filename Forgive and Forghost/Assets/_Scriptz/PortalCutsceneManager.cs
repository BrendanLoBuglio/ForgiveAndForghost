using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Scriptz.TheGamePartOfTheGame;

public class PortalCutsceneManager : MonoBehaviour
{
	[Header("References")]
	[SerializeField] protected List<Transform> portalOrbs = new List<Transform>();
	[SerializeField] protected Renderer portalRenderer;
	[SerializeField] protected Transform letterHoverSpot;
	[SerializeField] protected GameObject hellLetterPrefab;
	[SerializeField] protected GameObject wotlLetterPrefab;
	[SerializeField] protected Material hellPortalMaterial;
	[SerializeField] protected Material wotlPortalMaterial;
	[SerializeField] protected Transform letterDeliveryPoint;

	[Header("Open Portal Settings")]
	[SerializeField] protected float originalPortalOrbLocalY;
	[SerializeField] protected float animatePortalOrbsTo;
	[SerializeField] protected float animatePortalOrbsTime;
	[SerializeField] protected Ease animatePortalOrbsEase;
	[SerializeField] protected float portalFadeTime;

	[Header("Deliver Letter Settings")]
	[SerializeField] protected float moveLetterToHoverSpotTime;
	[SerializeField] protected Ease moveLetterToHoverSpotEase;
	[SerializeField] protected float hoverAndWaitTime;
	[SerializeField] protected float deliverLetterIntoPortalTime;
	[SerializeField] protected Ease deliverLetterIntoPortalEase;

	[Header("Receive Letter Settings")]
	[SerializeField] protected float waitTimeBeforeReceiveLetter;
	[SerializeField] protected float moveLetterToGhostyTime;
	[SerializeField] protected Ease moveLetterToGhostyEase;

	protected bool _doingCutscene;
	protected GameObject _deliveredLetter;
	protected GameObject _receivedLetter;
	protected string _decodedMessage;
	protected bool _isFinalMessage;

	public Action OnCustceneComplete = delegate {};

	void Update ()
	{
		/*if (Input.GetKeyDown(KeyCode.C))
		{
			StartCutscene("this is a test cutscene ;)");
		}*/
	}

	public void StartCutscene(string decodedMessage, bool isFinalMessage)
	{
		if (!_doingCutscene)
		{
			_doingCutscene = true;
			_decodedMessage = decodedMessage;
			_isFinalMessage = isFinalMessage;
			OpenPortal();
		}
	}

	protected void OpenPortal()
	{
		for (int i = 0; i < portalOrbs.Count; i++)
		{
			Transform orb = portalOrbs[i];
			orb.DOLocalMoveY(animatePortalOrbsTo, animatePortalOrbsTime).SetEase(animatePortalOrbsEase);
		}

		DOTween.Sequence().AppendInterval(animatePortalOrbsTime).AppendCallback(FadeInPortal);
	}

	protected void FadeInPortal()
	{
		Sequence s = DOTween.Sequence();

		if (GameplayManager.singleton.currentMissionHalf == UniverseType_E.WOTL)
		{
			portalRenderer.sharedMaterial = wotlPortalMaterial;
		}
		else
		{
			portalRenderer.sharedMaterial = hellPortalMaterial;
		}

		s.Append(portalRenderer.sharedMaterial.DOFade(0, 0));
		s.Append(portalRenderer.sharedMaterial.DOFade(1, portalFadeTime).OnComplete(DeliverLetter));
		portalRenderer.gameObject.SetActive(true);
	}

	protected void DeliverLetter()
	{
		if (GameplayManager.singleton.currentMissionHalf == UniverseType_E.WOTL) // then it's actually hell
		{
			_deliveredLetter = Instantiate(hellLetterPrefab);
		}
		else
		{
			_deliveredLetter = Instantiate(wotlLetterPrefab);
		}

		_deliveredLetter.transform.position = PlayerGhost.s.letterDeliveryAndReceiptPoint.position;
		_deliveredLetter.transform.DOMove(letterHoverSpot.position, moveLetterToHoverSpotTime).SetEase(moveLetterToHoverSpotEase).OnComplete(ShowLetterUI);
	}

	protected void ShowLetterUI()
	{
		UIManager.singleton.ShowLetterContents(_decodedMessage, GameplayManager.singleton.currentMissionHalf, (hoverAndWaitTime) * 0.85f);
		_deliveredLetter.transform.DOMove(letterDeliveryPoint.position, deliverLetterIntoPortalTime).SetEase(deliverLetterIntoPortalEase).SetDelay(hoverAndWaitTime).OnComplete(DestroyDeliveredLetter);
	}

	protected void DestroyDeliveredLetter()
	{
		Destroy(_deliveredLetter);
		DOTween.Sequence().AppendInterval(waitTimeBeforeReceiveLetter).AppendCallback(ReceiveLetter);
	}

	protected void ReceiveLetter()
	{
		if (_isFinalMessage)
		{
			ClosePortal();
		}
		else
		{
			if (GameplayManager.singleton.currentMissionHalf == UniverseType_E.WOTL)
			{
				_receivedLetter = Instantiate(wotlLetterPrefab);
			}
			else
			{
				_receivedLetter = Instantiate(hellLetterPrefab);
			}

			_receivedLetter.transform.position = letterDeliveryPoint.position;
			_receivedLetter.transform.DOMove(PlayerGhost.s.letterDeliveryAndReceiptPoint.position, moveLetterToGhostyTime).SetEase(moveLetterToGhostyEase).OnComplete(ClosePortal);
		}
	}

	protected void ClosePortal()
	{
		if (_receivedLetter != null)
		{
			Destroy(_receivedLetter);
		}

		for (int i = 0; i < portalOrbs.Count; i++)
		{
			Transform orb = portalOrbs[i];
			orb.DOLocalMoveY(originalPortalOrbLocalY, animatePortalOrbsTime).SetEase(animatePortalOrbsEase);
		}

		portalRenderer.sharedMaterial.DOFade(0, portalFadeTime).OnComplete(TurnOffPortal);
	}

	protected void TurnOffPortal()
	{
		portalRenderer.gameObject.SetActive(true);
		_doingCutscene = false;
		OnCustceneComplete.Invoke();
	}
}
