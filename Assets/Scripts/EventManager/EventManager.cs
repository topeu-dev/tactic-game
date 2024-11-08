using UnityEngine;
using UnityEngine.Events;

public static class EventManager
{
    public static readonly SelectableObjectEvents SelectableObject = new SelectableObjectEvents();
    public static readonly TurnEvents TurnEvent = new TurnEvents();
    public static readonly CameraEvents CameraEvent = new CameraEvents();

    public class SelectableObjectEvents
    {
        public UnityAction<Component, GameObject> OnObjectSelectedEvent;
    }
    
    public class CameraEvents
    {
        public UnityAction<Component, GameObject> OnPlayableCharacterFocusEvent;
    }
    
    public class TurnEvents
    {
        /// <summary>
        /// Component -> this, GameObject -> next char
        /// </summary>
        public UnityAction<Component, GameObject> OnNextTurnEvent;
    }
}