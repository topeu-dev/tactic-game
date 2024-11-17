using System.Collections;
using System.Collections.Generic;
using Actions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    private Canvas _canvas;
    private GameObject _characterWithActiveTurn;
    private GameObject _characterHudFor;

    public List<GameObject> actionPlaceholders = new();
    public GameObject notificationBar;
    public GameObject endOfTurnButton;

    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
        notificationBar.SetActive(false);
    }

    private void OnEnable()
    {
        EventManager.SelectableObject.OnObjectSelectedEvent += ShowHudFor;
        EventManager.TurnEvent.OnNextTurnEvent += TurnChanged;
        EventManager.NotificationEvent.OnErrorNotificationEvent += ShowErrorNotification;
        EventManager.ActionUseEvent.OnActionUsed += UpdateHud;
    }

    private void OnDisable()
    {
        EventManager.SelectableObject.OnObjectSelectedEvent -= ShowHudFor;
        EventManager.TurnEvent.OnNextTurnEvent -= TurnChanged;
    }

    
    private void UpdateHud(Component arg0, GameObject arg1)
    {
        ShowHudFor(this, _characterHudFor);
    }

    private void TurnChanged(Component arg0, GameObject arg1)
    {
        _characterWithActiveTurn = arg1;
    }

    private void ShowHudFor(Component source, GameObject clickedObject)
    {
        if (clickedObject is GameObject o && o.TryGetComponent(out GenericChar character))
        {
            _characterHudFor = clickedObject;
            ShowHudWithActions(character.actionsInstances, clickedObject);
        }
    }

    private void ShowHudWithActions(List<ActionInstance> actions, GameObject clickedObject)
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
                iconPlaceHolder.sprite = actions[i].actionDescription.icon;
                iconPlaceHolder.enabled = true;
                var textMeshProUGUI = buttons[i].GetComponentInChildren<TextMeshProUGUI>();
                textMeshProUGUI.enabled = false;
                if (actions[i].CurrentCooldown > 0)
                {
                    textMeshProUGUI.enabled = true;
                    textMeshProUGUI.text = actions[i].CurrentCooldown.ToString();
                    buttonComp.interactable = false;
                }
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


    private void ShowErrorNotification(Component arg0, string text)
    {
        notificationBar.GetComponent<TextMeshProUGUI>().text = text;
        StartCoroutine(showNotification());
    }

    private IEnumerator showNotification()
    {
        notificationBar.SetActive(true);
        yield return new WaitForSeconds(1);
        notificationBar.SetActive(false);
    }
}