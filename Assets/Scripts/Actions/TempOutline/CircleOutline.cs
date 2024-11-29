using System;
using System.Collections;
using UnityEngine;

namespace Actions.TempOutline
{
    public class CircleOutline : MonoBehaviour
    {
        private LineRenderer lineRenderer;
        private int segments = 50;
        
        private bool inProgress = false;

        private void Awake()
        {
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.enabled = false;
            DrawCircle(transform.position);
        }

        public void OutlineCircle()
        {
            if (inProgress)
                return;
            inProgress = true;
            StartCoroutine(showForHalfSec());
        }

        private IEnumerator showForHalfSec()
        {
            lineRenderer.enabled = true;
            // wait
            yield return new WaitForSeconds(0.8f);

            lineRenderer.enabled = false;
            inProgress = false;
        }

        void DrawCircle(Vector3 center)
        {
            lineRenderer.transform.position = center;
            // Create an array of positions for the circle points
            lineRenderer.positionCount = segments + 1;
            Vector3[] positions = new Vector3[segments + 1];

            for (int i = 0; i < segments + 1; i++)
            {
                float angle = (float)i / segments * Mathf.PI * 2;
                float x = center.x + Mathf.Cos(angle) * 0.5f;
                float z = center.z + Mathf.Sin(angle) * 0.5f;

                positions[i] = new Vector3(x, 0, z);
            }

            // Assign points to the LineRenderer
            lineRenderer.SetPositions(positions);
        }
    }
}