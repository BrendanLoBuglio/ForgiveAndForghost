using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Scriptz.TheGamePartOfTheGame;

public class RailGeneratorManager : MonoBehaviour
{
	[Header("References")]
	public Node nodePrefab;
	public Rail railPrefab;

	protected List<RailGenerator> _railGenerators = new List<RailGenerator>();

	public void GenerateAllTheRails()
	{
		TurnOffTheTriggersForPerformanceReasonsMaybe();

		RailGenerator[] railGeneratorArray = Object.FindObjectsOfType<RailGenerator>();

		for (int i = 0; i < railGeneratorArray.Length; i++)
		{
			if (railGeneratorArray[i].isActiveAndEnabled)
			{
				_railGenerators.Add(railGeneratorArray[i]);
			}
		}

		RunAllRailGeneration();
	}

	protected void TurnOffTheTriggersForPerformanceReasonsMaybe()
	{
		GeneratorTrigger[] generatorTrigger = Object.FindObjectsOfType<GeneratorTrigger>();

		for (int i = 0; i < generatorTrigger.Length; i++)
		{
			generatorTrigger[i].gameObject.SetActive(false);
		}
	}

	protected void RunAllRailGeneration()
	{
		for (int i = 0; i < _railGenerators.Count; i++)
		{
			_railGenerators[i].GenerateNodes();
		}

		MakeSureAllIntersectingZonesHaveConnection();

		for (int i = 0; i < _railGenerators.Count; i++)
		{
			_railGenerators[i].GenerateRails();
		}

		GeneratePortalNodeRails();
		MakeSureAllNodesHaveAtLeastTwoRails();
	}

	protected void GeneratePortalNodeRails()
	{
		PortalNode[] portalNodeArray = Object.FindObjectsOfType<PortalNode>();

		for (int i = 0; i < portalNodeArray.Length; i++)
		{
			portalNodeArray[i].FindClosestNodes(new NodeSearchSettings(false));
		}
	}

	protected void MakeSureAllIntersectingZonesHaveConnection()
	{
		for (int i = 0; i < _railGenerators.Count; i++)
		{
			_railGenerators[i].MakeSureIAmConnectedToAllMyOverlappingGenerators();
		}
	}

	protected void MakeSureAllNodesHaveAtLeastTwoRails()
	{
		for (int i = 0; i < _railGenerators.Count; i++)
		{
			_railGenerators[i].CheckAllMyNodes();
		}

		PortalNode[] portalNodeArray = Object.FindObjectsOfType<PortalNode>();

		for (int i = 0; i < portalNodeArray.Length; i++)
		{
			portalNodeArray[i].MakeSureIHaveAtLeastTwoRails();
		}
	}

	public static Rail GetNewRail()
	{
		return Instantiate(RailGeneratorManager.s.railPrefab);
	}

	private static RailGeneratorManager _myPrivateSelf;
	public static RailGeneratorManager s
	{
		get
		{
			if (_myPrivateSelf == null) _myPrivateSelf = Object.FindObjectOfType<RailGeneratorManager>();
			return _myPrivateSelf;
		}
	}
}
