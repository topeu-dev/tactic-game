using System.Collections.Generic;
using Actions;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    private Canvas _canvas;
    
    public List<GameObject> actionPlaceholders = new();

    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
    }

    private void OnEnable()
    {
        EventManager.SelectableObject.OnObjectSelectedEvent += ShowHudFor;
    }

    private void OnDisable()
    {
        EventManager.SelectableObject.OnObjectSelectedEvent -= ShowHudFor;
    }


    private void ShowHudFor(Component source, GameObject clickedObject)
    {
        if (clickedObject is GameObject o && o.TryGetComponent(out GenericChar character))
        {
            ShowHudWithActions(character.actions);
        }
    }

    private void ShowHudWithActions(List<ActionSo> actions)
    {
        var buttons = actionPlaceholders;

        for (var i = 0; i < buttons.Count; i++)
        {
            Debug.Log(buttons[i].name);
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
    }
}