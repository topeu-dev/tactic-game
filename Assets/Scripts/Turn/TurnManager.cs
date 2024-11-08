using System;
using System.Collections.Generic;
using System.Linq;
using Turn;
using UnityEngine;

namespace DefaultNamespace
{
    public class TurnManager : MonoBehaviour
    {
        private Queue<GameObject> _queue;
        private List<GameObject> sortedTurnRelatedObjects;


        [SerializeField]
        private GameObject _selectedCharacter;


        void Start()
        {
            sortedTurnRelatedObjects = FindAllObjectsOfInterface<TurnRelated>()
                .OrderBy(obj => obj.GetComponent<TurnRelated>().GetInitiative().Value)
                .Reverse()
                .ToList();

            _queue = new Queue<GameObject>(sortedTurnRelatedObjects);
            
        }

        public void makeNextTurnSequence()
        {
            _queue = new Queue<GameObject>(sortedTurnRelatedObjects
                .OrderBy(obj => obj.GetComponent<TurnRelated>().GetInitiative().Value)
                .Reverse()
                .ToList());
        }

        public bool hasNextInSequence()
        {
            return _queue.Count > 0;
        }

        public GameObject nextTurn()
        {
            return _queue.Dequeue();
        }

        private void OnEnable()
        {
            // subscribe to DeathEvent
            // subscribe to ChangeInitiativeEvent
            // subscribe to SpawnEvent (?)
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
    }
}