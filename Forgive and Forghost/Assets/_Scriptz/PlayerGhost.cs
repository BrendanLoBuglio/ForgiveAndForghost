using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGhost : MonoBehaviour {

    /*# Scene References #*/
    [SerializeField] private Rail startingRail;

    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float acceleration = 100f;

    private float currentSpeed = 0f;

    private Rail currentRail;
    private Node nextNode;

    /// Current angle around rail axis, in degrees
    [SerializeField] private float twist;

    void Start () {
        this.currentRail = startingRail;
        this.transform.position = this.startingRail.originNode.transform.position;

	}
	
	void Update () {
        // Rotate body to appropriate twist around rail:
        this.transform.rotation = Quaternion.AngleAxis(this.twist, this.currentRail.asAxis) * Quaternion.LookRotation(this.currentRail.asAxis);

        // Accelerate in rail direction:
        this.currentSpeed = Mathf.Clamp(this.currentSpeed + Time.deltaTime * this.acceleration, 0f, maxSpeed);

        // Update position along rail:
        this.transform.position += this.currentRail.asAxis * this.currentSpeed * Time.deltaTime;
	}


}
