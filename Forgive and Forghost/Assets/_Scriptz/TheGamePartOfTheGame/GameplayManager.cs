﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scriptz.TheGamePartOfTheGame
{
    public class GameplayManager : MonoBehaviour
    {
        /*# Scene References #*/
		[Header("Scene References")]
        public List<PortalNode> hellPortals = new List<PortalNode>();
        public List<PortalNode> wotlPortals = new List<PortalNode>();
		public Camera aerialCamera;

        /*# Config #*/
		[Header("Config")]
		public LevelData levelData;
        public float messageDegredationDuration_c = 30f;
        
        /*# State #*/
        public int missionIndex { get; private set; }
        public UniverseType_E currentMissionHalf { get; private set; }
        private List<PortalNode> _usedNodes = new List<PortalNode>();
        private bool _isInCutscene = false;

        private string _currentMessage;
        private float _messageDegradeTimer;
        private PortalNode _currentGoalPortal;
		private int _messagesDelivered;
		protected CameraFacingBillboard _ghostyMapIcon;
		protected bool _theGameHasBegun;

        /*public static GameplayManager singleton => _singleton ?? (_singleton = FindObjectOfType<GameplayManager>());
        private static GameplayManager _singleton;*/ // commenting this out and doing it the other way cuz i was gettinga null ref but i think it was actually related to something else but idk how to fix the other thing

		private static GameplayManager _singleton;
		public static GameplayManager singleton
		{
			get
			{
				if (_singleton == null) _singleton = UnityEngine.Object.FindObjectOfType<GameplayManager>();
				return _singleton;
			}
		}

        private void Awake()
        {
            this.currentMissionHalf = UniverseType_E.WOTL;
            this._messageDegradeTimer = this.messageDegredationDuration_c;

			if (aerialCamera != null)
			{
				aerialCamera.enabled = false;
			}

			if (_ghostyMapIcon == null)
			{
				_ghostyMapIcon = UnityEngine.Object.FindObjectOfType<CameraFacingBillboard>();
				_ghostyMapIcon.gameObject.SetActive(false);
			}
        }

		void Start()
		{
			StartCoroutine(DoStartGame());
		}

		protected IEnumerator DoStartGame()
		{
			yield return null; // waiting a frame so OnTriggerEnter can register all overlapping RailGenerator's! see "RegisterOverlappingGenerator()" in RailGenerator
			initializeGame();
		}

		protected void SetNewMessage()
		{
			this._currentMessage = this.currentMissionHalf == UniverseType_E.WOTL
				? this.levelData.missions[this.missionIndex].wotlQuestion
				: this.levelData.missions[this.missionIndex].hellAnswer;
		}

        public void initializeGame()
        {
			// Generate the level!
			RailGeneratorManager.s.GenerateAllTheRails();

			// Make sure we're referencing some level data
			if (levelData == null)
			{
				Debug.LogError("There is no LevelData asset assigned in the inspector! Please create and assign one! Ty");
			}

			SetupPortalsAndMissions();
            PlayerGhost.s.Initialize();

			_theGameHasBegun = true; // IT HAS BEGUN!
        }

		protected void SetupPortalsAndMissions()
		{
			// Get starting portals:
			var startingPortal = this.wotlPortals[Random.Range(0, this.wotlPortals.Count)];
			this._currentGoalPortal = this.hellPortals[Random.Range(0, this.hellPortals.Count)];
			this._usedNodes.Add(startingPortal);
			this._usedNodes.Add(this._currentGoalPortal);

			// Initialize first mission:
			this._currentGoalPortal.setIsGoal(true);
			// Get new message:
			SetNewMessage();

			SetMessagesDelivered(0);

			UIManager.singleton.ShowWotlRecieved();

			Debug.Log($"The message is {this._currentMessage}");
			UIManager.singleton.SetNewMessage(this._currentMessage, true);


			PlayerGhost.s.setStartNodesAndGoal(startingPortal, startingPortal.GetFirstRail().endWhichIsNot(startingPortal), this._currentGoalPortal);
		}

        public void finishCurrentMissionPart(Action<PortalNode> onCutsceneFinishedCallback)
        {
            Debug.Log($"Finished the {this.currentMissionHalf} half of mission {this.missionIndex} with remaining message {this._currentMessage}");
            
            //Show our decoded message: 
            //UIManager.singleton.SetNewMessage(this._currentMessage, false);
			string decodedMessage = this._currentMessage;
            
            // Use up our last goal:
            this._currentGoalPortal.setIsGoal(false);
			PortalNode portalNodeWeJustReached = _currentGoalPortal;
            this._usedNodes.Add(this._currentGoalPortal);

			SetMessagesDelivered(_messagesDelivered + 1);

			// Swap the message half, and if necessary increment the index
			if (this.currentMissionHalf == UniverseType_E.WOTL)
			{
				this.currentMissionHalf = UniverseType_E.HELL;
			}
			else
			{
				this.currentMissionHalf = UniverseType_E.WOTL;

				this.missionIndex++;
			}

			if (_messagesDelivered < (levelData.missions.Length * 2))
			{
				// Get new message:
				SetNewMessage();
            
				Debug.Log($"Now we're on the {this.currentMissionHalf} half of mission {this.missionIndex} with the new message {this._currentMessage}");
            
				//Reset timer:
				this._messageDegradeTimer = this.messageDegredationDuration_c;
            
				// Get a random new portal which has not been used
				var nextPortal = (this.currentMissionHalf == UniverseType_E.WOTL ? this.wotlPortals : this.hellPortals)
                .Where(portal => !this._usedNodes.Contains(portal))
                .OrderBy(portal => Random.Range(0f, 1f)).ToList()[0]; //Random index

				this._currentGoalPortal = nextPortal;

				this.StartCoroutine(this.finishedMissionCutscene(onCutsceneFinishedCallback, nextPortal, portalNodeWeJustReached, decodedMessage, false));
			}
			else
			{
				this.StartCoroutine(this.finishedMissionCutscene(onCutsceneFinishedCallback, null, portalNodeWeJustReached, decodedMessage, true));
			}
        }

		public void FinishFinalMission()
		{
			Debug.Log("you've done it!");
			UnityEngine.SceneManagement.SceneManager.LoadScene(OverallEverythingManager.s.victorySceneIndex);
		}

		protected void SetMessagesDelivered(int messagesDelivered)
		{
			_messagesDelivered = messagesDelivered;
			UIManager.singleton.SetMessagesDelieveredText(_messagesDelivered, levelData.missions.Length * 2);
		}

		private IEnumerator finishedMissionCutscene(Action<PortalNode> onCutsceneFinishedCallback, PortalNode nextGoal, PortalNode goalWeJustReached, string decodedMessage, bool isFinalMessage)
        {
            this._isInCutscene = true;
			goalWeJustReached.cutsceneManager.OnCustceneComplete += HandleCutsceneComplete;
			goalWeJustReached.cutsceneManager.StartCutscene(decodedMessage, isFinalMessage);

			while (this._isInCutscene)
			{
				yield return null;
			}

			goalWeJustReached.cutsceneManager.OnCustceneComplete -= HandleCutsceneComplete;

			if (isFinalMessage)
			{
				FinishFinalMission();
			}
			else
			{
				if (nextGoal != null)
				{
					nextGoal.setIsGoal(true);
					if (this.currentMissionHalf == UniverseType_E.HELL) UIManager.singleton.ShowGhostRecieved();
					else UIManager.singleton.ShowWotlRecieved();
            
					UIManager.singleton.SetNewMessage(this._currentMessage, true);
					onCutsceneFinishedCallback(nextGoal);
				}
				else
				{
					Debug.LogError("uh oh..");
				}
			}
        }

		protected void HandleCutsceneComplete()
		{
			this._isInCutscene = false;
		}

        private void Update()
        {
			if (!_theGameHasBegun)
			{
				return;
			}

            // Lose letters over time:
            if (!this._isInCutscene && this._currentMessage.Length > 1) {
                this._messageDegradeTimer -= Time.deltaTime;
                while (this._messageDegradeTimer <= 0f) {
                    var removeLetterIndex = Random.Range(0, this._currentMessage.Length);
                    this._currentMessage = this._currentMessage.Remove(removeLetterIndex, 1);
                    this._messageDegradeTimer += this.messageDegredationDuration_c;
                    Debug.Log($"Lost a letter! Message is now {this._currentMessage}");
                    UIManager.singleton.SetNewMessage(this._currentMessage, true);
                }
            }

			if (aerialCamera != null && PlayerGhost.s != null)
			{
				if (Input.GetKeyDown(KeyCode.Q))
				{
					PlayerGhost.s.cameraController.myCamera.enabled = false;
					aerialCamera.enabled = true;
					_ghostyMapIcon.gameObject.SetActive(true);
				}

				if (Input.GetKeyUp(KeyCode.Q))
				{
					aerialCamera.enabled = false;
					PlayerGhost.s.cameraController.myCamera.enabled = true;
					_ghostyMapIcon.gameObject.SetActive(false);
				}
			}
        }
    }
}