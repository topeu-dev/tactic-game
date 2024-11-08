using UnityEngine;

namespace Effects
{
    [CreateAssetMenu(menuName = "Buffs")]
    public class Buff : ScriptableObject
    {
        public string buffName;
        public Sprite icon;
        public float duration;

        public float value;

        public enum BuffType
        {
            IncreaseHealth,
            IncreaseAttack
        }

        public BuffType buffType;
    }
}