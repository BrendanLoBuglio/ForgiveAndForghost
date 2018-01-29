using System.Collections.Generic;
using UnityEngine;

namespace _Scriptz.TheGamePartOfTheGame
{
    public class GameplayManager : MonoBehaviour
    {
        /*# Scene References #*/
        public List<PortalNode> hellPortals = new List<PortalNode>();
        public List<PortalNode> wotlPortals = new List<PortalNode>();
        
        /*# Config #*/
        public Mission[] missions;
        
        /*# State #*/
        public int missionIndex { get; private set; }
        public UniverseType_E currentMissionHalf { get; private set; }
        private List<PortalNode> _usedNodes = new List<PortalNode>();

        private string _currentMessage;
        

        public static GameplayManager singleton => _singleton ?? (_singleton = FindObjectOfType<GameplayManager>());
        private static GameplayManager _singleton;
        

        public void initializeGame()
        {
            var startingPortal = this.wotlPortals[Random.Range(0, this.wotlPortals.Count)];
            
            this._usedNodes.Add(startingPortal);
            
            PlayerGhost.s.setStartNodes(startingPortal, startingPortal.GetFirstRail().endWhichIsNot(startingPortal));
            PlayerGhost.s.Initialize();

            this.missionIndex = 0;
            
            
        }
    }
}