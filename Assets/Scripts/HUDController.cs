using System.Collections.Generic;
using Actions;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    private Canvas _canvas;
    private GameObject _characterWithActiveTurn;
    
    public List<GameObject> actionPlaceholders = new();
    public GameObject endOfTurnButton;

    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
    }

    private void OnEnable()
    {
        EventManager.SelectableObject.OnObjectSelectedEvent += ShowHudFor;
        EventManager.TurnEvent.OnNextTurnEvent += TurnChanged;
    }

    private void TurnChanged(Component arg0, GameObject arg1)
    {
        _characterWithActiveTurn = arg1;
    }

    private void OnDisable()
    {
        EventManager.SelectableObject.OnObjectSelectedEvent -= ShowHudFor;
    }


    private void ShowHudFor(Component source, GameObject clickedObject)
    {
        if (clickedObject is GameObject o && o.TryGetComponent(out GenericChar character))
        {
            ShowHudWithActions(character.actions, clickedObject);
        }
    }

    private void ShowHudWithActions(List<ActionSo> actions, GameObject clickedObject)
    {
        var buttons = actionPlaceholders;

        for (var i = 0; i < buttons.Count; i++)
        {
            var buttonComp = buttons[i].GetComponent<Button>();
            if (_characterWithActiveTurn != null && _characterWithActiveTurn == clickedObject)
            {
                buttonComp.interactable = true;
            }
            else
            {
                buttonComp.interactable = false;
            }
            
            var iconPlaceHolder = buttons[i].GetComponent<Image>();
            
            if (i < actions.Count && actions[i] != null)
            {
                iconPlaceHolder.sprite = actions[i].icon;
                iconPlaceHolder.enabled = true;
            }
            else
            {
                iconPlaceHolder.enabled = false;
            }
        }
        
        
        var buttonEndOfTurnComp = endOfTurnButton.GetComponent<Button>();
        if (_characterWithActiveTurn != null && _characterWithActiveTurn == clickedObject)
        {
            buttonEndOfTurnComp.interactable = true;
        }
        else
        {
            buttonEndOfTurnComp.interactable = false;
        }
    }
}