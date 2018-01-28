using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using JetBrains.Annotations;
using UnityEngine;

public class PlayerGhost : MonoBehaviour {

    /*# Scene References #*/
    [SerializeField] private Rail _startingRail_c;

    /*# Config #*/
    [SerializeField] private float _maxSpeed_c = 30f;
	[SerializeField] private float _maxSpeedBoostMultiplier_c = 2.5f;
    [SerializeField] private float _acceleration_c = 10f;
    /// In degrees per second the rate of angular change in my _twistAngle along a rail given an input
    [SerializeField] private float _twistAcceleration_c = 270f;
    [SerializeField] private float _twistMaxVelocity_c = 270f;

    [SerializeField] private float _easeIntoNodeDistance_c = 1f;
    
    /*# Physical State #*/
    private float _currentSpeed = 0f;
    private float _angularVelocity = 0f;
    /// Current angle around rail axis, in degrees
    private float _twistAngle;
    private bool _hasLockedIntoCurrentSelection = false;

    /*# Object Reference State #*/
    private Node _fromNode;
    private Node _toNode;
    private Rail _currentlySelectedRail;
    
    /*# Cache #*/
    private ParticleSystem _speedLineParticleSystem;
    private float _fastSpeedLineEmissionRate;

    public bool dontInitializeOnAwake;
    private bool _initialized;

    private static PlayerGhost _myPrivateSelf;
    public static PlayerGhost s {
        get {
            if (_myPrivateSelf == null) {
                _myPrivateSelf = Object.FindObjectOfType<PlayerGhost>();
            }
            return _myPrivateSelf;
        }
    }
    
    private void Awake () {
        if (!dontInitializeOnAwake) {
            Initialize();
        }
	}

    public void SetStartRail(Rail rail)
    {
        this._startingRail_c = rail;
    }

    public void Initialize()
    {
        _initialized = true;
        this._fromNode = this._startingRail_c.originNode;
        this._toNode = this._startingRail_c.endNode;
        this.transform.position = this._fromNode.transform.position;

        // Initialize speed lines:
        this._speedLineParticleSystem = this.GetComponentInChildren<ParticleSystem>();
        if (this._speedLineParticleSystem != null)
        this._fastSpeedLineEmissionRate = this._speedLineParticleSystem.emission.rateOverTime.constant;
    }
	
	private void Update () {
        if (!_initialized) {

            return;
        }

        // Get some values baked out:
	    var fromPos = this._fromNode.transform.position;
	    var toPos = this._toNode.transform.position;
	    var railAxis = (toPos - fromPos).normalized;
		
		var maxSpeedModifier = 1f; 
		// If I'm locked into my choice, move a little bit faster:
		if (this._hasLockedIntoCurrentSelection)
			maxSpeedModifier = _maxSpeedBoostMultiplier_c;
	    // Otherwise, as I'm reaching my destination node, clamp my max speed to slow down:
	    else if ((this.transform.position - toPos).sqrMagnitude <= this._easeIntoNodeDistance_c * this._easeIntoNodeDistance_c)
	        maxSpeedModifier = Mathf.Clamp01((Vector3.Distance(this.transform.position, toPos) - 0.5f) / this._easeIntoNodeDistance_c) ;
		
	    // Apply acceleration:
        this._currentSpeed = Mathf.Clamp(this._currentSpeed + Time.deltaTime * this._acceleration_c, 0f, maxSpeedModifier * this._maxSpeed_c);
        // Move along rail:
        this.transform.position += railAxis * this._currentSpeed * Time.deltaTime;
		
        // Rotate:
        var twistInput = !this._hasLockedIntoCurrentSelection ? -Input.GetAxis("Horizontal") : 0f;
        if (Mathf.Abs(twistInput) > 0.001f)
            this._angularVelocity = Mathf.Clamp(this._angularVelocity + twistInput * this._twistAcceleration_c * Time.deltaTime, -this._twistMaxVelocity_c, this._twistMaxVelocity_c);
        else
            this._angularVelocity = 0f;
        this._twistAngle = (this._twistAngle + twistInput * this._twistAcceleration_c * Time.deltaTime) % 360f;
        // Update my rotation along the current rail:
        this.transform.rotation = Quaternion.AngleAxis(this._twistAngle, railAxis) * Quaternion.LookRotation(railAxis);

	    // Calculate which rail is our next rail:
	    var newSelectedRail = this.calculateWhichRailIsSelected();
	    if (this._currentlySelectedRail != newSelectedRail) {
	        this._currentlySelectedRail?.setIsSelected(false);
	        newSelectedRail?.setIsSelected(true);
	        this._currentlySelectedRail = newSelectedRail;
	    }

		// Check for input to lock the player into their current rail:
		if (Input.GetKeyDown(KeyCode.Space) && this._currentlySelectedRail != null)
			this._hasLockedIntoCurrentSelection = true;
		
        // Check to see if I've passed the end of my current rail:
        if(Vector3.Dot(toPos - this.transform.position, toPos - fromPos) < 0f) {
            // If I have a selected rail, go to it:
            if (this._currentlySelectedRail != null)
                // If so, move to the next rail:
                this.changeRail(this._currentlySelectedRail.endWhichIsNot(this._toNode));
            // Otherwise, dead end! Just stop me in my tracks:
            else
                this.transform.position = this._toNode.transform.position;
        }
    }
    
    private Rail calculateWhichRailIsSelected()
    {
        // Get all the nodes that my "to" node connects to, with the exception of the one I'm already coming from
        var potentialTargetNodes = this._toNode.getPossibleNodesForJunction(this._fromNode);
        
        // If there are no potential targets, then I'm heading to a dead end:
        if (potentialTargetNodes.Length == 0)
            return null;
        
        // If there's only one potential target, then that's automatically the target we'll be going to: 
        else if (potentialTargetNodes.Length < 2)
            return this._toNode.getRailByDestinationNode(potentialTargetNodes[0]);
        
        // Otherwise, we have multiple tarets that we need to sift through!

        // Get some values baked out:
        var fromPos = this._fromNode.transform.position;
        var toPos = this._toNode.transform.position;
        var railAxis = (toPos - fromPos).normalized;
        var myUp = this.transform.up;  // We use a Quaternion.AngleAxis to align our up with our twist angle 

        var selectedNode = potentialTargetNodes.Select(node => {
            // Project the vector from my current "to" node to each potential target node upon the plane of my current rail axis
            var perpendicular = Vector3.ProjectOnPlane((node.transform.position - fromPos).normalized, railAxis);
            // Get the shortest angle between my current twist and these other angles
            return new System.Tuple<Node, float>(node, Vector3.Angle(myUp, perpendicular));
        })
            // Sort by smallest angle and return that node:
         .OrderBy(nodeAngleTuple => nodeAngleTuple.Item2)
         .ToList()[0].Item1;

        return this._toNode.getRailByDestinationNode(selectedNode);
    }

    private void changeRail(Node newNode)
    {
        this._fromNode = this._toNode;
        this._toNode = newNode;
	    this._hasLockedIntoCurrentSelection = false;
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
