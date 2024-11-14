using System.Collections.Generic;
using Actions;
using Turn;
using UnityEngine;

public class GenericChar : MonoBehaviour, TurnRelated
{
    [SerializeField]
    private int startInitiativeValue;

    private Initiative currentInitiative;

    [SerializeField]
    public List<ActionInstance> actionsInstances = new();

    // public List<Effect> currentEffects = new();

    private void Awake()
    {
        currentInitiative = new Initiative(startInitiativeValue, 10);
        actionsInstances.ForEach(action => action.Init());
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
}