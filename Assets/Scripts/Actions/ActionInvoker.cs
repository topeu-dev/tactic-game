using UnityEngine;

namespace Actions
{
    public class ActionContext
    {
        public readonly GameObject CurrentActiveChar;
        public readonly GameObject ClickedObject;

        public ActionContext(GameObject currentActiveChar, GameObject clickedObject)
        {
            CurrentActiveChar = currentActiveChar;
            ClickedObject = clickedObject;
        }
    }

    public static class ActionInvoker
    {
        private static readonly MoveActionInvoker MoveActionInvoker = new();
        private static readonly HitActionInvoker HitActionInvoker = new();


        public static bool Invoke(string actionName, ActionContext actionContext)
        {
            if (actionName == "Move")
            {
                return MoveActionInvoker.Move(actionContext);
            }

            if (actionName == "Hit")
            {
                return HitActionInvoker.Hit(actionContext);
            }

            return false;
        }
    }
}