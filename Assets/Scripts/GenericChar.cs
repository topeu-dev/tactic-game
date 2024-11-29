using System.Collections.Generic;
using Actions;
using Turn;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using Utility;

public class GenericChar : MonoBehaviour, TurnRelated
{
    
    private Animator _animatorRef;
    
    //todo мб перенести это куда то
    private EventTrigger _triggerRef;
    private CapsuleCollider _colliderRef;
    private NavMeshAgent _navMeshAgentRef;
    
    [SerializeField]
    private int startInitiativeValue;

    private Initiative currentInitiative;
    public int startHp;
    public int CurrentHp {get; private set;}

    [SerializeField]
    public List<ActionInstance> actionsInstances = new();

    private void Awake()
    {
        _colliderRef = GetComponent<CapsuleCollider>();
        _triggerRef = GetComponent<EventTrigger>();
        _navMeshAgentRef = GetComponent<NavMeshAgent>();
        _animatorRef = GetComponent<Animator>();
        currentInitiative = new Initiative(startInitiativeValue, 10);
        actionsInstances.ForEach(action => action.Init());
        CurrentHp = startHp;
    }

    private void OnEnable()
    {
        EventManager.DamageRelatedEvent.OnDamageTaken += OnDamageTaken;
    }

    private void OnDisable()
    {
        EventManager.DamageRelatedEvent.OnDamageTaken -= OnDamageTaken;
    }

    public void EndOfTurn()
    {
        Debug.Log("End of Turn: " + gameObject.name);
    }

    public void StartOfTurn()
    {
        actionsInstances.ForEach(action => action.StartOfTurn());
        EventManager.CameraEvent.OnPlayableCharacterFocusEvent?.Invoke(this, gameObject);
    }

    public Initiative GetInitiative()
    {
        return currentInitiative;
    }


    private void OnDamageTaken(Component source, GameObject targetObject, int damage)
    {
        if (targetObject != gameObject) return;

        CurrentHp -= damage;

        EventManager.DamageRelatedEvent.UpdateHealthBar.Invoke(this, gameObject, CurrentHp, startHp);
        
        if (CurrentHp <= 0)
        {
            _animatorRef.SetTrigger(AnimTriggers.Death);
            _colliderRef.enabled = false;
            _triggerRef.enabled = false;
            _navMeshAgentRef.enabled = false;
                
            EventManager.DamageRelatedEvent.OnDeath(this, gameObject);
            Debug.Log(gameObject.name + " damage taken -> " + damage + " char is Dead");
        }
        else
        {
            _animatorRef.SetTrigger(AnimTriggers.GetHit);
            Debug.Log(gameObject.name + " damage taken -> " + damage);
        }
    }
}