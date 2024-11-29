using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Actions;
using Actions.TaskExecutor;
using Global;
using TaskApproachTest;
using UnityEngine;

namespace Enemy
{
    public class SwordsmanBotController : MonoBehaviour, BotController
    {
        private BattleContext _battleContext;
        private InputController _inputController;

        [SerializeField]
        private float distanceThreshold;

        private CharMover _charMover;
        private MeleeHitExecutor _meleeHitExecutor;

        private bool actionInProgress = false;
        private float stopDistance = 2f; 

        private void Awake()
        {
            _battleContext = FindFirstObjectByType<BattleContext>();
            _charMover = GetComponent<CharMover>();
            _meleeHitExecutor = GetComponent<MeleeHitExecutor>();
            _inputController = FindFirstObjectByType<InputController>();
        }


        public IEnumerator MakeMove(List<ActionInstance> actions, Action onComplete)
        {
            var moveActionInstance = actions.Find(
                action => action.actionDescription.actionName == "Move"
            );

            var hitActionInstance = actions.Find(
                action => action.actionDescription.actionName == "Hit"
            );
            
            var target = FindTarget();

            if (CanHit(target, hitActionInstance))
            {
                _meleeHitExecutor.Hit(
                    hitActionInstance,
                    new ActionContext(gameObject, target, null),
                    () => { _inputController.UnlockInput(); }
                );
            }
            else
            {
                actionInProgress = true;
                
                var closePosition = calcClosePosition(gameObject.transform.position, target.transform.position);
                _charMover.Move(
                    moveActionInstance,
                    new ActionContext(gameObject, null, closePosition),
                    toggleActionInProgress
                    );

                // Ждем пока actionInProgress станет false
                while (actionInProgress)
                {
                    yield return null; // Ждем до следующего кадра
                }
                // wait until moveDone -> 
                if (CanHit(target, hitActionInstance))
                {
                    Debug.Log("ENEMY CAN HIT");
                    _meleeHitExecutor.Hit(
                        hitActionInstance,
                        new ActionContext(gameObject, target, null),
                        () => { _inputController.UnlockInput(); }
                    );
                }
            }
            onComplete?.Invoke();
            Debug.Log("ENEMY MOVE DONE");
        }

        private Vector3 calcClosePosition(Vector3 botPosition, Vector3 targetPosition)
        {
            //ignore Y
            Vector3 direction = (targetPosition - botPosition).normalized;
            Vector3 adjustedTargetPosition = targetPosition - direction * stopDistance;

            Debug.Log("Adjusted -> " + adjustedTargetPosition + " target -> " + targetPosition);
            return new Vector3(adjustedTargetPosition.x, botPosition.y, adjustedTargetPosition.z);
        }

        private bool CanHit(GameObject target, ActionInstance hitActionInstance)
        {
            Debug.Log("Distance to hit -> " + Vector3.Distance(transform.position, target.transform.position));
            return hitActionInstance.CurrentCooldown == 0 &&
                   hitActionInstance.CurrentDistance >= Vector3.Distance(transform.position, target.transform.position);
        }

        private GameObject FindTarget()
        {
            var playerCharactersInBattle = _battleContext.getOnlyPlayerCharactersInBattle();
            // Step 1: Find the closest distance
            var closestDistance = playerCharactersInBattle
                .Min(player =>
                    Vector3.Distance(transform.position, player.Key.transform.position)
                );

            // Step 2: Filter players within the threshold
            var nearTargets = playerCharactersInBattle
                .Where(player =>
                    player.Value.IsVisible &&
                    Mathf.Abs(Vector3.Distance(transform.position, player.Key.transform.position) - closestDistance)
                    <= distanceThreshold)
                .ToList();

            // Step 3: Choose the target with the lowest HP among near targets
            var target = nearTargets
                .OrderBy(player => player.Key.GetComponent<GenericChar>().CurrentHp)
                .Select(kv => kv.Key)
                .FirstOrDefault();


            var result = target ?? playerCharactersInBattle
                .OrderBy(player => Vector3.Distance(transform.position, player.Key.transform.position))
                .Select(kv => kv.Key)
                .FirstOrDefault();
            
            Debug.Log("Enemy found target -> " + result?.name);
            return result;
        }


        public void toggleActionInProgress()
        {
            actionInProgress = !actionInProgress;
            Debug.Log("ACTION IN PROGRESS toggled" + actionInProgress);
        }
    }
}