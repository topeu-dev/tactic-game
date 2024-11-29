using System.Collections.Generic;
using Actions;
using Global;
using Turn;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using Utility;

namespace Enemy
{
    public class GenericEnemy : MonoBehaviour, TurnRelated
    {
        private Animator _animatorRef;
        
        //todo мб перенести это куда то
        private EventTrigger _triggerRef;
        private CapsuleCollider _colliderRef;
        private NavMeshAgent _navMeshAgentRef;

        [SerializeField]
        private GameContext gameContext;

        [SerializeField]
        private BotController _botController;

        [SerializeField]
        private int startInitiativeValue;

        private Initiative currentInitiative;
        public int startHp;
        public int _currentHp;

        [SerializeField]
        public List<ActionInstance> actionsInstances = new();


        private void OnEnable()
        {
            EventManager.DamageRelatedEvent.OnDamageTaken += OnDamageTaken;
        }

        private void OnDisable()
        {
            EventManager.DamageRelatedEvent.OnDamageTaken -= OnDamageTaken;
        }

        private void OnDamageTaken(Component source, GameObject targetObject, int damage)
        {
            if (targetObject != gameObject) return;

            _currentHp -= damage;
            EventManager.DamageRelatedEvent.UpdateHealthBar.Invoke(this, gameObject, _currentHp, startHp);

            if (_currentHp <= 0)
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

        private void Awake()
        {
            _colliderRef = GetComponent<CapsuleCollider>();
            _triggerRef = GetComponent<EventTrigger>();
            _navMeshAgentRef = GetComponent<NavMeshAgent>();
            
            _animatorRef = GetComponent<Animator>();
            _botController = GetComponent<BotController>();
            currentInitiative = new Initiative(startInitiativeValue, 10);
            actionsInstances.ForEach(action => action.Init());
            _currentHp = startHp;
        }

        public void EndOfTurn()
        {
            Debug.Log("ENEMY END OF TURN");
        }

        public void StartOfTurn()
        {
            Debug.Log("ENEMY START OF TURN");

            //refresh cooldowns
            actionsInstances.ForEach(action => action.StartOfTurn());

            EventManager.CameraEvent.OnPlayableCharacterFocusEvent?.Invoke(this, gameObject);
            StartCoroutine(_botController.MakeMove(actionsInstances, () =>
            {
                Debug.Log("Coroutine done");
                gameContext.turnEndClicked();
            }));
        }

        public Initiative GetInitiative()
        {
            return currentInitiative;
        }
    }
}