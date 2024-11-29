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
        OnDamageTaken // specific
    }

    [CreateAssetMenu(menuName = "New effect")]
    public class EffectDescription : ScriptableObject
    {
        public Sprite icon;
        public string effectName;
        public string description;

        public EffectType effectType;
        // public List<string> possibleObjectsToApply;
    }
}