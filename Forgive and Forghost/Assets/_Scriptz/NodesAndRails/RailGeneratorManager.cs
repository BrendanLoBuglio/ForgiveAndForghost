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

	void Start()
	{
		RailGenerator[] railGeneratorArray = Object.FindObjectsOfType<RailGenerator>();

		for (int i = 0; i < railGeneratorArray.Length; i++)
		{
			if (railGeneratorArray[i].isActiveAndEnabled)
			{
				_railGenerators.Add(railGeneratorArray[i]);
			}
		}
		
		RunAllRailGeneration();
		this.StartGame();
	}

	protected void RunAllRailGeneration()
	{
		for (int i = 0; i < _railGenerators.Count; i++)
		{
			_railGenerators[i].GenerateNodes();
		}

		for (int i = 0; i < _railGenerators.Count; i++)
		{
			_railGenerators[i].GenerateRails();
		}

		GeneratePortalNodeRails();
		MakeSureAllPortalNodesHaveAtLeastTwoRails();
		MakeSureAllIntersectingZonesHaveConnection();
		MakeSureAllNodesHaveAtLeastTwoRails();
	}

	protected void GeneratePortalNodeRails()
	{
		PortalNode[] portalNodeArray = Object.FindObjectsOfType<PortalNode>();

		for (int i = 0; i < portalNodeArray.Length; i++)
		{
			portalNodeArray[i].FindClosestNodes();
		}
	}

	protected void MakeSureAllPortalNodesHaveAtLeastTwoRails()
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

	protected void MakeSureAllIntersectingZonesHaveConnection()
	{

	}

	protected void MakeSureAllNodesHaveAtLeastTwoRails()
	{

	}

	protected void StartGame()
	{
		GameplayManager.singleton.initializeGame();
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
