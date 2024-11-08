using System;
using UnityEngine;

namespace Actions.Visualizers
{
    
    [RequireComponent(typeof(LineRenderer))]
    public class HitVisualizer : MonoBehaviour, ActionVisualizer
    {
        
        private LineRenderer lineRenderer;
        public int segments = 50; // Number of points along the circle, the higher the value, the smoother the circle
        public float radius = 5.0f; // Radius of the circle

        private bool _enabled;
        
        private void Awake()
        {
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.loop = true;
            lineRenderer.enabled = false;
        }
        
        public void EnableVisualizerFor(GameObject activeChar)
        {
            DrawCircle(activeChar.transform.position);
            lineRenderer.enabled = true;
            _enabled = true;
        }

        public void DisableVisualizer()
        {
            lineRenderer.enabled = false;
            _enabled = false;
        }


        // private void Update()
        // {
        //     throw new NotImplementedException();
        // }

        void DrawCircle(Vector3 center)
        {
            lineRenderer.transform.position = center;
            // Create an array of positions for the circle points
            lineRenderer.positionCount = segments + 1;
            Vector3[] positions = new Vector3[segments + 1];

            for (int i = 0; i < segments + 1; i++)
            {
                float angle = (float)i / segments * Mathf.PI * 2;
                float x = center.x + Mathf.Cos(angle) * radius;
                float z = center.z + Mathf.Sin(angle) * radius;

                positions[i] = new Vector3(x, 0, z);
            }

            // Assign points to the LineRenderer
            lineRenderer.SetPositions(positions);
        }
    }
}