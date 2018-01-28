using System.Runtime.CompilerServices;
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

	public void setIsSelected(bool isSelected) => this._renderer.material = isSelected ? this.selectedMaterial : this._defaultMaterial;  
	
	[Header("References")]
	public LineRenderer lineRenderer;
	public Material selectedMaterial;

	/*# Cached References #*/
	private Material _defaultMaterial;
	private Renderer _renderer;
	
	private void Awake()
	{
		this._renderer = this.GetComponent<Renderer>();
		this._defaultMaterial = this._renderer.material;
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