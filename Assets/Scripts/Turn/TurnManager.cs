using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Turn
{
    public class TurnManager : MonoBehaviour
    {
        private LinkedList<GameObject> _roundQueue;
        private List<GameObject> sortedTurnRelatedObjects;

        [SerializeField]
        private GameObject _selectedCharacter;


        void Start()
        {
            sortedTurnRelatedObjects = FindAllObjectsOfInterface<TurnRelated>()
                .OrderBy(obj => obj.GetComponent<TurnRelated>().GetInitiative().Value)
                .Reverse()
                .ToList();

            makeNextTurnSequence();
        }

        public void makeNextTurnSequence()
        {
      _roundQueue = new LinkedList<GameObject>(sortedTurnRelatedObjects
                .OrderBy(obj => obj.GetComponent<TurnRelated>().GetInitiative().Value)
                .Reverse()
                .ToList());
            EventManager.TurnEvent.OnRoundEndedEvent?.Invoke(this);
            EventManager.TurnEvent.OnNextTurnSequenceEvent?.Invoke(this, new LinkedList<GameObject>(_roundQueue));
        }

        public bool hasNextInSequence()
        {
            return _roundQueue.Count > 0;
        }

        public GameObject nextTurn()
        {
            var obj = _roundQueue.First();
            Debug.Log("NEXT_TURN" + obj.name);
            _roundQueue.RemoveFirst();
            return obj;
        }

    private void OnEnable()
        {
            EventManager.DamageRelatedEvent.OnDeath += HandleOnDeath;
        }

        private void OnDisable()
        {
            EventManager.DamageRelatedEvent.OnDeath -= HandleOnDeath;
        }

        private void HandleOnDeath(Component arg0, GameObject arg1)
        {
            removeFromTurnRelatedObjects(arg1);
        }

        private void removeFromTurnRelatedObjects(GameObject o)
        {
            sortedTurnRelatedObjects.Remove(o);
            _roundQueue.Remove(o);
        }


        private List<GameObject> FindAllObjectsOfInterface<T>()
        {
            List<GameObject> result = new List<GameObject>();

            // Get all GameObjects in the scene
            GameObject[] allObjects = FindObjectsOfType<GameObject>();

            foreach (GameObject obj in allObjects)
            {
                // Get all components of the GameObject
                T components = obj.GetComponent<T>();
                if (components != null)
                {
                    Debug.Log(obj.name);
                    result.Add(obj);
                }
            }

            return result;
        }

        public LinkedList<GameObject> GetRoundQueue()
        {
            return _roundQueue;
        }
    }
}