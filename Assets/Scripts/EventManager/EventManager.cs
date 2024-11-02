using UnityEngine;
using UnityEngine.Events;

public static class EventManager
{
    public static readonly SelectableObjectEvents SelectableObject = new SelectableObjectEvents();

    public class SelectableObjectEvents
    {
        public UnityAction<Component, GameObject> OnObjectSelectedEvent;
    }
}