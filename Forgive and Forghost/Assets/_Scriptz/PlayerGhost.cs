using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGhost : MonoBehaviour {

    /*# Scene References #*/
    [SerializeField] private Rail startingRail;

    /*# Config #*/
    [SerializeField] private float maxSpeed_c = 10f;
    [SerializeField] private float acceleration_c = 100f;
    /// In degrees per second the rate of angular change in my twist along a rail given an input
    [SerializeField] private float twistAcceleration_c = 270f;
    [SerializeField] private float twistMaxVelocity_c = 270f;

    /*# State #*/
    private float currentSpeed = 0f;
    private float angularVelocity = 0f;

    /*# Cache #*/
    private Rail currentRail;
    private Node nextNode;

    /// Current angle around rail axis, in degrees
    private float twist;

    void Start () {
        this.currentRail = startingRail;
        this.transform.position = this.startingRail.originNode.transform.position;
	}
	
	void Update () {
        
        // Apply acceleration and rotation:
        this.currentSpeed = Mathf.Clamp(this.currentSpeed + Time.deltaTime * this.acceleration_c, 0f, maxSpeed_c);

        var twistInput = Input.GetAxis("Horizontal");
        if (Mathf.Abs(twistInput) > 0.001f){
            this.angularVelocity = Mathf.Clamp(this.angularVelocity + twistInput * this.twistAcceleration_c * Time.deltaTime, -this.twistMaxVelocity_c, this.twistMaxVelocity_c);
        }
        else {
            this.angularVelocity = 0f;
        }
        this.twist = (this.twist + twistInput * twistAcceleration_c * Time.deltaTime) % 360f;

        // Position and Rotate body appropriately along rail:
        this.transform.rotation = Quaternion.AngleAxis(this.twist, this.currentRail.asAxis) * Quaternion.LookRotation(this.currentRail.asAxis);
        this.transform.position += this.currentRail.asAxis * this.currentSpeed * Time.deltaTime;
    }


}
