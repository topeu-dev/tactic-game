using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(LineRenderer))]
public class PathVisualizer : MonoBehaviour
{
    public Transform target;
    private NavMeshAgent agent;
    private LineRenderer lineRenderer;
    private NavMeshPath path;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        lineRenderer = GetComponent<LineRenderer>();
        path = new NavMeshPath();
    }

    void Update()
    {
        if (target != null)
        {
            // Рассчитываем путь
            NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, path);

            // Проверяем, есть ли путь
            if (path.status == NavMeshPathStatus.PathComplete)
            {
                // Отображаем путь
                DrawPath();
            }
            else
            {
                // Очищаем линию, если пути нет
                lineRenderer.positionCount = 0;
            }
        }
    }

    void DrawPath()
    {
        if (path.corners.Length < 2)
            return;

        lineRenderer.positionCount = path.corners.Length;
        lineRenderer.SetPositions(path.corners);
    }
}