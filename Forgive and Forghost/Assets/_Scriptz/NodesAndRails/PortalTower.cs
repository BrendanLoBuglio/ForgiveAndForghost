using System.Text;
using UnityEngine;

namespace _Scriptz.NodesAndRails
{
    public class PortalTower : MonoBehaviour
    {
        public GameObject towerHighlightBeam;
        public Renderer[] decoMeshRenderers;

        private Color _portalColor;
		public Color portalColor { get { return _portalColor; } }

        public void initialize(Color portalColor)
        {
            this._portalColor = portalColor;
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

        private void Update()
        {
            var dist = Vector3.Distance(this.transform.position, PlayerGhost.s.transform.position);

            if (dist < 20f) {
                var beam = this.towerHighlightBeam.GetComponent<Renderer>();
                var t = Mathf.Clamp01(dist / 20f);
                beam.material.SetColor("_Color", new Color(this._portalColor.r, this._portalColor.g, this._portalColor.b, t));
                beam.material.SetColor("_EmissionColor", new Color(t * this._portalColor.r, t * this._portalColor.g, t * this._portalColor.b, t));
            }
            
        }
    }
}