using UnityEngine;
using UnityEngine.AI;
using Utility;

namespace Actions.Visualizers
{
    public class MoveVisualizer : MonoBehaviour, ActionVisualizer
    {
        [SerializeField]
        private LineRenderer firstLineRenderer;

        [SerializeField]
        private LineRenderer secondLineRenderer;

        private NavMeshAgent _navMeshAgentRef;
        private bool _enabled;
        private NavMeshPath _path;
        private ActionInstance actionInstance;

        private void Awake()
        {
            _path = new NavMeshPath();
        }


        public void EnableVisualizerFor(GameObject activeChar, ActionInstance actionInstanceParam)
        {
            _navMeshAgentRef = activeChar.gameObject.GetComponent<NavMeshAgent>();
            actionInstance = actionInstanceParam;
            firstLineRenderer.enabled = true;
            secondLineRenderer.enabled = true;

            _enabled = true;
        }

        public void DisableVisualizer()
        {
            firstLineRenderer.enabled = false;
            secondLineRenderer.enabled = false;
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
                firstLineRenderer.positionCount = 0;
                secondLineRenderer.positionCount = 0;
            }
        }

        void DrawPath()
        {
            if (_path.corners.Length < 1)
                return;

            var possiblePositions = NavMeshHelper.GetPathCornersWithinDistance(_path, actionInstance.CurrentDistance);
            Vector3[] impossiblePositions =
                NavMeshHelper.GetPathCornersBeyondDistance(_path, actionInstance.CurrentDistance);

            firstLineRenderer.positionCount = possiblePositions.Length;
            firstLineRenderer.SetPositions(possiblePositions);

            secondLineRenderer.positionCount = impossiblePositions.Length;
            secondLineRenderer.SetPositions(impossiblePositions);
        }
    }
}