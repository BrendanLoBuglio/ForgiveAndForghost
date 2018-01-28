using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGhost : MonoBehaviour {

    /*# Scene References #*/
    [SerializeField] private Rail _startingRail_c;

    /*# Config #*/
    [SerializeField] private float _maxSpeed_c = 30f;
    [SerializeField] private float _acceleration_c = 10f;
    /// In degrees per second the rate of angular change in my _twistAngle along a rail given an input
    [SerializeField] private float _twistAcceleration_c = 270f;
    [SerializeField] private float _twistMaxVelocity_c = 270f;

    /// Must be at this distance or less to be arrived at the node
    [SerializeField] private float _arrivedAtNodeTolerance_c = 0.5f;

    /*# State #*/
    private float _currentSpeed = 0f;
    private float _angularVelocity = 0f;
    /// Current angle around rail axis, in degrees
    private float _twistAngle;

    private Node _targetNode;

    /*# Cache #*/
    private Rail _currentRail;
    private Node _nextNode;
    private ParticleSystem _speedLineParticleSystem;
    private float _fastSpeedLineEmissionRate;
    
    

    private void Start () {
        this._currentRail = this._startingRail_c;
        this.transform.position = this._startingRail_c.originNode.transform.position;
        
        // Initialize speed lines:
        this._speedLineParticleSystem = this.GetComponentInChildren<ParticleSystem>();
        this._fastSpeedLineEmissionRate = this._speedLineParticleSystem.emission.rateOverTime.constant;
	}
	
	private void Update () {
        // Apply acceleration:
        this._currentSpeed = Mathf.Clamp(this._currentSpeed + Time.deltaTime * this._acceleration_c, 0f, this._maxSpeed_c);

        // Rotate:
        var twistInput = Input.GetAxis("Horizontal");
        if (Mathf.Abs(twistInput) > 0.001f)
            this._angularVelocity = Mathf.Clamp(this._angularVelocity + twistInput * this._twistAcceleration_c * Time.deltaTime, -this._twistMaxVelocity_c, this._twistMaxVelocity_c);
        else
            this._angularVelocity = 0f;
        this._twistAngle = (this._twistAngle + twistInput * this._twistAcceleration_c * Time.deltaTime) % 360f;

        // Move along rail:
        this.transform.position += this._currentRail.asAxis * this._currentSpeed * Time.deltaTime;

        // Check to see if I've passed the end of my current rail:
        var mPos = this.transform.position;
        if(Vector3.Dot(this._currentRail.endNode.transform.position - mPos, this._currentRail.endNode.transform.position - this._currentRail.originNode.transform.position) < 0f) {
            // If so, move to the next rail:
            this.changeRail(this._currentRail.endNode.getNextRailOnArrive(this._currentRail));
        }

        // Update my rotation along the current rail:
        this.transform.rotation = Quaternion.AngleAxis(this._twistAngle, this._currentRail.asAxis) * Quaternion.LookRotation(this._currentRail.asAxis);
    }

    private void changeRail(Rail newRail)
    {
        this._currentRail = newRail;
        this.transform.position = newRail.originNode.transform.position;
    }

    private void updateWindParticles()
    {
        var lerpAmount = Mathf.InverseLerp(0, this._maxSpeed_c, this._currentSpeed);
        var emiss = this._speedLineParticleSystem.emission;
        var rate = emiss.rateOverTime;
        rate.constant = Mathf.Lerp(0, this._fastSpeedLineEmissionRate, lerpAmount);
        emiss.rateOverTime = rate;
    }
}
