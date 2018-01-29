using UnityEngine;

namespace _Scriptz.NodesAndRails
{
    public class PortalTower : MonoBehaviour
    {
        public GameObject towerHighlightBeam;
        
        public void setIsTargetTower(bool isTarget)
        {
            this.towerHighlightBeam.SetActive(isTarget);
        }
    }
}