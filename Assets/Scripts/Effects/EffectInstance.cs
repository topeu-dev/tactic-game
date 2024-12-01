using System;

namespace Effects
{
    [Serializable]
    public class EffectInstance
    {
        public EffectDescription effectDescription;

        //делаем помойку тут, нет времени делать красиво(
        public int duration;
        public int periodicalDamage;
        public float movementModifer;
        public float damageModifer;
        public int currentDuration;

        public EffectInstance(
            EffectDescription effectDescription,
            int duration,
            int periodicalDamage,
            float movementModifer,
            float damageModifer)
        {
            this.effectDescription = effectDescription;
            currentDuration = duration;
            this.duration = duration;
            this.periodicalDamage = periodicalDamage;
            this.movementModifer = movementModifer;
            this.damageModifer = damageModifer;
        }

        public void DecreaseDuration()
        {
            currentDuration--;
        }

        public EffectInstance Clone()
        {
            return (EffectInstance)this.MemberwiseClone();
        }
    }
}