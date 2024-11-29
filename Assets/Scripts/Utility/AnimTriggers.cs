using UnityEngine;

namespace Utility
{
    public class AnimTriggers
    {
        public static readonly int Move = Animator.StringToHash("MoveTrigger");
        public static readonly int Idle = Animator.StringToHash("IdleTrigger");
        public static readonly int MeleeHit = Animator.StringToHash("MeleeHitTrigger");
        public static readonly int GetHit = Animator.StringToHash("GetHitTrigger");
        public static readonly int Death = Animator.StringToHash("DeathTrigger");
        public static readonly int MeleeAoeHitTrigger = Animator.StringToHash("MeleeAoeHitTrigger");
    }
}