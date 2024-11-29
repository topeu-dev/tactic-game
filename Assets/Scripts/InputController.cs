using System.Collections.Generic;
using Actions;
using Actions.Visualizers;
using TaskApproachTest;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Utility;

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

    [FormerlySerializedAs("_activeCharacter")]
    [SerializeField]
    private GameObject _selectedCharacter;

    [FormerlySerializedAs("selectedActionDescription")]
    [SerializeField]
    private ActionInstance selectedAction;

    private static int actionUnselected = -100;

    [SerializeField]
    private bool actionInProgress = false;

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
        var raycastRangeHitVisualizer = FindFirstObjectByType<RaycastTargetVisualizer>();
        _actionVisualizers.Add(ActionVisualizerType.RaycastTargetHit, raycastRangeHitVisualizer);
        var meleeHitAoeVisualizer = FindFirstObjectByType<AoeMeleeHitVisualizer>();
        _actionVisualizers.Add(ActionVisualizerType.MeleeHitAoe, meleeHitAoeVisualizer);
    }

    private void OnEnable()
    {
        InputActionSingleton.GeneralInputActions.Gameplay.PressBack.Enable();
        EventManager.TurnEvent.OnNextTurnEvent += TurnChanged;
    }

    private void TurnChanged(Component arg0, GameObject arg1)
    {
        if (arg1.CompareTag("PlayableChar"))
        {
            SelectAction(actionUnselected);
            _selectedCharacter = arg1;
            // SelectNewCharacter(arg1);
        }
        else
        {
            SelectAction(actionUnselected);
            _selectedCharacter = null;
        }
    }

    private void OnDisable()
    {
        InputActionSingleton.GeneralInputActions.Gameplay.PressBack.Disable();
        EventManager.TurnEvent.OnNextTurnEvent -= TurnChanged;
    }

    private void HandlePressBack(InputAction.CallbackContext obj)
    {
        switch (currentPhase)
        {
            case GamePhase.None:
                // toggle in-game menu
                break;
            case GamePhase.CharSelected:
                SelectNewCharacter(null);
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
                SelectNewCharacter(clickedObject);
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
                SelectNewCharacter(clickedObject);
            }

            if (clickedObject.CompareTag(CustomTags.BattleField.ToString()))
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
            if (CanApplyActionToClickedObject(clickedObject) ||
                selectedAction.actionDescription.possibleObjectsToApply.Contains("BattleField"))
            {
                actionInProgress = true;
                TaskExecutor taskExecutor = _selectedCharacter.GetComponent<TaskExecutor>();
                taskExecutor.ExecuteTask(
                    selectedAction,
                    new ActionContext(_selectedCharacter, clickedObject, battleFieldClickedPos),
                    () => UnlockInput()
                );

                currentPhase = GamePhase.CharSelected;
                _selectedActionVisualizer.DisableVisualizer();
                _selectedActionVisualizer = null;
            }
            else
            {
                Debug.Log("Can't apply" + selectedAction.actionDescription.actionName + " to " + " clickedObject: " +
                          clickedObject.name
                          + " currentActiveCharacter: " + _selectedCharacter.name);
            }
        }
    }

    private void SelectNewCharacter(GameObject selectedChar)
    {
        _selectedCharacter = selectedChar;

        if (selectedChar != null)
        {
            Debug.Log("Current Active Char -> " + selectedChar.name);
            currentPhase = GamePhase.CharSelected;
            EventManager.SelectableObject.OnObjectSelectedEvent.Invoke(this, selectedChar);
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

        var genericChar = _selectedCharacter.GetComponent<GenericChar>();
        selectedAction = genericChar.actionsInstances[actionId];

        currentPhase = GamePhase.ActionSelected;
        var visualizer = _actionVisualizers[selectedAction.actionDescription.actionVisualizerType];
        if (visualizer != null)
        {
            _selectedActionVisualizer = visualizer;
            visualizer.EnableVisualizerFor(_selectedCharacter, selectedAction);
        }

        Debug.Log("Spell selected: " + selectedAction.actionDescription.actionName);
    }

    private bool CanApplyActionToClickedObject(GameObject clickedObject)
    {
        return selectedAction.actionDescription.possibleObjectsToApply.Contains(clickedObject.tag);
    }

    public void UnlockInput()
    {
        Debug.Log("Unlocked from callback!!!");
        actionInProgress = false;
    }
}