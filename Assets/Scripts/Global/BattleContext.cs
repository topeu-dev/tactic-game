using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Global
{
    public class BattleContext : MonoBehaviour
    {
        private readonly Dictionary<GameObject, CharacterInfo> _charactersInBattle = new();

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
            print(_charactersInBattle.Count);
            _charactersInBattle.Remove(arg1);
            print(_charactersInBattle.Count);
            if (getOnlyPlayerCharactersInBattle().Count == 0)
            {
                // TODO GAME OVER EVENT
                Debug.Log("GAME OVER GGWP");
            }

            if (getOnlyEnemiesInBattle().Count == 0)
            {
                // TODO U WON!
                Debug.Log("U WON, WELL PLAYED!");
            }
        }

        private void Awake()
        {
            foreach (var playableChar in GameObject.FindGameObjectsWithTag("PlayableChar"))
            {
                _charactersInBattle.Add(playableChar, new CharacterInfo(
                        true
                    )
                );
            }

            foreach (var enemyChar in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                _charactersInBattle.Add(enemyChar, new CharacterInfo(
                        true
                    )
                );
            }
        }


        public Dictionary<GameObject, CharacterInfo> GetCharactersInBattle()
        {
            return _charactersInBattle;
        }

        public Dictionary<GameObject, CharacterInfo> getOnlyPlayerCharactersInBattle()
        {
            return _charactersInBattle
                .Where(entry => entry.Key.CompareTag("PlayableChar"))
                .ToDictionary(entry => entry.Key, entry => entry.Value);
        }
        
        public Dictionary<GameObject, CharacterInfo> getOnlyEnemiesInBattle()
        {
            return _charactersInBattle
                .Where(entry => entry.Key.CompareTag("Enemy"))
                .ToDictionary(entry => entry.Key, entry => entry.Value);
        }
    }
    

    public class CharacterInfo
    {
        public bool IsVisible { get; set; }

        public CharacterInfo(bool isVisible)
        {
            IsVisible = isVisible;
        }
    }
}