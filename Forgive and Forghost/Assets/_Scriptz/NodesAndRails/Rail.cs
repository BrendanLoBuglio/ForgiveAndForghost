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

    public Vector3 asAxis => (this.endNode.transform.position - this.originNode.transform.position).normalized;

	[Header("References")]
	public LineRenderer lineRenderer;

	public void setNodes(Node origin, Node end)
	{
		this.originNode = origin;
		this.endNode = end;
		this.lineRenderer.positionCount = 2;
		this.lineRenderer.SetPosition(0, origin.transform.position);
		this.lineRenderer.SetPosition(1, end.transform.position);
		origin.TrackRail(this);
		end.TrackRail(this);
	}

	public bool hasNodes()
	{
		return (this.originNode != null && this.endNode != null);
	}
}