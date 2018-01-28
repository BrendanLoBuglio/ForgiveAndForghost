﻿using System.Collections;
using System.Linq;
using System.Collections.Generic;
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

    public Rail getNextRailOnArrive(Rail fromRail)
    {
        //If there's only one rail, go to the other rail:
        if (rails.Count == 2)
            return this.rails.Where(rail => rail != fromRail).ToList()[0];
        else
            throw new System.Exception("Haven't handled this case yet");
    }
	protected int _maxRails = 4;
	protected int _minNodeDistance = 30;
	protected int _maxNodeDistance = 300;

	public void FindClosestNodes()
	{
		Node[] nodeArray = FindObjectsOfType<Node>();

		List<NodeAndDistance> closestNodes = new List<NodeAndDistance>();
		int numRailsWeNeed = _maxRails - rails.Count;

		for (int i = 0; i < nodeArray.Length; i++)
		{
			//Debug.Log("b");
			if (!nodeArray[i].HasFullRails())
			{
				//Debug.Log("a");
				if (nodeArray[i] != this)
				{
					//Debug.Log("hey");	
					if (!IsConnectedTo(nodeArray[i]))
					{
						//Debug.Log("hi");
						float distance = Vector3.Distance(transform.position, nodeArray[i].transform.position);

						if (distance < _maxNodeDistance && distance > _minNodeDistance)
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
			}
		}

		//Debug.LogFormat("instantiating {0} rails", closestNodes.Count);
		for (int i = 0; i < closestNodes.Count; i++)
		{
			Rail newRail = RailGenerator.GetNewRail();
			newRail.SetNodes(this, closestNodes[i].node);
		}
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
}
