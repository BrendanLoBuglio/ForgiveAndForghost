using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class Rail : MonoBehaviour
{
	public Node originNode;
	public Node endNode;

    public float width { get; private set; }

    private void Start()
    {
        this.width = this.GetComponent<CapsuleCollider>().radius;
    }

    public Vector3 asAxis => (endNode.transform.position - originNode.transform.position).normalized;

	[Header("References")]
	public LineRenderer lineRenderer;

	public void SetNodes(Node origin, Node end)
	{
		originNode = origin;
		endNode = end;
		lineRenderer.positionCount = 2;
		lineRenderer.SetPosition(0, origin.transform.position);
		lineRenderer.SetPosition(1, end.transform.position);
		origin.TrackRail(this);
		end.TrackRail(this);
	}

	public bool HasNodes()
	{
		return (originNode != null && endNode != null);
	}
}