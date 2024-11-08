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
    public class ActionSo : ScriptableObject
    {
        public string actionName;
        public string description;
        public Sprite icon;

        //Object tags used
        public List<string> possibleObjectsToApply;

        //TEMP change to Interface
        public ActionVisualizerType actionVisualizerType;
        // add-self apply?
    }
}