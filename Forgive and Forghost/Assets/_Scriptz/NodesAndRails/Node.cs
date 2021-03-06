﻿using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class NodeSearchSettings
{
	public bool includeRegardlessOfMaxRails;

	public NodeSearchSettings()
	{
		includeRegardlessOfMaxRails = false;
	}

	public NodeSearchSettings(bool _includeRegardlessOfMaxRails)
	{
		includeRegardlessOfMaxRails = _includeRegardlessOfMaxRails;
	}
}

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

	protected bool IsNodeOkayToConnectTo(Node n, NodeSearchSettings searchSettings)
	{
		if (PassesRailLimitCheck(n, searchSettings))
		{
			if (n != this)
			{
				if (!IsConnectedTo(n))
				{
					if (!(n is PortalNode))
					{
						return true;
					}
				}
			}
		}

		return false;
	}

	protected bool PassesRailLimitCheck(Node n, NodeSearchSettings searchSettings)
	{
		if (searchSettings.includeRegardlessOfMaxRails)
		{
			return true;
		}
		else
		{
			return (!n.HasFullRails());
		}
	}

	private List<NodeAndDistance> GetListOfClosestNodes(Node[] nodeArray, int numRailsWeNeed, NodeSearchSettings searchSettings)
	{
		List<NodeAndDistance> closestNodes = new List<NodeAndDistance>();

		for (int i = 0; i < nodeArray.Length; i++)
		{
			if (IsNodeOkayToConnectTo(nodeArray[i], searchSettings))
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
							// hmmm we can make this part better eventually by replacing the one that's furthest away instead of the first one we happen to find...
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

		return closestNodes;
	}

	public void FindClosestNodes(NodeSearchSettings searchSettings)
	{
		Node[] nodeArray = FindObjectsOfType<Node>();
		int numRailsWeNeed = _maxRails - rails.Count;
		List<NodeAndDistance> closestNodes = GetListOfClosestNodes(nodeArray, numRailsWeNeed, searchSettings);
		ConnectMeToListOfNodes(closestNodes);
	}

	public void MakeSureIHaveAtLeastTwoRails()
	{
		if (rails.Count < 2) // [x] check if i have less than 2 rails
		{
			name = ("Rehabilitated Node " + UnityEngine.Random.Range(0, 999));
			Debug.LogFormat("node {0} only has {1} rails :(\ngonna make it so it has at least 2 rails :)", name, rails.Count);
			int numNodesINeed = 2 - rails.Count; // [x] x is the number of nodes i need to connect to (will either be 1 or 2)
			Node[] nodeArray = FindObjectsOfType<Node>();
			List<NodeAndDistance> closestNodes = GetListOfClosestNodes(nodeArray, numNodesINeed, new NodeSearchSettings(true)); // [x] find the x closest nodes to myself that are not me and that i'm not connected to, but ignore if they have max rails or not
			ConnectMeToListOfNodes(closestNodes); // [x] foreach of those nodes, create a new rail and set it up with us (regardless of other node's max rails)
		}
	}

	private void ConnectMeToListOfNodes(List<NodeAndDistance> closestNodes)
	{
		for (int i = 0; i < closestNodes.Count; i++)
		{
			Rail newRail = RailGeneratorManager.GetNewRail();
			newRail.initialize(this, closestNodes[i].node);
		}
	}

	public void ConnectMeToNode(Node node)
	{
		Rail newRail = RailGeneratorManager.GetNewRail();
		newRail.initialize(this, node);
	}

	public void SetupColor(Color regionColorIn)
	{
		if (this._renderer == null)
		{
			this._renderer = GetComponentInChildren<Renderer>();
		}

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
		return rails.Count >= _maxRails;
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

	public Node[] GetConnectedNodes()
	{
		Node[] connectedNodeArray = new Node[rails.Count];

		for (int i = 0; i < rails.Count; i++)
		{
			connectedNodeArray[i] = rails[i].endWhichIsNot(this);
		}

		return connectedNodeArray;
	}
	
	#region aesthetic 
	#endregion
}