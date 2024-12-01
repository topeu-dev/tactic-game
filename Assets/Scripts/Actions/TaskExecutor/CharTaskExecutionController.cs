using System;
using Actions;
using Actions.TaskExecutor;
using TaskApproachTest;
using UnityEngine;

public class CharTaskExecutionController : MonoBehaviour, TaskExecutor
{
    public void ExecuteTask(ActionInstance actionInstance, ActionContext actionContext, Action callback)
    {
        switch (actionInstance.actionDescription.actionVisualizerType)
        {
            case ActionVisualizerType.Move:
            {
                gameObject.GetComponent<CharMover>().Move(actionInstance, actionContext, callback);
                break;
            }
            case ActionVisualizerType.MeleeHit:
            {
                gameObject.GetComponent<MeleeHitExecutor>().Hit(actionInstance, actionContext, callback);
                break;
            }
            case ActionVisualizerType.MeleeHitAoe:
            {
                gameObject.GetComponent<MeleeAoeExecutor>().AoeHit(actionInstance, actionContext, callback);
                break;
            }
            case ActionVisualizerType.RaycastTargetHit:
            {
                gameObject.GetComponent<HitScanExecutor>().HitScan(actionInstance, actionContext, callback);
                break;
            }
            default:
                Debug.Log("Error: gameObject: " + gameObject.name + " doesn't know how to execute " +
                          actionInstance.actionDescription.actionName);
                break;
        }


        // switch (actionInstance.actionDescription.actionName)
        // {
        //     case "Move":
        //         gameObject.GetComponent<CharMover>().Move(actionInstance, actionContext, callback);
        //         break;
        //     case "Hit":
        //         gameObject.GetComponent<MeleeHitExecutor>().Hit(actionInstance, actionContext, callback);
        //         break;
        //     case "MeleeAoeHit":
        //         gameObject.GetComponent<MeleeAoeExecutor>().AoeHit(actionInstance, actionContext, callback);
        //         break;
        //     case "Raycast":
        //         gameObject.GetComponent<HitScanExecutor>().HitScan(actionInstance, actionContext, callback);
        //         break;
        //     case "ApplyRage":
        //         gameObject.GetComponent<HitScanExecutor>().HitScan(actionInstance, actionContext, callback);
        //         break;  
        //     case "Heal":
        //         gameObject.GetComponent<HitScanExecutor>().HitScan(actionInstance, actionContext, callback);
        //         break;   
        //     default:
        //         Debug.Log("Error: gameObject: " + gameObject.name + " doesn't know how to execute " +
        //                   actionInstance.actionDescription.actionName);
        //         break;
        // }
    }
}