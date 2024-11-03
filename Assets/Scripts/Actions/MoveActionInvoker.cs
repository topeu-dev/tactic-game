using UnityEngine;

namespace Actions
{
    public class MoveActionInvoker
    {
        public bool Move(ActionContext actionContext)
        {
            Debug.Log("Move" + " about to perform");

            Vector3 ignoreY = new Vector3(
                actionContext.ClickedObject.transform.position.x,
                actionContext.CurrentActiveChar.transform.position.y,
                actionContext.ClickedObject.transform.position.z
            );
            actionContext.CurrentActiveChar.transform.position = ignoreY;
            return true;
        }
    }
}