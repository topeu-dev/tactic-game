using Actions;
using UnityEngine;
using UnityEngine.InputSystem;

public enum GamePhase
{
    None,
    CharSelected,
    ActionSelected
}

public class InputController : MonoBehaviour
{
    [SerializeField]
    private GamePhase currentPhase = GamePhase.None;

    [SerializeField]
    private GameObject _activeCharacter;

    [SerializeField]
    private ActionSo selectedActionSO;

    private static int actionUnselected = -100;

    private bool actionInProgress = false;

    private void Awake()
    {
        InputActionSingleton.GeneralInputActions.Gameplay.PressBack.performed += HandlePressBack;
    }

    private void OnEnable()
    {
        InputActionSingleton.GeneralInputActions.Gameplay.PressBack.Enable();
    }

    private void OnDisable()
    {
        InputActionSingleton.GeneralInputActions.Gameplay.PressBack.Disable();
    }

    private void HandlePressBack(InputAction.CallbackContext obj)
    {
        switch (currentPhase)
        {
            case GamePhase.None:
                // toggle in-game menu
                break;
            case GamePhase.CharSelected:
                SetNewActiveCharacter(null);
                currentPhase = GamePhase.None;
                break;
            case GamePhase.ActionSelected:
                SelectAction(actionUnselected);
                currentPhase = GamePhase.CharSelected;
                break;
        }
    }

    public void HandleClickOnObject(GameObject clickedObject)
    {
        if (actionInProgress)
            return;

        if (currentPhase == GamePhase.None)
        {
            if (clickedObject.CompareTag("PlayableChar"))
            {
                SetNewActiveCharacter(clickedObject);
            }

            if (clickedObject.CompareTag("Cell"))
            {
                // ignore
            }

            if (clickedObject.CompareTag("Enemy"))
            {
                // show tooltip
            }

            return;
        }

        if (currentPhase == GamePhase.CharSelected)
        {
            if (clickedObject.CompareTag("PlayableChar"))
            {
                SetNewActiveCharacter(clickedObject);
            }

            if (clickedObject.CompareTag("Cell"))
            {
                // ignore
            }

            if (clickedObject.CompareTag("Enemy"))
            {
                //ignore
            }

            return;
        }

        if (currentPhase == GamePhase.ActionSelected)
        {
            if (CanApplyActionToClickedObject(clickedObject))
            {
                Debug.Log("Can apply");
                actionInProgress = true;
                ActionInvoker.Invoke(selectedActionSO.actionName, new ActionContext(_activeCharacter, clickedObject));
                currentPhase = GamePhase.CharSelected;
                actionInProgress = false;
            }
            else
            {
                Debug.Log("Can't apply" + selectedActionSO.actionName + " to " + " clickedObject: " + clickedObject.name
                          + " currentActiveCharacter: " + _activeCharacter.name);
            }
        }
    }

    private void SetNewActiveCharacter(GameObject clickedObject)
    {
        _activeCharacter = clickedObject;
        Debug.Log("Current Active Char -> " + clickedObject.name);
        if (clickedObject != null)
        {
            currentPhase = GamePhase.CharSelected;
            EventManager.SelectableObject.OnObjectSelectedEvent(this, clickedObject);
        }
    }

    public void SelectAction(int actionId)
    {
        var genericChar = _activeCharacter.GetComponent<GenericChar>();
        selectedActionSO = genericChar.actions[actionId];
        currentPhase = GamePhase.ActionSelected;
        Debug.Log("Spell selected: " + selectedActionSO.actionName);
    }

    private bool CanApplyActionToClickedObject(GameObject clickedObject)
    {
        return selectedActionSO.possibleObjectsToApply.Contains(clickedObject.tag);
    }
}