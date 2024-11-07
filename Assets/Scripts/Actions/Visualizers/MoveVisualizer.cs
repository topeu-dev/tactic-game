using System;
using UnityEngine;
using UnityEngine.AI;

namespace Actions.Visualizers
{
    [RequireComponent(typeof(LineRenderer))]
    public class MoveVisualizer : MonoBehaviour
    {
        private LineRenderer lineRenderer;

        [SerializeField]
        private NavMeshAgent _navMeshAgentRef;

        [SerializeField]
        private bool _enabled;

        private NavMeshPath path;

        private void Awake()
        {
            path = new NavMeshPath();
            lineRenderer = GetComponent<LineRenderer>();
        }


        public void EnableVisualizerFor(GameObject activeChar)
        {
            Debug.Log("GOGOGOG!123");
            _navMeshAgentRef = activeChar.gameObject.GetComponent<NavMeshAgent>();
            _enabled = true;
        }

        private void Update()
        {
            if (!_enabled)
                return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 200f, 1 << LayerMask.NameToLayer("BattleFieldLayer")))
            {
                NavMesh.CalculatePath(_navMeshAgentRef.transform.position, hit.point, NavMesh.AllAreas, path);
                // Проверяем, есть ли путь
                if (path.status == NavMeshPathStatus.PathComplete)
                {
                    // Отображаем путь
                    DrawPath();
                }
            }
            else
            {
                lineRenderer.positionCount = 0;
            }
        }

        void DrawPath()
        {
            if (path.corners.Length < 1)
                return;
            if (GetPathDistance(path) < 10)
            {
                lineRenderer.startColor = Color.green;
            }
            else
            {
                lineRenderer.startColor = Color.red;
            }

            lineRenderer.positionCount = path.corners.Length;
            
            lineRenderer.SetPositions(path.corners);
        }
        
        float GetPathDistance(NavMeshPath path)
        {
            float distance = 0.0f;

            // Iterate through the corners of the path
            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                distance += Vector3.Distance(path.corners[i], path.corners[i + 1]);
            }

            return distance;
        }
    }
}