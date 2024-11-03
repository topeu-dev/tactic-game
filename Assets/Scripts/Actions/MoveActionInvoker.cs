using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace Actions
{
  public class MoveActionInvoker
  {
    public bool Move(ActionContext actionContext)
    {
      GameObject character = actionContext.CurrentActiveChar;

      if (character == null)
        return false;

      // Получаем или добавляем компонент Mover
      MoveControllerPtagus mover = character.GetComponent<MoveControllerPtagus>();
      if (mover == null)
      {
        mover = character.AddComponent<MoveControllerPtagus>();
      }

      // Устанавливаем цель перемещения
      if (actionContext.ClickedObject != null)
      {
        Vector3 targetPosition = actionContext.ClickedObject.transform.position;

        mover.MoveToPositionAsync(targetPosition);
        return true; // Движение успешно завершено
      }

      return false; // Цель не задана
    }
    /*
    public async Task<bool> MoveAsync(ActionContext actionContext)
    {
      Debug.Log("startasyncfunc");
      GameObject character = actionContext.CurrentActiveChar;

      if (character == null)
        return false;

      // Получаем или добавляем компонент Mover
      MoveControllerPtagus mover = character.GetComponent<MoveControllerPtagus>();
      if (mover == null)
      {
        mover = character.AddComponent<MoveControllerPtagus>();
      }

      // Устанавливаем цель перемещения
      if (actionContext.ClickedObject != null)
      {
        Vector3 targetPosition = actionContext.ClickedObject.transform.position;

        Debug.Log("startasyncfuncinmove");
        // Асинхронно ждем завершения движения
        await mover.MoveToPositionAsync(targetPosition);

        Debug.Log("Movement complete");
        return true; // Движение успешно завершено
      }

      return false; // Цель не задана
    }*/
  }
}