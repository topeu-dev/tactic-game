namespace Effects
{
    public class EffectInstance
    {
        public EffectDescription EffectDescription { get; set; }

        //делаем помойку тут, нет времени делать красиво(
        public int Duration { get; set; }
        public int PeriodicalDamage { get; set; }

        public float MovementModifer { get; set; }

        public float DamageModifer { get; set; }

        public int CurrentDuration;
        
        
    }
}