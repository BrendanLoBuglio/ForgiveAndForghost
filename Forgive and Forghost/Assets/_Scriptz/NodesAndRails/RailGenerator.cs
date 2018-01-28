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
	public float nodeDensityInMillionths;
    public bool thisOne;
	[SerializeField] private Color nodeColor = Color.white;

	protected List<Vector3> _testPoints = new List<Vector3>();
	protected List<Node> _currentNodes = new List<Node>();
	protected List<Rail> _currentRails = new List<Rail>();

	void Start()
	{
		float density = nodeDensityInMillionths / 1000000f;
		int numNodes = Mathf.RoundToInt(density * GetVolume());
		Debug.LogFormat("i calculated {0} nodex", numNodes);
		numNodesToGenerate = numNodes;
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.C))
		{
			CheckDensity();
		}

		if (Input.GetKeyDown(KeyCode.P))
		{
			GenerateNodes();

            if (thisOne) {
                bool hi = PlayerGhost.s == null;
                Debug.Log($"player ghost is null: {hi}");
                Debug.LogFormat("node count: {0}", _currentNodes.Count);
                PlayerGhost.s.SetStartRail(_currentNodes[0].GetFirstRail());
                PlayerGhost.s.Initialize();
            }
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

		Debug.LogFormat("x: {0} // y: {1} // z: {2}", xScaleGlobal, yScaleGlobal, zScaleGlobal);

		float volume = xScaleGlobal * yScaleGlobal * zScaleGlobal;

		return volume;
	}

	protected void CheckDensity()
	{
		float volume = GetVolume();
		float density = numNodesToGenerate / volume * 1000000f;
		Debug.LogFormat("my density is: {0} 1 millionth nodes/volume unity\nvolume: {1} // num nodes: {2}", density, volume, numNodesToGenerate);
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

	protected void GenerateNodes()
	{
		_testPoints.Clear();
		ClearNodesAndRails();

		for (int i = 0; i < numNodesToGenerate; i++)
		{
			bool randomPointSuccessful = false;
			Vector3 randomPos = GetRandomPointWithinBox(spawnBox, out randomPointSuccessful);

			if (randomPointSuccessful)
			{
				_testPoints.Add(randomPos);
				Node newNode = Instantiate(nodePrefab);
				newNode.transform.position = randomPos;
				_currentNodes.Add(newNode);
			}
			else
			{
				break;
			}
		}

		for (int i = 0; i < _currentNodes.Count; i++)
		{
			_currentNodes[i].FindClosestNodes();
			this._currentNodes[i].setColor(this.nodeColor);
			Debug.Log($"Set node at {i} to color {this.nodeColor}");
		}

		Debug.Log("generated nodes!");
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
