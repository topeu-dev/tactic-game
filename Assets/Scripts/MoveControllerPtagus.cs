using UnityEngine;
using System.Collections;
using System.Threading.Tasks;


public class MoveControllerPtagus : MonoBehaviour
{
  private TaskCompletionSource<bool> moveCompletionSource;
  private Vector3 targetPosition;
  private bool shouldMove = false;

  void Update()
  {
    if (shouldMove)
    {
      float speed = 5f * Time.deltaTime; // Скорость перемещения
      transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed);

      // Останавливаем движение при достижении цели
      if (Vector3.Distance(transform.position, targetPosition)< 1f)
      {
        Debug.Log("endofmove");
        shouldMove = false;

        // Завершаем задачу
        if (moveCompletionSource != null)
        {
          Debug.Log("endtask");
          moveCompletionSource.SetResult(true);
        }
      }
    }
  }

  // Метод для начала движения с асинхронным ожиданием завершения
  public async Task<bool> MoveToPositionAsync(Vector3 target)
  {
    Debug.Log("startasyncinmove");
    targetPosition = target;
    moveCompletionSource = new TaskCompletionSource<bool>();
    shouldMove = true;

    // Асинхронное ожидание завершения движения
    await moveCompletionSource.Task;
    Debug.Log("endasyncinmove");
    return true;
  }
}
