﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using JetBrains.Annotations;
using UnityEngine;
using _Scriptz.TheGamePartOfTheGame;

public class PlayerGhost : MonoBehaviour {

    /*# Scene References #*/
    [SerializeField] private Node _startFromNode;
    [SerializeField] private Node _startToNode;
	[SerializeField] private CameraController _cameraController;
	public CameraController cameraController { get { return _cameraController; } }
	public Transform letterDeliveryAndReceiptPoint;
	public GhostyCloth ghostyCloth;
	public STUPIDSINWAVE ghostySinWave;

    /*# Config #*/
    [SerializeField] private float _maxSpeed_c = 30f;
	[SerializeField] private float _maxSpeedBoostMultiplier_c = 2.5f;
    [SerializeField] private float _acceleration_c = 10f;
    /// In degrees per second the rate of angular change in my _twistAngle along a rail given an input
    [SerializeField] private float _twistAcceleration_c = 270f;
    [SerializeField] private float _twistMaxVelocity_c = 270f;

    [SerializeField] private float _easeIntoNodeDistance_c = 1f;

    public AudioClip speedUpClip;
    public float minSpeedUpClipVolume = .5f;
    public float maxSpeedUpClipVolume = .7f;

    /*# Physical State #*/
    private float _currentSpeed = 0f;
    private float _angularVelocity = 0f;
    /// Current angle around rail axis, in degrees
    private float _twistAngle;
    private bool _hasLockedIntoCurrentSelection = false;
    private bool _isStoppedForCutscene = false;
    
    /*# Object Reference State #*/
    private Node _fromNode;
    private Node _toNode;
    private Rail _currentlySelectedRail;
    private Rail _currentlyOnRail;
    private PortalNode _goalPortalNode;

