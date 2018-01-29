using System.Text;
using UnityEngine;

namespace _Scriptz.NodesAndRails
{
    public class PortalTower : MonoBehaviour
    {
        public GameObject towerHighlightBeam;
        public Renderer[] decoMeshRenderers;


        public void initialize(Color portalColor)
        {
            foreach (var decoMeshRenderer in this.decoMeshRenderers) {
                decoMeshRenderer.material.SetColor("_Color", portalColor);
            }
            var highlightRenderer = this.towerHighlightBeam.GetComponent<Renderer>(); 
            highlightRenderer.material.SetColor("_Color", portalColor);
            highlightRenderer.material.SetColor("_EmissionColor", portalColor);
        }
        
        public void setIsTargetTower(bool isTarget)
        {
            this.towerHighlightBeam.SetActive(isTarget);
        }
    }
}