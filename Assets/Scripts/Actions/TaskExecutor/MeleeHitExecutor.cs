using System;
using Actions;
using UnityEngine;

namespace TaskApproachTest
{
    public class MeleeHitExecutor : MonoBehaviour
    {
        private Animator _animatorRef;

        private void Awake()
        {
            _animatorRef = GetComponent<Animator>();
        }


        public void Hit(ActionContext actionContext, Action callback)
        {
            var hitTarget = actionContext.ClickedObject;
            var activeChar = actionContext.CurrentActiveChar;
            
            activeChar.transform.LookAt(hitTarget.transform);
            _animatorRef.SetTrigger("MeleeHitTrigger");
            callback?.Invoke();
            // activeChar calcDamage
            // activeChar -> playHitAnim;
            // get component Damageable? + takeDamage()
            // calc damage
            // if damage > remaining hp
            // play dead anim
            // else play damageTakenAnim
        }
    }
}