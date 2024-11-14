using System;
using UnityEngine;
using Utility;

namespace Actions.TaskExecutor
{
    public class MeleeHitExecutor : MonoBehaviour
    {
        
        private Animator _animatorRef;

        private void Awake()
        {
            _animatorRef = GetComponent<Animator>();
        }


        public void Hit(ActionInstance actionInstance, ActionContext actionContext, Action callback)
        {
            var hitTarget = actionContext.ClickedObject;
            var activeChar = actionContext.CurrentActiveChar;

            var distanceToHitTarget = Vector3.Distance(hitTarget.transform.position, activeChar.transform.position);
            //TODO: also check if there is no wall between
            if (distanceToHitTarget > actionInstance.CurrentDistance)
            {
                CantHit(callback);
                return;
            }
            
            activeChar.transform.LookAt(hitTarget.transform);
            _animatorRef.SetTrigger(AnimTriggers.MeleeHitTrigger);
            actionInstance.Used();
            EventManager.ActionUseEvent.OnActionUsed(this, null);
            callback?.Invoke();
            // activeChar calcDamage
            // activeChar -> playHitAnim;
            // get component Damageable? + takeDamage()
            // calc damage
            // if damage > remaining hp
            // play dead anim
            // else play damageTakenAnim
        }


        private void CantHit(Action callback)
        {
            EventManager.NotificationEvent.OnErrorNotificationEvent(this, "Can't reach target");
            callback?.Invoke();
        }
    }
}