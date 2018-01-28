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
}