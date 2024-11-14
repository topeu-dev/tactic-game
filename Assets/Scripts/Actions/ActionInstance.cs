using System;
using UnityEngine;

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

        public int CurrentCooldown { get; private set; }
        public float CurrentDistance { get; private set; }
        public float CurrentAoe { get; private set; }
        // public List<string> targetEffect;

        ActionInstance(ActionDescription actionDescription, int cooldown, float distance, float aoe)
        {
            this.actionDescription = actionDescription;
            this.cooldown = cooldown;
            this.distance = distance;
            this.aoe = aoe;

            CurrentCooldown = cooldown;
            CurrentDistance = distance;
            CurrentAoe = aoe;
        }

        public void Init()
        {
            CurrentCooldown = 0;
            CurrentDistance = distance;
            CurrentAoe = aoe;
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
    }
}