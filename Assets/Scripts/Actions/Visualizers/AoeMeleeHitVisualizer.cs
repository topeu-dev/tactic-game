using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Actions.Visualizers
{
    public class AoeMeleeHitVisualizer : MonoBehaviour, ActionVisualizer
    {
        private LineRenderer lineRenderer;
        public int segments = 50;
        private float _aoeRadius;
        List<DecalProjector> enabledDecalProjectors = new();


        private void Awake()
        {
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.loop = true;
            lineRenderer.enabled = false;
        }

        public void EnableVisualizerFor(GameObject activeChar, ActionInstance actionInstanceParam)
        {
            _aoeRadius = actionInstanceParam.CurrentAoe;
            DrawCircle(activeChar.transform.position, _aoeRadius);
            lineRenderer.enabled = true;
            Vizualize();
        }

        public void DisableVisualizer()
        {
            foreach (var enabledDecalProjector in enabledDecalProjectors)
            {
                enabledDecalProjector.enabled = false;
            }

            lineRenderer.enabled = false;
        }


        private void Vizualize()
        {
            var hitTargets = Physics.OverlapSphere(transform.position, _aoeRadius);
            
            foreach (Collider target in hitTargets)
            {
                Debug.Log("Sphere collides with -> " + target.gameObject.name);
                if (target.gameObject.CompareTag("Enemy"))
                {
                    var decalProjector = target.GetComponentInChildren<DecalProjector>();
                    if (decalProjector != null)
                    {
                        //todo make AimedDecalController to animate + enable
                        decalProjector.enabled = true;
                        enabledDecalProjectors.Add(decalProjector);
                    }
                }
            }
        }


        void DrawCircle(Vector3 center, float radius)
        {
            lineRenderer.transform.position = center;
            // Create an array of positions for the circle points
            lineRenderer.positionCount = segments + 1;
            Vector3[] positions = new Vector3[segments + 1];

            for (int i = 0; i < segments + 1; i++)
            {
                float angle = (float)i / segments * Mathf.PI * 2;
                float x = center.x + Mathf.Cos(angle) * _aoeRadius;
                float z = center.z + Mathf.Sin(angle) * _aoeRadius;

                positions[i] = new Vector3(x, 0, z);
            }

            // Assign points to the LineRenderer
            lineRenderer.SetPositions(positions);
        }
    }
}