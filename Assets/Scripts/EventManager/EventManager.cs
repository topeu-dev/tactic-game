using UnityEngine;
using UnityEngine.Events;

public static class EventManager
{
    public static readonly SelectableObjectEvents SelectableObject = new SelectableObjectEvents();
    public static readonly ActionUseEvents ActionUseEvent = new ActionUseEvents();
    public static readonly TurnEvents TurnEvent = new TurnEvents();
    public static readonly CameraEvents CameraEvent = new CameraEvents();
    public static readonly NotificationEvents NotificationEvent = new NotificationEvents();
    public static readonly DamageRelatedEvents DamageRelatedEvent = new DamageRelatedEvents();

    public class SelectableObjectEvents
    {
        public UnityAction<Component, GameObject> OnObjectSelectedEvent;
    }

    public class CameraEvents
    {
        public UnityAction<Component, GameObject> OnPlayableCharacterFocusEvent;
    }

    public class NotificationEvents
    {
        public UnityAction<Component, string> OnErrorNotificationEvent;
    }

    public class ActionUseEvents
    {
        public UnityAction<Component, GameObject> OnActionUsed;
    }

    public class DamageRelatedEvents
    {
        public UnityAction<Component, GameObject, int> OnDamageTaken;
        public UnityAction<Component, GameObject, int, int> UpdateHealthBar;
        public UnityAction<Component, GameObject> OnDeath;
    }


    public class TurnEvents
    {
        /// <summary>
        /// Component -> this, GameObject -> next char
        /// </summary>
        public UnityAction<Component, GameObject> OnNextTurnEvent;

        public UnityAction<Component> OnBattleStartEvent;
        public UnityAction<Component> OnRoundEndedEvent;
    }
}