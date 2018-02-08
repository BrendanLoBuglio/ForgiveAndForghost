using UnityEngine;
using _Scriptz.NodesAndRails;

public enum UniverseType_E {HELL, WOTL}

public class PortalNode : Node
{
    public Color nodeColor;
    public PortalTower tower;
    public UniverseType_E universeType;
	public PortalCutsceneManager cutsceneManager;
	public Node portalConnectionNode;

    private void Awake()
    {
        this.tower.initialize(this.nodeColor);
    }

    public void setIsGoal(bool isGoal)
    {
        this.tower.setIsTargetTower(isGoal);
    }
}