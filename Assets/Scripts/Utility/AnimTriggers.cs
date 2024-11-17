using UnityEngine;

namespace Utility
{
    public class AnimTriggers
    {
        public static readonly int MoveTrigger = Animator.StringToHash("MoveTrigger");
        public static readonly int IdleTrigger = Animator.StringToHash("IdleTrigger");
        public static readonly int MeleeHitTrigger = Animator.StringToHash("MeleeHitTrigger");
    }
}