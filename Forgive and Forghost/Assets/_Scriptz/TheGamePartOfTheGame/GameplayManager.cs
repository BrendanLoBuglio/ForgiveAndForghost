using System;
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
        public List<PortalNode> hellPortals = new List<PortalNode>();
        public List<PortalNode> wotlPortals = new List<PortalNode>();
        
        /*# Config #*/
        public Mission[] missions;
        public float messageDegredationDuration_c = 15f;
        
        /*# State #*/
        public int missionIndex { get; private set; }
        public UniverseType_E currentMissionHalf { get; private set; }
        private List<PortalNode> _usedNodes = new List<PortalNode>();
        private bool _isInCutscene = false;

        private string _currentMessage;
        private float _messageDegradeTimer;
        private PortalNode _currentGoalPortal;
		private int _messagesDelivered;

        public static GameplayManager singleton => _singleton ?? (_singleton = FindObjectOfType<GameplayManager>());
        private static GameplayManager _singleton;

        private void Awake()
        {
            this.currentMissionHalf = UniverseType_E.WOTL;
            this._messageDegradeTimer = this.messageDegredationDuration_c;
        }

        public void initializeGame()
        {
            // Get starting portals:
            var startingPortal = this.wotlPortals[Random.Range(0, this.wotlPortals.Count)];
            this._currentGoalPortal = this.hellPortals[Random.Range(0, this.hellPortals.Count)];
            this._usedNodes.Add(startingPortal);
            this._usedNodes.Add(this._currentGoalPortal);
            
            // Initialize first mission:
            this._currentGoalPortal.setIsGoal(true);
            // Get new message:
            this._currentMessage = this.currentMissionHalf == UniverseType_E.WOTL
                ? this.missions[this.missionIndex].wotlQuestion
                : this.missions[this.missionIndex].hellAnswer;

			SetMessagesDelivered(0);
            
            UIManager.singleton.ShowWotlRecieved();
            
            Debug.Log($"The message is {this._currentMessage}");
            UIManager.singleton.SetNewMessage(this._currentMessage, true);
            
            
            PlayerGhost.s.setStartNodesAndGoal(startingPortal, startingPortal.GetFirstRail().endWhichIsNot(startingPortal), this._currentGoalPortal);
            PlayerGhost.s.Initialize();
        }

        public void finishCurrentMissionPart(Action<PortalNode> onCutsceneFinishedCallback)
        {
            Debug.Log($"Finished the {this.currentMissionHalf} half of mission {this.missionIndex} with remaining message {this._currentMessage}");
            
            //Show our decoded message: 
            UIManager.singleton.SetNewMessage(this._currentMessage, false);
            
            // Use up our last goal:
            this._currentGoalPortal.setIsGoal(false);
            this._usedNodes.Add(this._currentGoalPortal);
            
            // Swap the message half, and if necessary increment the index
            if (this.currentMissionHalf == UniverseType_E.WOTL) {
                this.currentMissionHalf = UniverseType_E.HELL;
            }
            else {
                this.currentMissionHalf = UniverseType_E.WOTL;
                
                this.missionIndex++;
            }
            
            // Get new message:
            this._currentMessage = this.currentMissionHalf == UniverseType_E.WOTL
                ? this.missions[this.missionIndex].wotlQuestion
                : this.missions[this.missionIndex].hellAnswer;
            
            Debug.Log($"Now we're on the {this.currentMissionHalf} half of mission {this.missionIndex} with the new message {this._currentMessage}");
            
            //Reset timer:
            this._messageDegradeTimer = this.messageDegredationDuration_c;
            
            // Get a random new portal which has not been used
            var nextPortal = (this.currentMissionHalf == UniverseType_E.WOTL ? this.wotlPortals : this.hellPortals)
                .Where(portal => !this._usedNodes.Contains(portal))
                .OrderBy(portal => Random.Range(0f, 1f)).ToList()[0]; //Random index

			this._currentGoalPortal = nextPortal;

			SetMessagesDelivered(_messagesDelivered + 1);
            
            this.StartCoroutine(this.finishedMissionCutscene(onCutsceneFinishedCallback, nextPortal));
        }

		protected void SetMessagesDelivered(int messagesDelivered)
		{
			_messagesDelivered = messagesDelivered;
			UIManager.singleton.SetMessagesDelieveredText(_messagesDelivered, missions.Length * 2);
		}

        private IEnumerator finishedMissionCutscene(Action<PortalNode> onCutsceneFinishedCallback, PortalNode nextGoal)
        {
            this._isInCutscene = true;
            yield return new WaitForSeconds(8f);
            this._isInCutscene = false;
            
            nextGoal.setIsGoal(true);
            if(this.currentMissionHalf == UniverseType_E.HELL)
                UIManager.singleton.ShowGhostRecieved();
            else
                UIManager.singleton.ShowWotlRecieved();
            
            UIManager.singleton.SetNewMessage(this._currentMessage, true);
            onCutsceneFinishedCallback(nextGoal);
        }

        private void Update()
        {
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
        }
    }
}