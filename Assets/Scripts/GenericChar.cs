using System;
using System.Collections.Generic;
using Actions;
using Effects;
using Turn;
using UnityEngine;

public class GenericChar : MonoBehaviour, TurnRelated
{
    [SerializeField]
    private int startInitiativeValue;

    private Initiative currentInitiative;

    public List<ActionSo> actions = new();
    public List<Effect> currentEffects = new();

    private void Awake()
    {
        currentInitiative = new Initiative(startInitiativeValue, 10);
    }

    public void EndOfTurn()
    {
        Debug.Log("End of Turn: " + gameObject.name);
    }

    public void StartOfTurn()
    {
        EventManager.CameraEvent.OnPlayableCharacterFocusEvent?.Invoke(this, gameObject);
    }

    public Initiative GetInitiative()
    {
        return currentInitiative;
    }
}