using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Actions
{
    [Serializable]
    public class ActionInstance
    {
        public ActionDescription actionDescription;

        [SerializeField]
        private int cooldown;

        [SerializeField]
        private float distance;

        [SerializeField]
        private float aoe;

        [SerializeField]
        private int minDmg;

        [SerializeField]
        private int maxDmg;

        public int CurrentCooldown { get; private set; }
        public float CurrentDistance { get; private set; }
        public float CurrentAoe { get; private set; }

        public int CurrentMinDamage { get; private set; }

        public int CurrentMaxDamage { get; private set; }

        ActionInstance(ActionDescription actionDescription, int cooldown, float distance, float aoe, int minDamage,
            int maxDamage)
        {
            this.actionDescription = actionDescription;
            this.cooldown = cooldown;
            this.distance = distance;
            this.aoe = aoe;
            this.minDmg = minDamage;
            this.maxDmg = maxDamage;

            CurrentCooldown = cooldown;
            CurrentDistance = distance;
            CurrentAoe = aoe;
            CurrentMinDamage = minDamage;
            CurrentMaxDamage = maxDamage;
        }

        public void Init()
        {
            CurrentCooldown = 0;
            CurrentDistance = distance;
            CurrentAoe = aoe;
            CurrentMinDamage = minDmg;
            CurrentMaxDamage = maxDmg;
        }

        public void StartOfTurn()
        {
            if (CurrentCooldown > 0)
            {
                CurrentCooldown--;
            }

            if (actionDescription.actionName == "Move" && CurrentDistance < distance)
            {
                CurrentDistance = distance;
            }
        }

        public void Used()
        {
            CurrentCooldown = cooldown;
        }

        public void DecreaseDistance(float distanceToSubtract)
        {
            if (CurrentDistance - distanceToSubtract < 0)
            {
                CurrentDistance = 0f;
                return;
            }

            CurrentDistance -= distanceToSubtract;
        }


        public int CalcDamage()
        {
            return Random.Range(minDmg, maxDmg);
        }
    }
}