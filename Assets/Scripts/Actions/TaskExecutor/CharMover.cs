using System;
using System.Collections;
using Actions;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

namespace TaskApproachTest
{
    public class CharMover : MonoBehaviour
    {
        public float speed;

        private NavMeshSurface _navMeshSurfaceRef;
        private NavMeshAgent _navMeshAgentRef;
        private Animator _animatorRef;

        [SerializeField]
        private float _currentSpeed;

        private bool changed = false;


        private void Awake()
        {
            _navMeshSurfaceRef = FindFirstObjectByType<NavMeshSurface>();
            _navMeshAgentRef = gameObject.GetComponent<NavMeshAgent>();
            _animatorRef = gameObject.GetComponent<Animator>();
        }

        public void Move(ActionContext actionContext, Action callback)
        {
            if (actionContext.BattleFieldClickedPos.HasValue &&
                CanAgentReach(actionContext.BattleFieldClickedPos.Value))
            {
                StartCoroutine(MoveToPosition(actionContext.BattleFieldClickedPos.Value, speed, callback));
            }
            else
            {
                callback?.Invoke();
                Debug.LogWarning("BattleFieldClickedPos is null or can't be reached.");
            }
        }

        public IEnumerator MoveToPosition(Vector3 target, float speed, Action onComplete)
        {
            _animatorRef.SetTrigger("MoveTrigger");
            _navMeshAgentRef.SetDestination(target);

            while (!AgentReachedDestination())
            {
                _currentSpeed = _navMeshAgentRef.velocity.sqrMagnitude;
                yield return null;
            }
            _animatorRef.SetTrigger("IdleTrigger");

            _navMeshSurfaceRef.BuildNavMesh();

            onComplete?.Invoke();
        }


        bool AgentReachedDestination()
        {
            // Check if the agent has reached its destination
            if (!_navMeshAgentRef.pathPending)
            {
                if (_navMeshAgentRef.remainingDistance <= _navMeshAgentRef.stoppingDistance)
                {

                    if (!_navMeshAgentRef.hasPath || _navMeshAgentRef.velocity.sqrMagnitude < 3f)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        //todo: implement later
        GameObject GetClosestEnemy(Vector3 currentPosition)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            GameObject closest = null;
            float minDistance = Mathf.Infinity;

            foreach (GameObject enemy in enemies)
            {
                float distance = Vector3.Distance(enemy.transform.position, currentPosition);
                if (distance < minDistance)
                {
                    closest = enemy;
                    minDistance = distance;
                }
            }

            return closest;
        }

        public bool CanAgentReach(Vector3 targetPosition)
        {
            // Create a NavMeshPath instance to store the calculated path
            NavMeshPath path = new NavMeshPath();

            // Calculate a path to the target position
            _navMeshAgentRef.CalculatePath(targetPosition, path);

            // Check if the path status is complete
            return path.status == NavMeshPathStatus.PathComplete;
        }
    }
}