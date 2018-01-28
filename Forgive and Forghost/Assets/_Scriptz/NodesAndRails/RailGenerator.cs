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
		Vector3 pos = Vector3.zero;

		Vector3 localScale = spawnBox.transform.localScale;
		Vector3 parentLocalScale = spawnBox.transform.parent.localScale;
		Vector3 center = spawnBox.transform.position;

		float xScaleGlobal = localScale.x * parentLocalScale.x;
		float yScaleGlobal = localScale.y * parentLocalScale.y;
		float zScaleGlobal = localScale.z * parentLocalScale.z;

		Vector3 startPoint = center + (spawnBox.transform.TransformVector(Vector3.forward).normalized * zScaleGlobal / 2);
		Vector3 endPoint = center - (spawnBox.transform.TransformVector(Vector3.forward).normalized * zScaleGlobal / 2);
		Vector3 randomPointOnLine = Vector3.Lerp(startPoint, endPoint, Random.Range(0f, 1f));

		float randomX = Random.Range(-xScaleGlobal / 2, xScaleGlobal / 2);
		float randomY = Random.Range(-yScaleGlobal / 2, yScaleGlobal / 2);
		Vector3 randomOffset = new Vector3(randomX, randomY, 0);
		randomOffset = spawnBox.transform.TransformDirection(randomOffset);

		pos = randomPointOnLine + randomOffset;

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
