using System;
using System.Collections.Generic;
using Effects;
using Enemy;
using UnityEngine;
using Utility;

namespace Actions.TaskExecutor
{
    public class MeleeAoeExecutor : MonoBehaviour
    {
        private Animator _animatorRef;
        public AudioSource audioRef;
        private EffectController _effectControllerRef;

        private void Awake()
        {
            _effectControllerRef = GetComponent<EffectController>();
            _animatorRef = GetComponent<Animator>();
        }


        public void AoeHit(ActionInstance actionInstance, ActionContext actionContext, Action callback)
        {
            var hitTargets = Physics.OverlapSphere(transform.position, actionInstance.CurrentAoe);

            audioRef.PlayOneShot(actionInstance.actionDescription.audioClip);
            _animatorRef.SetTrigger(AnimTriggers.MeleeAoeHitTrigger);

            var lookingFor = defineLookingFor();
            var filteredHitTargets = new List<Collider>(hitTargets)
                .FindAll(coll => coll.gameObject.CompareTag(lookingFor));

            foreach (Collider hitTarget in filteredHitTargets)
            {
                if (hitTarget.gameObject.CompareTag("Enemy") || hitTarget.gameObject.CompareTag("PlayableChar"))
                {
                    var damage = actionInstance.CalcDamage();
                    Debug.Log(hitTarget.gameObject.name + " got " + damage + " damage");
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

                    EventManager.DamageRelatedEvent.OnDamageTaken(this, hitTarget.gameObject, damage, actionInstance.actionDescription.impactDelay);
                }
            }

            actionInstance.Used();
            EventManager.ActionUseEvent.OnActionUsed(this, null);

            callback?.Invoke();
        }

        private string defineLookingFor()
        {
            return gameObject.CompareTag("PlayableChar") ? "Enemy" : "PlayableChar";
        }
    }
}