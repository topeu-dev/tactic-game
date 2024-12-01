using UnityEngine;

namespace Effects
{
    public enum EffectType
    {
        PeriodicalDamage, // const damage
        DamageModifer, // apply to all damageable actions
        MoveModifer, // apply to all moveable actions
        Stun, // specific
        Invisibility, // specific
        Block // specific
    }

    public enum WhenToApply
    {
        OnTurnStart
    }

    public enum StackBehaviour
    {
        Refresh
    }

    [CreateAssetMenu(menuName = "New effect")]
    public class EffectDescription : ScriptableObject
    {
        public Sprite icon;
        public string effectName;
        public string description;

        public EffectType effectType;

        public WhenToApply whenToApply;
        
        public StackBehaviour stackBehaviour;
        public AudioClip audioClip;
        // public List<string> possibleObjectsToApply;
    }
}