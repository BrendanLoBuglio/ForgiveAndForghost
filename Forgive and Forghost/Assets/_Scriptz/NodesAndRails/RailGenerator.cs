using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailGenerator : MonoBehaviour
{
	[Header("References")]
	public Node nodePrefab;
	public Rail railPrefab;
	public BoxCollider spawnBox;

	[Header("Settings")]
	public int numNodesToGenerate;

	protected List<Vector3> _testPoints = new List<Vector3>();
	protected List<Node> _currentNodes = new List<Node>();
	protected List<Rail> _currentRails = new List<Rail>();

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			GenerateNodes();
		}
	}

	void OnDrawGizmos()
	{
		/*for (int i = 0; i < _testPoints.Count; i++)
		{
			Gizmos.DrawSphere(_testPoints[i], 1);
		}*/
	}

	protected void ClearNodesAndRails()
	{
		for (int i = 0; i < _currentNodes.Count; i++)
		{
			//Destroy(_currentNodes[i].gameObject);
			_currentNodes[i].DestroyMeAndMyRails();
		}

		for (int i = 0; i < _currentRails.Count; i++)
		{
			Destroy(_currentRails[i].gameObject);
		}

		_currentNodes.Clear();
		_currentRails.Clear();
	}

	protected void GenerateNodes()
	{
		_testPoints.Clear();
		ClearNodesAndRails();

		for (int i = 0; i < numNodesToGenerate; i++)
		{
			Vector3 randomPos = GetRandomPointWithinBox(spawnBox);
			_testPoints.Add(randomPos);
			Node newNode = Instantiate(nodePrefab);
			newNode.transform.position = randomPos;
			_currentNodes.Add(newNode);
		}

		for (int i = 0; i < _currentNodes.Count; i++)
		{
			_currentNodes[i].FindClosestNodes();
		}

		Debug.Log("generated nodes!");
	}

	protected Vector3 GetRandomPointWithinBox(BoxCollider box)
	{
		Bounds bounds = box.bounds;
		Vector3 pos = Vector3.zero;
		pos.x = Random.Range(bounds.min.x, bounds.max.x);
		pos.y = Random.Range(bounds.min.y, bounds.max.y);
		pos.z = Random.Range(bounds.min.z, bounds.max.z);
		return pos;
	}

	public static Rail GetNewRail()
	{
		return Instantiate(RailGenerator.s.railPrefab);
	}

	private static RailGenerator _myPrivateSelf;
	public static RailGenerator s
	{
		get
		{
			if (_myPrivateSelf == null) _myPrivateSelf = Object.FindObjectOfType<RailGenerator>();
			return _myPrivateSelf;
		}
	}
}
