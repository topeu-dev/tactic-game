using System;
using System.Collections;
using Actions;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using Utility;

namespace TaskApproachTest
{
    public class CharMover : MonoBehaviour
    {

        public float speed;

        private NavMeshSurface _navMeshSurfaceRef;
        private NavMeshAgent _navMeshAgentRef;
        private Animator _animatorRef;


        private void Awake()
        {
            _navMeshSurfaceRef = FindFirstObjectByType<NavMeshSurface>();
            _navMeshAgentRef = gameObject.GetComponent<NavMeshAgent>();
            _animatorRef = gameObject.GetComponent<Animator>();
        }

        public void Move(ActionInstance actionInstance, ActionContext actionContext, Action callback)
        {
            if (!actionContext.BattleFieldClickedPos.HasValue)
            {
                CantMove(callback);
                return;
            }

            NavMeshPath path = CalcPath(actionContext.BattleFieldClickedPos.Value);
            if (!AgentCanReachDestination(path))
            {
                CantMove(callback);
                return;
            }

            if (GetPathDistance(path) > actionInstance.CurrentDistance)
            {
                CantMove(callback);
                return;
            }
            
            StartCoroutine(
                MoveToPosition(
                    path,
                    actionInstance,
                    speed,
                    callback
                )
            );
            
            
        }

        private bool AgentCanReachDestination(NavMeshPath path)
        {
            return path.status == NavMeshPathStatus.PathComplete;
        }

        private void CantMove(Action callback)
        {
            EventManager.NotificationEvent.OnErrorNotificationEvent(this, "Cant move there");
            Debug.LogWarning("BattleFieldClickedPos is null or can't be reached.");
            callback?.Invoke();
        }

        public IEnumerator MoveToPosition(NavMeshPath path, ActionInstance actionInstance, float speed, Action onComplete)
        {
            float distanceToMove = GetPathDistance(path);
            _animatorRef.SetTrigger(AnimTriggers.MoveTrigger);
            _navMeshAgentRef.SetPath(path);

            while (!AgentReachedDestination())
            {
                yield return null;
            }


            actionInstance.DecreaseDistance(distanceToMove);
            if (actionInstance.CurrentDistance < 0.5f)
            {
                actionInstance.Used();
                EventManager.ActionUseEvent.OnActionUsed(this, null);
            }
            _animatorRef.SetTrigger(AnimTriggers.IdleTrigger);
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
                    if (!_navMeshAgentRef.hasPath || _navMeshAgentRef.velocity.sqrMagnitude == 0f)
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

        NavMeshPath CalcPath(Vector3 targetPosition)
        {
            NavMeshPath path = new NavMeshPath();
            _navMeshAgentRef.CalculatePath(targetPosition, path);
            return path;
        }

        float GetPathDistance(NavMeshPath path)
        {
            float totalDistance = 0.0f;

            if (path.corners.Length < 2)
                return totalDistance;

            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                totalDistance += Vector3.Distance(path.corners[i], path.corners[i + 1]);
            }

            return totalDistance;
        }
    }
}