    /*# Cache #*/
    public ParticleSystem sparkParticleSystem;
    public ParticleSystem flareParticleSystem;
    public ParticleSystem _speedLineParticleSystem;
    public float maxGrindVolume;
    private AudioSource grindSource;
    private float _fastSpeedLineEmissionRate;
    private float sparkEmissionRate;
    private float flareEmissionRate;
    private float grindDampVel;


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
        grindSource = GetComponent<AudioSource>();
	}

    public void setStartNodesAndGoal(Node from, Node to, PortalNode goal)
    {
        this._startFromNode = from;
        this._startToNode = to;
        this._goalPortalNode = goal;
    }

    public void Initialize()
    {
        this._initialized = true;
        this._fromNode = this._startFromNode;
        this._toNode = this._startToNode;
        this.transform.position = this._fromNode.transform.position;
        this._fromNode.getRailByDestinationNode(this._toNode).setSelectionState(Rail.RailSelectionState.currentlyOn);

        // Initialize speed lines:
        if (this._speedLineParticleSystem != null)
        this._fastSpeedLineEmissionRate = this._speedLineParticleSystem.emission.rateOverTime.constant;
        this.sparkEmissionRate = sparkParticleSystem.emission.rateOverTime.constant;
        this.flareEmissionRate = flareParticleSystem.emission.rateOverTime.constant;
    }
	
	private void Update () {
        if (!this._initialized || this._isStoppedForCutscene) {
            return;
        }

        // Get some values baked out:
	    var fromPos = this._fromNode.transform.position;
	    var toPos = this._toNode.transform.position;
	    var railAxis = (toPos - fromPos).normalized;
		
		var maxSpeedModifier = 1f;
		var inSlowOnApproachMode = (!this._hasLockedIntoCurrentSelection && this._toNode != this._goalPortalNode && this._currentlySelectedRail != null &&
	                                (this.transform.position - toPos).sqrMagnitude <=
	                                this._easeIntoNodeDistance_c * this._easeIntoNodeDistance_c);
	    
		// If I'm locked into my choice, move a little bit faster:
		if (this._hasLockedIntoCurrentSelection)
			maxSpeedModifier = this._maxSpeedBoostMultiplier_c;
	    // Otherwise, as I'm reaching my destination node, clamp my max speed to slow down:
	    else if (inSlowOnApproachMode)
	        maxSpeedModifier = Mathf.Clamp01((Vector3.Distance(this.transform.position, toPos) - 0.5f) / this._easeIntoNodeDistance_c) ;
		
	    if(inSlowOnApproachMode)
	        UIManager.singleton.ShowRailMessage();
	    else
	        UIManager.singleton.HideRailMessage();
	    
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
	    Rail newSelectedRail;
	    if (this._toNode == this._goalPortalNode) {
	        // We're approaching a portal node, so there's no selected rail:
	        newSelectedRail = null;
	    }
	    else {
	        // We're approaching a regular node, so figure out which one it is: 
	        newSelectedRail = this.calculateWhichRailIsSelected();
	    }
	    
	    if (this._currentlySelectedRail != newSelectedRail) {
	        this._currentlySelectedRail?.setSelectionState(this._currentlySelectedRail.selectionState != Rail.RailSelectionState.currentlyOn ? Rail.RailSelectionState.notSelected : Rail.RailSelectionState.currentlyOn);
	        newSelectedRail?.setSelectionState(Rail.RailSelectionState.isSelected);
	        this._currentlySelectedRail = newSelectedRail;
	    }

        // Check for input to lock the player into their current rail:
        if (Input.GetKeyDown(KeyCode.Space) && this._currentlySelectedRail != null) {
            if (!this._hasLockedIntoCurrentSelection)
            {
                GameObject audioSourceObject = new GameObject("Audio Clip");
                audioSourceObject.transform.position = transform.position;
                AudioSource newSource = audioSourceObject.AddComponent<AudioSource>();
                newSource.clip = speedUpClip;
                newSource.volume = Random.Range(minSpeedUpClipVolume, maxSpeedUpClipVolume);
                newSource.pitch = Random.Range(.7f, 1.1f);
                newSource.Play();
                Destroy(audioSourceObject, speedUpClip.length);
            }
            this._hasLockedIntoCurrentSelection = true;
        }
		
        // Check to see if I've passed the end of my current rail:
        if(Vector3.Dot(toPos - this.transform.position, toPos - fromPos) < 0f) {

            // Hit our portal node!!
            if (this._toNode == this._goalPortalNode) {
                // Stop and go play the cutscene for our mission finishing:
                this._isStoppedForCutscene = true;
                GameplayManager.singleton.finishCurrentMissionPart((newGoal) => {
                    // After the cutscene finishes, un-stop and go in a direction
                    this._isStoppedForCutscene = false;
                    this.changeRail(this._toNode.GetFirstRail().endWhichIsNot(this._toNode));
                    this._goalPortalNode = newGoal;
                });
            }
            // Just going to a new rail:
            else {
                // If I have a selected rail, go to it:
				if (this._currentlySelectedRail != null)
				{
					// If I have selected it officially:
					if (this._hasLockedIntoCurrentSelection)
                    	// If so, move to the next rail:
                    	this.changeRail(this._currentlySelectedRail.endWhichIsNot(this._toNode));
				}
                // Otherwise, dead end! Just stop me in my tracks:
                else
				{
					this.transform.position = this._toNode.transform.position;
					this.changeRail(this._toNode.GetFirstRail().endWhichIsNot(this._toNode));
				}
            }
        }
        this.updateWindParticles();
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

    public void changeRail(Node newNode)
    {
        this._fromNode = this._toNode;
        this._toNode = newNode;
	    this._hasLockedIntoCurrentSelection = false;
        
        this._currentlyOnRail?.setSelectionState(Rail.RailSelectionState.notSelected);
        this._currentlyOnRail = this._fromNode.getRailByDestinationNode(this._toNode);
        this._currentlyOnRail?.setSelectionState(Rail.RailSelectionState.currentlyOn);

		this.transform.position = this._fromNode.transform.position;

		_cameraController?.WaitAFrame();
    }
    
    private void updateWindParticles()
    {
        var lerpAmount = Mathf.InverseLerp(0, this._maxSpeed_c * this._maxSpeedBoostMultiplier_c, this._currentSpeed);
        float targetGrindVolume = Mathf.Lerp(0, maxGrindVolume, lerpAmount);
        lerpAmount *= lerpAmount;
        grindSource.volume = Mathf.SmoothDamp(grindSource.volume, targetGrindVolume, ref grindDampVel, .5f, 100, Time.deltaTime);
        var emiss = this._speedLineParticleSystem.emission;
        var rate = emiss.rateOverTime;
        rate.constant = Mathf.Lerp(0, this._fastSpeedLineEmissionRate, lerpAmount);
        emiss.rateOverTime = rate;
        emiss = this.sparkParticleSystem.emission;
        rate = emiss.rateOverTime;
        rate.constant = Mathf.Lerp(0, this.sparkEmissionRate, lerpAmount);
        //Debug.Log(rate.constant);
        emiss.rateOverTime = rate;
        emiss = this.flareParticleSystem.emission;
        rate = emiss.rateOverTime;
        rate.constant = Mathf.Lerp(0, this.flareEmissionRate, lerpAmount);
        emiss.rateOverTime = rate;

		_cameraController?.UpdateSpeedRelatedLerpThings(lerpAmount);
		ghostyCloth.UpdateXZValue(lerpAmount);
		ghostySinWave.UpdateSinWave(lerpAmount);
    }
}
