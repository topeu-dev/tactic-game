using System.Collections.Generic;
using Effects;
using UnityEngine;

namespace Actions
{
    public enum ActionVisualizerType
    {
        None,
        MeleeHit,
        Move,
        RaycastTargetHit,
        AoeHit,
        MeleeHitAoe,
    }

    [CreateAssetMenu(menuName = "Actions/New Char Action")]
    public class ActionDescription : ScriptableObject
    {
        public string actionName;
        public string description;
        public Sprite icon;
        public Sprite iconDescription; 

        //Tags
        public List<string> possibleObjectsToApply;
        public ActionVisualizerType actionVisualizerType;
        public List<EffectInstance> effectsToApply;
        public AudioClip audioClip;
        public string animExecutorTriggerName;
        public float impactDelay;
    }
}