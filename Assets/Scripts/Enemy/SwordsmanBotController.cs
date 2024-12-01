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
            Debug.Log(gameObject.name + " is moving.");
            var moveActionInstance = actions.Find(
                action => action.actionDescription.actionName == "Move"
            );

            var hitActionInstance = actions.Find(
                action => action.actionDescription.actionName.Contains("Hit")
            );

            var hitActionWithStun = actions.Find(
                action => action.actionDescription.actionName.Contains("HitWithStun")
            );

            var target = FindTarget();
            if (target == null)
            {
                //improve logic here
                onComplete?.Invoke();
                Debug.Log("No target found");
                yield break;
            }

            if (canStun(target, hitActionWithStun))
            {
                actionInProgress = true;
                Debug.Log("1Swordsman use STUN HIT");
                _meleeHitExecutor.Hit(hitActionWithStun, new ActionContext(gameObject, target, null),
                    toggleActionInProgress
                );
            }

            while (actionInProgress)
            {
                yield return null;
            }
            
            if (CanHit(target, hitActionInstance))
            {
                actionInProgress = true;
                Debug.Log("1Swordsman use common HIT");
                _meleeHitExecutor.Hit(
                    hitActionInstance,
                    new ActionContext(gameObject, target, null),
                    toggleActionInProgress
                );
            }
            
            while (actionInProgress)
            {
                yield return null;
            }

            actionInProgress = true;
            var closePosition = calcClosePosition(gameObject.transform.position, target.transform.position);
            _charMover.Move(
                moveActionInstance,
                new ActionContext(gameObject, null, closePosition),
                toggleActionInProgress
            );

            while (actionInProgress)
            {
                yield return null;
            }


            // wait until moveDone -> 
            if (CanHit(target, hitActionInstance))
            {
                Debug.Log("2Swordsman use common HIT");
                actionInProgress = true;
                _meleeHitExecutor.Hit(
                    hitActionInstance,
                    new ActionContext(gameObject, target, null),
                    toggleActionInProgress
                );
            }

            while (actionInProgress)
            {
                yield return null; // Ждем до следующего кадра
            }

            if (canStun(target, hitActionWithStun))
            {
                Debug.Log("2Swordsman use STUN HIT");
                _meleeHitExecutor.Hit(hitActionWithStun, new ActionContext(gameObject, target, null),
                    () => { _inputController.UnlockInput(); }
                );
            }
            

            yield return new WaitForSeconds(1f);
            onComplete?.Invoke();
            Debug.Log(gameObject.name + " done moving.");
        }

        private bool canStun(GameObject target, ActionInstance hitActionWithStun)
        {
            if (hitActionWithStun == null) 
                return false;
            
            if (hitActionWithStun.CurrentCooldown != 0 ||
                hitActionWithStun.CurrentDistance < Vector3.Distance(transform.position, target.transform.position))
            {
                Debug.Log("Can't stun 1" + " currCD " + hitActionWithStun.CurrentCooldown + " currDist " +
                          Vector3.Distance(transform.position, target.transform.position));
                return false;
            }

            var targetChar = target.GetComponent<GenericChar>();
            if (targetChar.effectsInstances.Find(ei => ei.effectDescription.effectName == "Stun") != null)
            {
                Debug.Log("Can't stun 2");
                return false;
            }

            return true;
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