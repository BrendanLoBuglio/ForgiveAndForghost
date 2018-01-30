using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Node : MonoBehaviour
{
    private class NodeAndDistance
    {
        public Node node;
        public float distance;

        public NodeAndDistance(Node n, float d)
        {
            node = n;
            distance = d;
        }
    }

	public List<Rail> rails;

	/*# Local Reference #*/
	private Renderer _renderer;

	/*# Injected config #*/
	public Color regionColor { get; private set; }
	
	private void Awake()
	{
		this._renderer = this.GetComponentInChildren<Renderer>();
	}

	public Rail GetFirstRail()
    {
        return rails[0];
    }

	/// Given a node that the player is coming from, return a list of all the nodes that I might connect them to
	public Node[] getPossibleNodesForJunction(Node fromNode)
	{
		return this.rails.Select(rail => rail.originNode != this ? rail.originNode : rail.endNode)
			.Where(node => node != fromNode).ToArray();
	}

	/// Given a node that I connect to, return the rail that connects us. If I'm not connected to that node, you got
	///  an error on your hands. 
	public Rail getRailByDestinationNode(Node destination)
	{
		var output = this.rails.Where(rail => rail.originNode == destination || rail.endNode == destination).ToList();
		if (output.Count == 0)
			throw new Exception($"Error! Tried to get the rail that connects {this.name} to node {destination}, but no such rail exists.");
		return output[0];
	}

	protected int _maxRails = 4;
	protected int _maxNodeDistance = 300;

	protected bool IsNodeOkayToConnectTo(Node n)
	{
		if (!n.HasFullRails())
		{
			if (n != this)
			{
				if (!IsConnectedTo(n))
				{
					return true;
				}
			}
		}

		return false;
	}

	public void FindClosestNodes()
	{
		Node[] nodeArray = FindObjectsOfType<Node>();

		List<NodeAndDistance> closestNodes = new List<NodeAndDistance>();
		int numRailsWeNeed = _maxRails - rails.Count;

		for (int i = 0; i < nodeArray.Length; i++)
		{
			if (IsNodeOkayToConnectTo(nodeArray[i]))
			{
				float distance = Vector3.Distance(transform.position, nodeArray[i].transform.position);

				if (distance < _maxNodeDistance)
				{
					if (closestNodes.Count < numRailsWeNeed)
					{
						closestNodes.Add(new NodeAndDistance(nodeArray[i], distance));
						continue;
					}
					else
					{
						for (int j = 0; j < closestNodes.Count; j++)
						{
							if (distance < closestNodes[j].distance)
							{
								closestNodes[j] = new NodeAndDistance(nodeArray[i], distance);
								break;
							}
						}

						continue;
					}
				}
			}
		}

		for (int i = 0; i < closestNodes.Count; i++)
		{
			Rail newRail = RailGeneratorManager.GetNewRail();
			newRail.initialize(this, closestNodes[i].node);
		}
	}

	public void MakeSureIHaveAtLeastTwoRails()
	{
		if (rails.Count < 2)
		{
			name = ("Broken Node " + UnityEngine.Random.Range(0, 999));
			Debug.LogErrorFormat("node {0} only has {1} rails :(\ngonna make it so it has at least 2 rails", name, rails.Count);
		}
	}


	public void initialize(Color regionColorIn)
	{
		float h, s, v;
		Color.RGBToHSV(regionColorIn, out h, out s, out v);
		var satOffset = 1000f;
		var perlinScale = 20f;
		
		var perlinHueModifier = Mathf.Lerp(0.9f, 1.1f, Mathf.PerlinNoise(this.transform.position.x / perlinScale, this.transform.position.y / perlinScale));
		var perlinSatModifier = Mathf.Lerp(0.9f, 1.1f, Mathf.PerlinNoise(satOffset + this.transform.position.x / perlinScale, satOffset + this.transform.position.y / perlinScale));
		regionColorIn = Color.HSVToRGB(h * perlinHueModifier, s * perlinSatModifier, v);
		
		this.regionColor = regionColorIn;
		var albedoAlpha = this._renderer.material.GetColor("_Color").a;
		this._renderer.material.SetColor("_Color", new Color(this.regionColor.r, this.regionColor.g, this.regionColor.b, albedoAlpha));
		
		var emissionAlpha = this._renderer.material.GetColor("_EmissionColor").a;
		this._renderer.material.SetColor("_EmissionColor", new Color(this.regionColor.r, this.regionColor.g, this.regionColor.b, emissionAlpha));
		
		
	}

	public void TrackRail(Rail rail)
	{
		if (!rails.Contains(rail))
		{
			rails.Add(rail);
		}
	}

	public bool HasFullRails()
	{
		return rails.Count == _maxRails;
	}

	public bool IsConnectedTo(Node node)
	{
		for (int i = 0; i < rails.Count; i++)
		{
			if (rails[i].endNode == node || rails[i].originNode == node)
			{
				return true;
			}
		}

		return false;
	}

	public void DestroyMeAndMyRails()
	{
		for (int i=0; i<rails.Count; i++)
		{
			if (rails[i] != null)
			{
				Destroy(rails[i].gameObject);
			}
		}

		Destroy(gameObject);
	}
	
	#region aesthetic 
	#endregion
}