using System.Collections.Generic;
using UnityEngine;

namespace Actions
{
    [CreateAssetMenu(menuName = "Actions/New Char Action")]
    public class ActionSo : ScriptableObject
    {
        public string actionName;
        public string description;
        public Sprite icon;
        
        //Object tags used
        public List<string> possibleObjectsToApply;
        
        // add-self apply?
    }
}