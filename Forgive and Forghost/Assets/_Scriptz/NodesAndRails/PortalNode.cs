using UnityEngine;
using _Scriptz.NodesAndRails;

public enum UniverseType_E {HELL, WOTL}

public class PortalNode : Node
{
    public Color nodeColor;
    public PortalTower tower;
    public UniverseType_E universeType;


}