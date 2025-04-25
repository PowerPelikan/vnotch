using System.Collections.Generic;
using UnityEngine;

namespace MEEP.InteractionSystem
{
    public class InteractableVisuals : MonoBehaviour
    {

        [SerializeReference]
        private List<Renderer> targetRenderers;

        private MaterialPropertyBlock cachedBlock;

        private bool hasReceivedHit = false;

        [SerializeField]
        [LayerDropdown]
        private int onInactiveLayer;

        [SerializeField]
        [LayerDropdown]
        private int onActiveLayer; 

        private void Start()
        {
            hasReceivedHit = false;

            if (targetRenderers == null)
                targetRenderers = new();

        }

        private void LateUpdate()
        {
            if (hasReceivedHit)
            {
                SetMaterialProperties(1);
            }
            else
            {
                SetMaterialProperties(0);
                this.enabled = false;
            }

            this.hasReceivedHit = false;
        }


        public void ReceiveHit()
        {
            enabled = true;
            hasReceivedHit = true;
        }


        /// <summary>
        /// Applies selection effect on target Renderer's material.
        /// Only works if the Material within has a property named "Selected".
        /// </summary>
        private void SetMaterialProperties(float selected)
        {
            if (cachedBlock == null)
                cachedBlock = new MaterialPropertyBlock();

            cachedBlock.SetFloat("_Selected", selected);

            foreach (var renderer in targetRenderers)
            {
                if (renderer != null)
                    renderer.gameObject.layer = (selected > 0) ? onActiveLayer : onInactiveLayer;
            }

            //Debug.LogFormat("Set Material Properties: {0}", selected);
        }

        public void AddRenderer(Renderer r)
        {
            targetRenderers.Add(r);
        }

        public void RemoveRenderer(Renderer r)
        {
            r.gameObject.layer = onInactiveLayer;
            targetRenderers.Remove(r);
        }

    }
}