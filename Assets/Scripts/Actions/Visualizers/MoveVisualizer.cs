using System;
using UnityEngine;
using UnityEngine.AI;

namespace Actions.Visualizers
{
    [RequireComponent(typeof(LineRenderer))]
    public class MoveVisualizer : MonoBehaviour, ActionVisualizer
    {
        private LineRenderer _lineRenderer;
        private NavMeshAgent _navMeshAgentRef;
        private bool _enabled;
        private NavMeshPath _path;
        private ActionInstance actionInstance;

        private void Awake()
        {
            _path = new NavMeshPath();
            _lineRenderer = GetComponent<LineRenderer>();
        }


        public void EnableVisualizerFor(GameObject activeChar, ActionInstance actionInstance)
        {
            _navMeshAgentRef = activeChar.gameObject.GetComponent<NavMeshAgent>();
            this.actionInstance = actionInstance;
            _lineRenderer.enabled = true;
            _enabled = true;
        }

        public void DisableVisualizer()
        {
            _lineRenderer.enabled = false;
            _enabled = false;
        }

        private void Update()
        {
            if (!_enabled)
                return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 200f, 1 << LayerMask.NameToLayer("BattleFieldLayer")))
            {
                NavMesh.CalculatePath(_navMeshAgentRef.transform.position, hit.point, NavMesh.AllAreas, _path);
                // Проверяем, есть ли путь
                if (_path.status == NavMeshPathStatus.PathComplete)
                {
                    // Отображаем путь
                    DrawPath();
                }
            }
            else
            {
                _lineRenderer.positionCount = 0;
            }
        }

        void DrawPath()
        {
            if (_path.corners.Length < 1)
                return;
            if (GetPathDistance(_path) < actionInstance.CurrentDistance)
            {
                _lineRenderer.startColor = Color.green;
                _lineRenderer.endColor = Color.green;
            }
            else
            {
                _lineRenderer.startColor = Color.red;
                _lineRenderer.endColor = Color.red;
            }

            _lineRenderer.positionCount = _path.corners.Length;

            
            _lineRenderer.SetPositions(_path.corners);
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