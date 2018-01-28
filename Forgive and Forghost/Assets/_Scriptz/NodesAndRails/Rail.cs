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

    public void setIsSelected(bool isSelected)
    {
        if(this._renderer != null)
            this._renderer.material = isSelected ? this.selectedMaterial : this._defaultMaterial;
    }

    /// Don't @ me
    public Node endWhichIsNot(Node n) => this.originNode == n ? this.endNode : this.originNode;
	
	[Header("References")]
	public LineRenderer lineRenderer;
	public Material selectedMaterial;

	/*# Cached References #*/
	private Material _defaultMaterial;
	private Renderer _renderer;
	
	private void Awake()
	{
		this._renderer = this.GetComponent<Renderer>();
        if (this._renderer != null) {
            this._defaultMaterial = this._renderer.material;
        }
	}

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