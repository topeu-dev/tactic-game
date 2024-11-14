using System.Collections.Generic;
using UnityEngine;

namespace Actions
{
    public enum ActionVisualizerType
    {
        None,
        MeleeHit,
        Move,
        RangedHit,
        AoeHit
    }

    [CreateAssetMenu(menuName = "Actions/New Char Action")]
    public class ActionDescription : ScriptableObject
    {
        public string actionName;
        public string description;
        public Sprite icon;

        //Tags
        public List<string> possibleObjectsToApply;
        public ActionVisualizerType actionVisualizerType;
    }
}