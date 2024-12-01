using System;
using System.Collections.Generic;
using Effects;
using Enemy;
using UnityEngine;

namespace Actions.TaskExecutor
{
    public class HitScanExecutor : MonoBehaviour
    {
        private Animator _animatorRef;
        private EffectController _effectControllerRef;
        public AudioSource audioRef;

        private void Awake()
        {
            _effectControllerRef = GetComponent<EffectController>();
            _animatorRef = GetComponent<Animator>();
        }


        public void HitScan(ActionInstance actionInstance, ActionContext actionContext, Action callback)
        {
            var hitTarget = actionContext.ClickedObject;
            var activeChar = actionContext.CurrentActiveChar;

            var distanceToHitTarget = Vector3.Distance(hitTarget.transform.position, activeChar.transform.position);

            if (distanceToHitTarget > actionInstance.CurrentDistance)
            {
                CantHit(callback);
                return;
            }

            activeChar.transform.LookAt(hitTarget.transform);
            audioRef.PlayOneShot(actionInstance.actionDescription.audioClip);
            _animatorRef.SetTrigger(actionInstance.actionDescription.animExecutorTriggerName);
            actionInstance.Used();

            //todo: dont trigger if its a bot
            EventManager.ActionUseEvent.OnActionUsed(this, null);

            if (actionInstance.actionDescription.effectsToApply is { } effectsToApply)
            {
                List<EffectInstance> currentTargetEffects = new List<EffectInstance>();
                if (hitTarget.GetComponent<GenericEnemy>() is { } genericEnemy)
                {
                    currentTargetEffects = genericEnemy.effectsInstances;
                }
                else if (hitTarget.GetComponent<GenericChar>() is { } genericChar)
                {
                    currentTargetEffects = genericChar.effectsInstances;
                }

                _effectControllerRef.ApplyNewEffects(
                    hitTarget.GetComponent<VfxController>(),
                    currentTargetEffects,
                    effectsToApply,
                    audioRef
                );
            }

            EventManager.DamageRelatedEvent.OnDamageTaken(this, 
                hitTarget,
                actionInstance.CalcDamage(),
                actionInstance.actionDescription.impactDelay
                );
            callback?.Invoke();
        }


        private void CantHit(Action callback)
        {
            EventManager.NotificationEvent.OnErrorNotificationEvent.Invoke(this, "Can't reach target");
            callback?.Invoke();
        }
    }
}