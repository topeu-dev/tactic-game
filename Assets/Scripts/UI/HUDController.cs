using System.Collections;
using System.Collections.Generic;
using Actions;
using Enemy;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    private Canvas _canvas;
    private GameObject _characterWithActiveTurn;
    private GameObject _characterHudFor;

    public GameObject actionPanel;
    public List<GameObject> actionPlaceholders = new();
    public List<GameObject> hintPlaceholders = new();
    public GameObject notificationBar;
    public GameObject endOfTurnButton;

    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
        notificationBar.SetActive(false);
    }

    private void OnEnable()
    {
        EventManager.TurnEvent.OnNextTurnEvent += TurnChanged;
        EventManager.SelectableObject.OnObjectSelectedEvent += ShowHudFor;
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
        Debug.Log("Turn Changed invoked" + arg1.name + " source " + arg0.name);
        _characterWithActiveTurn = arg1;
        ShowHudFor(this, _characterWithActiveTurn);
    }

    private void ShowHudFor(Component source, GameObject clickedObject)
    {
        // Debug.Log("ShowHudFor invoked" + clickedObject.name + " in source: " + source.name + "char with active turn" + _characterWithActiveTurn.name);
        if (clickedObject != null && clickedObject.TryGetComponent(out GenericChar character))
        {
            _characterHudFor = clickedObject;
            ShowHudWithActions(character.actionsInstances, clickedObject);
            return;
        }

        if (clickedObject && clickedObject.TryGetComponent(out GenericEnemy enemy))
        {
            Debug.Log("ShowHudFor invoked 2" + clickedObject.name);
            actionPanel.SetActive(false);
        }
    }

    private void ShowHudWithActions(List<ActionInstance> actions, GameObject clickedObject)
    {
        actionPanel.SetActive(true);
        var buttons = actionPlaceholders;
        var hints = hintPlaceholders;

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
            var hintPlaceHolder = hints[i].GetComponent<Image>();

            if (i < actions.Count && actions[i] != null)
            {
                hintPlaceHolder.sprite = actions[i].actionDescription.iconDescription;
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

  public void EnableHint(int i)
  {
    hintPlaceholders[i].gameObject.SetActive(true);
  }
  public void DisableHint(int i)
  {
    hintPlaceholders[i].gameObject.SetActive(false);
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