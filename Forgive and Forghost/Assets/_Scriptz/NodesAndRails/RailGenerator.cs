using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	public int GetNumNodesToGenerate()
	{
		if (!_numNodesCalculated)
		{
			float density = nodeDensityInMillionths / 1000000f;
			int numNodes = Mathf.RoundToInt(density * GetVolume());
			Debug.LogFormat("i calculated {0} nodex", numNodes);
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
			_currentNodes[i].FindClosestNodes();
		}

		//Debug.Log("generated rails!");
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
