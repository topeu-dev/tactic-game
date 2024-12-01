using System;
using System.Collections.Generic;
using System.Linq;
using Actions;
using Effects;
using Global;
using Turn;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using Utility;

public class GenericChar : MonoBehaviour, TurnRelated
{
    private Animator _animatorRef;
    private GameContext _gameContextRef;
    private EffectController _effectControllerRef;

    //todo мб перенести это куда то
    private EventTrigger _triggerRef;
    private CapsuleCollider _colliderRef;
    private NavMeshAgent _navMeshAgentRef;

    [SerializeField]
    private int startInitiativeValue;

    private Initiative currentInitiative;
    public int startHp;
    public int CurrentHp { get; private set; }

    [SerializeField]
    public List<ActionInstance> actionsInstances = new();

    [SerializeField]
    public List<EffectInstance> effectsInstances = new();

    private void Awake()
    {
        _effectControllerRef = GetComponent<EffectController>();
        _gameContextRef = FindAnyObjectByType<GameContext>();
        _colliderRef = GetComponent<CapsuleCollider>();
        _triggerRef = GetComponent<EventTrigger>();
        _navMeshAgentRef = GetComponent<NavMeshAgent>();
        _animatorRef = GetComponent<Animator>();
        currentInitiative = new Initiative(startInitiativeValue, 10);
        actionsInstances.ForEach(action => action.Init());
        CurrentHp = startHp;
    }

    private void Start()
    {
        _effectControllerRef.EnableInitialVfxs(effectsInstances);
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

        effectsInstances
            .FindAll(effect => effect.effectDescription.whenToApply == WhenToApply.OnTurnStart)
            .ForEach(effect => _effectControllerRef.HandleEffectOnTurnStart(effect, actionsInstances));

        foreach (var effect in effectsInstances.ToList())
        {
            effect.DecreaseDuration();
            if (effect.currentDuration <= 0)
            {
                _effectControllerRef.EffectEnded(effectsInstances, effect);
            }
        }

        if (effectsInstances.Find(ei => ei.effectDescription.effectType == EffectType.Stun) != null)
        {
            _gameContextRef.activeCharSkipTurn();
        }
        EventManager.CameraEvent.OnPlayableCharacterFocusEvent?.Invoke(this, gameObject);
        // check if dead
        if (CurrentHp <= 0)
        {
            _gameContextRef.activeCharSkipTurn();
        }
    }

    public Initiative GetInitiative()
    {
        return currentInitiative;
    }


    private void OnDamageTaken(Component source, GameObject targetObject, int damage, float impactDelay)
    {
        if (targetObject != gameObject) return;

        var blockEffect = effectsInstances.Find(ei => ei.effectDescription.effectType == EffectType.Block);
        if (blockEffect != null) 
        {
            Debug.Log("Attack blocked");
            _animatorRef.SetTrigger(AnimTriggers.Block);
            //play someSound
            _effectControllerRef.EffectEnded(effectsInstances, blockEffect);
            return;
        }
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
            //todo: нужно посчитать какую анимацию включить в зависимости от текущих состояний (мб использовать приоритет)
            _animatorRef.SetTrigger(AnimTriggers.GetHit);
            Debug.Log(gameObject.name + " damage taken -> " + damage);
        }
    }
}