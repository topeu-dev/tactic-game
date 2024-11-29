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

        private IEnumerator MoveToPosition(NavMeshPath path, ActionInstance actionInstance, float speed,
            Action onComplete)
        {
            var possibleCorners = NavMeshHelper.GetPathCornersWithinDistance(path, actionInstance.CurrentDistance);
            
            float distanceToMove = GetPathDistance(possibleCorners);
            _animatorRef.SetTrigger(AnimTriggers.Move);
            _navMeshAgentRef.SetDestination(possibleCorners[^1]);
            // _navMeshAgentRef.SetPath(path);

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

            _animatorRef.SetTrigger(AnimTriggers.Idle);
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

        NavMeshPath CalcPath(Vector3 targetPosition)
        {
            NavMeshPath path = new NavMeshPath();
            _navMeshAgentRef.CalculatePath(targetPosition, path);
            return path;
        }

        float GetPathDistance(Vector3[] pathCorners)
        {
            float totalDistance = 0.0f;

            if (pathCorners.Length < 2)
                return totalDistance;

            for (int i = 0; i < pathCorners.Length - 1; i++)
            {
                totalDistance += Vector3.Distance(pathCorners[i], pathCorners[i + 1]);
            }

            return totalDistance;
        }
    }
}