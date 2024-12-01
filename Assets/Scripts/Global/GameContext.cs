using Turn;
using UnityEngine;

namespace Global
{
    public class GameContext : MonoBehaviour
    {
        private TurnManager _turnManager;
        private GameObject _activeObject;


        private void Awake()
        {
            _turnManager = GetComponent<TurnManager>();
        }

        public void StartBattle()
        {
            _activeObject = _turnManager.nextTurn();
            var newTurnRelated = _activeObject.GetComponent<TurnRelated>();
            newTurnRelated.StartOfTurn();
            EventManager.TurnEvent.OnNextTurnEvent?.Invoke(this, _activeObject);
            EventManager.TurnEvent.OnBattleStartEvent?.Invoke(this);
        }


        public void turnEndClicked()
        {
            var oldActiveObjectTurnRelated = _activeObject.GetComponent<TurnRelated>();
            oldActiveObjectTurnRelated.EndOfTurn();


            if (_turnManager.hasNextInSequence())
            {
                _activeObject = _turnManager.nextTurn();
            }
            else
            {
                _turnManager.makeNextTurnSequence();
                _activeObject = _turnManager.nextTurn();
            }

            var newTurnRelated = _activeObject.GetComponent<TurnRelated>();
            newTurnRelated.StartOfTurn();
            EventManager.TurnEvent.OnNextTurnEvent?.Invoke(this, _activeObject);
        }

        public void activeCharSkipTurn()
        {

            if (_turnManager.hasNextInSequence())
            {
                _activeObject = _turnManager.nextTurn();
            }
            else
            {
                _turnManager.makeNextTurnSequence();
                _activeObject = _turnManager.nextTurn();
            }

            var newTurnRelated = _activeObject.GetComponent<TurnRelated>();
            newTurnRelated.StartOfTurn();
            EventManager.TurnEvent.OnNextTurnEvent?.Invoke(this, _activeObject);
        }
    }
}