using System;
using UnityEngine;
using Utility;

namespace Actions.TaskExecutor
{
    public class MeleeAoeExecutor : MonoBehaviour
    {
        private Animator _animatorRef;

        private void Awake()
        {
            _animatorRef = GetComponent<Animator>();
        }


        public void AoeHit(ActionInstance actionInstance, ActionContext actionContext, Action callback)
        {
            var hitTargets = Physics.OverlapSphere(transform.position, actionInstance.CurrentAoe);

            _animatorRef.SetTrigger(AnimTriggers.MeleeAoeHitTrigger);
            foreach (Collider hitTarget in hitTargets)
            {
                if (hitTarget.gameObject.CompareTag("Enemy"))
                {
                    var damage = actionInstance.CalcDamage();
                    Debug.Log(hitTarget.gameObject.name + " got " + damage + " damage");
                    EventManager.DamageRelatedEvent.OnDamageTaken(this, hitTarget.gameObject, damage);
                }
            }

            actionInstance.Used();
            EventManager.ActionUseEvent.OnActionUsed(this, null);

            callback?.Invoke();
        }
    }
}