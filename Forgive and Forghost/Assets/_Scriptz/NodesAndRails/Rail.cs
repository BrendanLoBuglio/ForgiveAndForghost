using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Rail : MonoBehaviour
{
	public Node originNode;
	public Node endNode;
	
    public float width { get; private set; }

    private void Start()
    {
        if (this.GetComponent<CapsuleCollider>() != null) {
            this.width = this.GetComponent<CapsuleCollider>().radius;
        }
    }

	public enum RailSelectionState{notSelected, isSelected, currentlyOn}

	public RailSelectionState selectionState { get; private set; }
    public void setSelectionState(RailSelectionState selectionState)
    {
	    this.selectionState = selectionState;
	    switch (selectionState) {
		    case RailSelectionState.notSelected:
			    this.lineRenderer.material.SetColor("_Color", this._defaultColor);
			    break;
		    case RailSelectionState.isSelected:
			    this.lineRenderer.material.SetColor("_Color", this.originNode.regionColor);
			    break;
		    case RailSelectionState.currentlyOn:
			    this.lineRenderer.material.SetColor("_Color", Color.Lerp(this._defaultColor, this.originNode.regionColor, 0.4f));
			    break;
	    }
	    
	    
	    //this.lineRenderer.startColor = selectionState ? this.originNode.regionColor : this._defaultColor;
	    //this.lineRenderer.endColor   = selectionState ? this.endNode.regionColor    : this._defaultColor;
    }

    /// Don't @ me
    public Node endWhichIsNot(Node n) => this.originNode == n ? this.endNode : this.originNode;
	
	[Header("References")]
	public LineRenderer lineRenderer;

	/*# Cached References #*/
	private Color _defaultColor;
	
	
	private void Awake()
	{
		this._defaultColor = this.lineRenderer.material.GetColor("_Color");
	}

	public void initialize(Node origin, Node end)
	{
		this.originNode = origin;
		this.endNode = end;
		this.lineRenderer.positionCount = 2;
		this.lineRenderer.SetPosition(0, origin.transform.position);
		this.lineRenderer.SetPosition(1, end.transform.position);
		origin.TrackRail(this);
		end.TrackRail(this);
		
		// Set up my default color:
		float nodeH, nodeS, NodeV;
		Color.RGBToHSV(this.originNode.regionColor, out nodeH, out nodeS, out NodeV);
		nodeS *= 0.4f;
		NodeV *= 0.15f;
		this._defaultColor = Color.HSVToRGB(nodeH, nodeS, NodeV);
		this.lineRenderer.material.SetColor("_Color", this._defaultColor);
	}

	public bool hasNodes()
	{
		return (this.originNode != null && this.endNode != null);
	}
}