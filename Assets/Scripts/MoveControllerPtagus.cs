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
      float speed = 5f * Time.deltaTime; // �������� �����������
      transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed);

      // ������������� �������� ��� ���������� ����
      if (Vector3.Distance(transform.position, targetPosition)< 1f)
      {
        Debug.Log("endofmove");
        shouldMove = false;

        // ��������� ������
        if (moveCompletionSource != null)
        {
          Debug.Log("endtask");
          moveCompletionSource.SetResult(true);
        }
      }
    }
  }

  // ����� ��� ������ �������� � ����������� ��������� ����������
  public async Task<bool> MoveToPositionAsync(Vector3 target)
  {
    Debug.Log("startasyncinmove");
    targetPosition = target;
    moveCompletionSource = new TaskCompletionSource<bool>();
    shouldMove = true;

    // ����������� �������� ���������� ��������
    await moveCompletionSource.Task;
    Debug.Log("endasyncinmove");
    return true;
  }
}
