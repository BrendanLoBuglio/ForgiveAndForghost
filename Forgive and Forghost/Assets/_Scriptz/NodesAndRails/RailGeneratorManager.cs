using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.P))
		{
			RunAllRailGeneration();
			PutPlayerOnRail();
		}
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
	}

	protected void PutPlayerOnRail()
	{
		PlayerGhost.s.SetStartRail(_railGenerators[0].GetFirstNode().GetFirstRail());
		PlayerGhost.s.Initialize();
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
