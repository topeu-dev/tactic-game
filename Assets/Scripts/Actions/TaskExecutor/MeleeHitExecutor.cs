using System;
using System.Collections.Generic;
using Effects;
using Enemy;
using Unity.VisualScripting;
using UnityEngine;
using Utility;

namespace Actions.TaskExecutor
{
    public class MeleeHitExecutor : MonoBehaviour
    {
        private Animator _animatorRef;
        private EffectController _effectControllerRef;
        public AudioSource audioRef;

        private void Awake()
        {
            _effectControllerRef = GetComponent<EffectController>();
            _animatorRef = GetComponent<Animator>();
        }


        public void Hit(ActionInstance actionInstance, ActionContext actionContext, Action callback)
        {
            var hitTarget = actionContext.ClickedObject;
            var activeChar = actionContext.CurrentActiveChar;

            Debug.Log("ActionInstance :" + actionInstance.actionDescription.name + " activeChar: " + activeChar  + " hitTarget: " + hitTarget);
            var distanceToHitTarget = Vector3.Distance(hitTarget.transform.position, activeChar.transform.position);
            //TODO: also check if there is no wall between
            if (distanceToHitTarget > actionInstance.CurrentDistance)
            {
                CantHit(callback);
                return;
            }

            activeChar.transform.LookAt(hitTarget.transform);
            audioRef.PlayOneShot(actionInstance.actionDescription.audioClip);
            _animatorRef.SetTrigger(AnimTriggers.MeleeHit);
            actionInstance.Used();

            //todo: dont trigger if its a bot
            EventManager.ActionUseEvent.OnActionUsed(this, null);

            if (actionInstance.actionDescription.effectsToApply is { } effectsToApply)
            {
                List<EffectInstance> currentTargetEffects = new List<EffectInstance>();
                if (hitTarget.GetComponent<GenericEnemy>() is { } genericEnemy)
                {
                    Debug.Log("Took GenericEnemyEffects, ActionInstance :" + actionInstance.actionDescription.name + " activeChar: " + activeChar  + " hitTarget: " + hitTarget);
                    currentTargetEffects = genericEnemy.effectsInstances;
                }
                else if (hitTarget.GetComponent<GenericChar>() is { } genericChar)
                {
                    Debug.Log("Took GenericCharEffects, ActionInstance :" + actionInstance.actionDescription.name + " activeChar: " + activeChar  + " hitTarget: " + hitTarget);
                    currentTargetEffects = genericChar.effectsInstances;
                }

                _effectControllerRef.ApplyNewEffects(
                    hitTarget.GetComponent<VfxController>(),
                    currentTargetEffects,
                    effectsToApply,
                    audioRef
                );
            }

            EventManager.DamageRelatedEvent.OnDamageTaken(this, hitTarget, actionInstance.CalcDamage(), actionInstance.actionDescription.impactDelay);
            callback?.Invoke();
        }


        private void CantHit(Action callback)
        {
            EventManager.NotificationEvent.OnErrorNotificationEvent.Invoke(this, "Can't reach target");
            callback?.Invoke();
        }
    }
}