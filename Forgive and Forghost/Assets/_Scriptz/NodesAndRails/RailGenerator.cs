using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodePair
{
	public Node nodeA;
	public Node nodeB;
	public float distance;
}

public class RailGenerator : MonoBehaviour
{
	[Header("References")]
	public BoxCollider spawnBox;

	[Header("Settings")]
	public int numNodesToGenerate;
	public float nodeDensityInMillionths;
	[SerializeField] private Color nodeColor = Color.white;

	protected List<Vector3> _testPoints = new List<Vector3>();
	protected List<Node> _currentNodes = new List<Node>();
	protected List<Rail> _currentRails = new List<Rail>();
	protected bool _numNodesCalculated = false;

	void Start()
	{
		if (this.spawnBox.GetComponent<Renderer>() != null) {
			this.spawnBox.GetComponent<Renderer>().enabled = false;
		}
	}

	public void DoALittleTest(List<RailGenerator> railGeneratorList)
	{
		/*for (int i = 0; i < railGeneratorList.Count; i++)
		{
			if (railGeneratorList[i] != this)
			{
				RailGenerator otherGenerator = railGeneratorList[i];
				Physics.
			}
		}*/
	}

	public int GetNumNodesToGenerate()
	{
		if (!_numNodesCalculated)
		{
			float density = nodeDensityInMillionths / 1000000f;
			int numNodes = Mathf.RoundToInt(density * GetVolume());
			numNodesToGenerate = numNodes;
			_numNodesCalculated = true;
		}

		return numNodesToGenerate;
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.C))
		{
			CheckDensity();
		}
	}

	protected float GetVolume()
	{
		Vector3 localScale = spawnBox.transform.localScale;
		Vector3 parentLocalScale = spawnBox.transform.parent.localScale;
		Vector3 center = spawnBox.transform.position;

		float xScaleGlobal = localScale.x * parentLocalScale.x;
		float yScaleGlobal = localScale.y * parentLocalScale.y;
		float zScaleGlobal = localScale.z * parentLocalScale.z;

		//Debug.LogFormat("x: {0} // y: {1} // z: {2}", xScaleGlobal, yScaleGlobal, zScaleGlobal);

		float volume = xScaleGlobal * yScaleGlobal * zScaleGlobal;

		return volume;
	}

	protected void CheckDensity()
	{
		float volume = GetVolume();
		float density = numNodesToGenerate / volume * 1000000f;
		//Debug.LogFormat("my density is: {0} 1 millionth nodes/volume unity\nvolume: {1} // num nodes: {2}", density, volume, numNodesToGenerate);
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

	protected int _uhhHowManyTriesShouldIDo = 60;

	public void GenerateNodes()
	{
		_testPoints.Clear();
		ClearNodesAndRails();

		for (int i = 0; i < GetNumNodesToGenerate(); i++)
		{
			bool randomPointSuccessful = false;
			Vector3 randomPos = GetRandomPointWithinBox(spawnBox, out randomPointSuccessful);

			if (randomPointSuccessful)
			{
				_testPoints.Add(randomPos);
				Node newNode = Instantiate(RailGeneratorManager.s.nodePrefab);
				newNode.transform.position = randomPos;
				_currentNodes.Add(newNode);
				newNode.initialize(this.nodeColor);
			}
			else
			{
				break;
			}
		}

		//Debug.Log("generated nodes!");
	}

	public void GenerateRails()
	{
		for (int i = 0; i < _currentNodes.Count; i++)
		{
			_currentNodes[i].FindClosestNodes(new NodeSearchSettings(false));
		}

		//MakeSureAllNodesAreConnected(_currentNodes);

		//Debug.Log("generated rails!");
	}

	// utility function
	protected void MakeSureAllNodesAreConnected(List<Node> nodes)
	{
		List<Node> remainingNodesToSearch = new List<Node>(nodes);
		List<List<Node>> distinctSubGraphs = new List<List<Node>>();

		while (remainingNodesToSearch.Count > 0)
		{
			List<Node> connectedNodes = new List<Node>();
			dfs_whatDoesThatStandFor(remainingNodesToSearch[0], remainingNodesToSearch, connectedNodes);
			distinctSubGraphs.Add(connectedNodes);
		}

		Debug.LogFormat("found {0} distinct subgraphs", distinctSubGraphs.Count);

		while (distinctSubGraphs.Count > 1)
		{
			List<Node> graphA = distinctSubGraphs[0];
			List<Node> graphB = distinctSubGraphs[1];
			distinctSubGraphs.RemoveAt(0);
			distinctSubGraphs.RemoveAt(1);
			distinctSubGraphs.Insert(0, ConnectTwoGraphs(graphA, graphB));
			Debug.Log("connected 2 distinct subgraphs into 1 graph");
		}

		Debug.LogFormat("all subgraphs have been merged, we are now 1 connected graph god bless");
	}

	// a List<Node> is a graph?? let's go for it
	protected List<Node> ConnectTwoGraphs(List<Node> graphA, List<Node> graphB)
	{
		List<Node> connectedGraphs = new List<Node>();
		// IMPLEMENT
		return connectedGraphs;
	}

	protected NodePair GetTwoClosestNodesFromTwoGraphs(List<Node> graphA, List<Node> graphB)
	{
		NodePair closestNodePair = null;
		// IMPLEMENT
		return closestNodePair;
	}

	protected void dfs_whatDoesThatStandFor(Node node, List<Node> listOfRemainingNodesToSearch, List<Node> listOfConnectedNodes)
	{
		if (listOfRemainingNodesToSearch.Contains(node))
		{
			listOfRemainingNodesToSearch.Remove(node);
		}
		else
		{
			node.name = string.Format("Error Node {0}", UnityEngine.Random.Range(0, 999));
			Debug.LogErrorFormat("i couldn't remove this fromt he list something is wrong: {0}", node.name);
		}

		if (!listOfConnectedNodes.Contains(node))
		{
			listOfConnectedNodes.Add(node);
		}
		else
		{
			node.name = string.Format("Error Node {0}", UnityEngine.Random.Range(0, 999));
			Debug.LogErrorFormat("i wanted to add to list but it was alreay there, something is wrong: {0}", node.name);
		}

		Node[] connectedNodeArray = node.GetConnectedNodes();

		for (int i = 0; i < connectedNodeArray.Length; i++)
		{
			if (!listOfConnectedNodes.Contains(connectedNodeArray[i]))
			{
				dfs_whatDoesThatStandFor(connectedNodeArray[i], listOfRemainingNodesToSearch, listOfConnectedNodes);
			}
		}
	}

	public void CheckAllMyNodes()
	{
		for (int i = 0; i < _currentNodes.Count; i++)
		{
			_currentNodes[i].MakeSureIHaveAtLeastTwoRails();
		}
	}

	protected int _minNodeDistance = 40;

	protected Vector3 GetRandomPointWithinBox(BoxCollider box, out bool successfullyPulledRandomPoint)
	{
		int triesSoFar = 0;
		float distance = -1;
		Vector3 randomPoint = Vector3.zero;
		successfullyPulledRandomPoint = true;

		while (distance < _minNodeDistance)
		{
			randomPoint = TryRandomPointWithinBox(box);

			if (_currentNodes.Count > 0)
			{
				for (int i = 0; i < _currentNodes.Count; i++)
				{
					distance = Vector3.Distance(randomPoint, _currentNodes[i].transform.position);

					if (distance > _minNodeDistance)
					{
						// we're good! next
						continue;
					}
					else
					{
						// not good, pull another one
						break;
					}
				}

				triesSoFar++;

				if (triesSoFar >= _uhhHowManyTriesShouldIDo)
				{
					successfullyPulledRandomPoint = false;
					randomPoint = Vector3.zero;
					break;
				}
			}
			else
			{
				break;
			}
		}

		return randomPoint;
	}

	protected Vector3 TryRandomPointWithinBox(BoxCollider box)
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

	public Node GetFirstNode()
	{
		return _currentNodes[0];
	}
}
