using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace Actions
{
  public class MoveActionInvoker
  {
    public bool Move(ActionContext actionContext)
    {

    }
    /*
    public async Task<bool> MoveAsync(ActionContext actionContext)
    {
      Debug.Log("startasyncfunc");
      GameObject character = actionContext.CurrentActiveChar;

      if (character == null)
        return false;

      // Ïîëó÷àåì èëè äîáàâëÿåì êîìïîíåíò Mover
      MoveControllerPtagus mover = character.GetComponent<MoveControllerPtagus>();
      if (mover == null)
      {
        mover = character.AddComponent<MoveControllerPtagus>();
      }

      // Óñòàíàâëèâàåì öåëü ïåðåìåùåíèÿ
      if (actionContext.ClickedObject != null)
      {
        Vector3 targetPosition = actionContext.ClickedObject.transform.position;

        Debug.Log("startasyncfuncinmove");
        // Àñèíõðîííî æäåì çàâåðøåíèÿ äâèæåíèÿ
        await mover.MoveToPositionAsync(targetPosition);

        Debug.Log("Movement complete");
        return true; // Äâèæåíèå óñïåøíî çàâåðøåíî
      }

      return false; // Öåëü íå çàäàíà
    }*/
  }
}