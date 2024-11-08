using System.Collections.Generic;
using Actions;
using Actions.Visualizers;
using TaskApproachTest;
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

    [SerializeField]
    private bool actionInProgress = false;

    private GameObject _characterWithActiveTurn;

    private Dictionary<ActionVisualizerType, ActionVisualizer> _actionVisualizers = new();
    private ActionVisualizer _selectedActionVisualizer;

    private void Awake()
    {
        InputActionSingleton.GeneralInputActions.Gameplay.PressBack.performed += HandlePressBack;
    }

    private void Start()
    {
        var moveVisualizer = FindFirstObjectByType<MoveVisualizer>();
        _actionVisualizers.Add(ActionVisualizerType.Move, moveVisualizer);
        var hitVisualizer = FindFirstObjectByType<HitVisualizer>();
        _actionVisualizers.Add(ActionVisualizerType.MeleeHit, hitVisualizer);
    }

    private void OnEnable()
    {
        InputActionSingleton.GeneralInputActions.Gameplay.PressBack.Enable();
        EventManager.TurnEvent.OnNextTurnEvent += TurnChanged;
    }

    private void TurnChanged(Component arg0, GameObject arg1)
    {
        _characterWithActiveTurn = arg1;
        if (arg1.tag == "PlayableChar")
        {
            SelectAction(actionUnselected);
            SetNewActiveCharacter(arg1);
        }
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

    public Vector3? GetBattleFieldHitPosition()
    {
        // Debug.Log(Input.mousePosition);
        // Convert screen position to world position using a raycast
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 400f, 1 << LayerMask.NameToLayer("BattleFieldLayer")))
        {
            Vector3 worldPosition = hit.point;
            Debug.Log("battlefield hit found " + worldPosition);
            return worldPosition;
        }

        Debug.Log("No battlefield hit found");
        return null;
    }

    public void HandleClickOnObject(GameObject clickedObject)
    {
        Debug.Log(clickedObject);
        if (actionInProgress)
            return;

        Vector3? battleFieldClickedPos = GetBattleFieldHitPosition();

        if (currentPhase == GamePhase.None)
        {
            if (clickedObject.CompareTag("PlayableChar"))
            {
                SetNewActiveCharacter(clickedObject);
            }

            if (clickedObject.CompareTag("BattleField"))
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

            if (clickedObject.CompareTag("BattleField"))
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
                TaskExecutor taskExecutor = _activeCharacter.GetComponent<TaskExecutor>();
                taskExecutor.ExecuteTask(
                    selectedActionSO,
                    new ActionContext(_activeCharacter, clickedObject, battleFieldClickedPos),
                    () => UnlockInput()
                );

                currentPhase = GamePhase.CharSelected;
                _selectedActionVisualizer.DisableVisualizer();
                _selectedActionVisualizer = null;
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

        if (clickedObject != null)
        {
            Debug.Log("Current Active Char -> " + clickedObject.name);
            currentPhase = GamePhase.CharSelected;
            EventManager.SelectableObject.OnObjectSelectedEvent(this, clickedObject);
        }
    }

    public void SelectAction(int actionId)
    {
        if (_selectedActionVisualizer != null)
        {
            _selectedActionVisualizer.DisableVisualizer();
            _selectedActionVisualizer = null;
        }

        if (actionId == actionUnselected)
        {
            return;
        }

        var genericChar = _activeCharacter.GetComponent<GenericChar>();
        selectedActionSO = genericChar.actions[actionId];
        currentPhase = GamePhase.ActionSelected;
        var visualizer = _actionVisualizers[selectedActionSO.actionVisualizerType];
        if (visualizer != null)
        {
            _selectedActionVisualizer = visualizer;
            visualizer.EnableVisualizerFor(_activeCharacter);
        }

        Debug.Log("Spell selected: " + selectedActionSO.actionName);
    }

    private bool CanApplyActionToClickedObject(GameObject clickedObject)
    {
        return selectedActionSO.possibleObjectsToApply.Contains(clickedObject.tag);
    }

    public void UnlockInput()
    {
        Debug.Log("Unlocked from callback!!!");
        actionInProgress = false;
    }
}