using UnityEngine;

namespace Actions
{
    public class HitActionInvoker
    {
        public bool Hit(ActionContext actionContext)
        {
            Debug.Log(actionContext.ActionName + " about to perform");
            Debug.Log(actionContext.CurrentActiveChar.name + " hit " + actionContext.ClickedObject.name);
            return true;
        }
    }
}