using System;
using Actions;
using TaskApproachTest;
using UnityEngine;

public class CharTaskExecutionController : MonoBehaviour, TaskExecutor
{
    public void ExecuteTask(ActionSo actionSo, ActionContext actionContext, Action callback)
    {
        switch (actionSo.actionName)
        {
            case "Move":
                gameObject.GetComponent<CharMover>().Move(actionContext, callback);
                break;
            case "Hit":
                gameObject.GetComponent<MeleeHitExecutor>().Hit(actionContext, callback);
                break;
            default:
                Debug.Log("Error: gameObject: " + gameObject.name + " doesn't know how to execute " +
                          actionSo.actionName);
                break;
        }
    }
}