using System;
using System.Collections.Generic;
using Actions;
using Global;
using UnityEngine;

namespace Effects
{
    public class EffectController : MonoBehaviour
    {
        private BattleContext _battleContext;
        private VfxController _vfxController;


        private void Awake()
        {
            _vfxController = GetComponent<VfxController>();
            _battleContext = FindFirstObjectByType<BattleContext>();
        }


        public void EnableInitialVfxs(List<EffectInstance> effects)
        {
            effects.ForEach(effect => { _vfxController.EnableVfxFor(effect); });
        }

        public void HandleEffectOnTurnStart(EffectInstance effectInstance, List<ActionInstance> actionsInstances)
        {
            if (effectInstance.effectDescription.effectType == EffectType.PeriodicalDamage)
            {
                EventManager.DamageRelatedEvent.OnDamageTaken?.Invoke(null, gameObject,
                    effectInstance.periodicalDamage, 0.2f);
                return;
            }

            if (effectInstance.effectDescription.effectType == EffectType.MoveModifer)
            {
                var moveAction = actionsInstances.Find(action => action.actionDescription.actionName == "Move");
                if (moveAction != null)
                {
                    moveAction.CurrentDistance *= effectInstance.movementModifer;
                }
                else
                {
                    Debug.LogWarning("No move action found");
                }

                return;
            }

            if (effectInstance.effectDescription.effectType == EffectType.Invisibility)
            {
                Debug.Log("MADE INVISIBLE " + gameObject.name);
                _battleContext.MakeInvisible(gameObject);
                return;
            }

            if (effectInstance.effectDescription.effectType == EffectType.DamageModifer)
            {
                var actionsWithDamage
                    = actionsInstances.FindAll(action => action.CurrentMinDamage != 0 || action.CurrentMaxDamage != 0);

                actionsWithDamage.ForEach(action =>
                {
                    action.CurrentMinDamage =
                        (int)Math.Ceiling(action.CurrentMinDamage * effectInstance.damageModifer);
                    action.CurrentMaxDamage *=
                        (int)Math.Ceiling(action.CurrentMaxDamage * effectInstance.damageModifer);
                });
                return;
            }
        }

        public void EffectEnded(List<EffectInstance> effectsInstances, EffectInstance effect)
        {
            if (effect.effectDescription.effectType == EffectType.Invisibility)
            {
                _battleContext.MakeVisible(gameObject);
            }

            _vfxController.DisableVfxFor(effect);
            effectsInstances.Remove(effect);
        }


        public void ApplyNewEffects(
            VfxController targetVfxController,
            List<EffectInstance> targetCurrentEffects,
            List<EffectInstance> effectsToApply,
            AudioSource audioRef)
        {
            effectsToApply.ForEach(
                effectToApply =>
                {
                    var existedEffect = targetCurrentEffects.Find(ce =>
                        ce.effectDescription.effectName.Equals(effectToApply.effectDescription.effectName));
                    if (existedEffect != null)
                    {
                        // stack
                        switch (effectToApply.effectDescription.stackBehaviour)
                        {
                            case StackBehaviour.Refresh:
                            {
                                var indexOf = targetCurrentEffects.IndexOf(existedEffect);
                                targetCurrentEffects.Remove(existedEffect);
                                targetCurrentEffects.Insert(indexOf, effectToApply.Clone());
                                break;
                            }

                            default:
                            {
                                Debug.LogWarning("Mismatched stack behaviour " +
                                                 effectToApply.effectDescription.effectName);
                                break;
                            }
                        }
                    }
                    else
                    {
                        // add new
                        targetCurrentEffects.Add(effectToApply.Clone());
                        targetVfxController.EnableVfxFor(effectToApply);
                        audioRef.PlayOneShot(effectToApply.effectDescription.audioClip);
                    }
                }
            );
        }
    }
}