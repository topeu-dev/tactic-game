using Actions.TempOutline;
using UnityEngine;

namespace Actions.Visualizers
{
    public class RaycastTargetVisualizer : MonoBehaviour, ActionVisualizer
    {
        private LineRenderer lineRenderer;
        private Camera mainCamera;


        private Vector3 startPoint;
        private float maxDistance = 10f;
        private bool enabled;

        private void Awake()
        {
            lineRenderer = GetComponent<LineRenderer>();
            mainCamera = Camera.main;
        }

        public void EnableVisualizerFor(GameObject activeChar, ActionInstance actionInstanceParam)
        {
            startPoint = new Vector3(
                activeChar.transform.position.x,
                activeChar.transform.position.y + 2.5f,
                activeChar.transform.position.z
                );
            enabled = true;
        }

        private void Update()
        {
            if (!enabled)
                return;
            // Получаем позицию курсора в мировом пространстве
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            Plane plane = new Plane(Vector3.up, startPoint);
            float distanceToPlane;

            if (plane.Raycast(ray, out distanceToPlane))
            {
                Vector3 cursorWorldPosition = ray.GetPoint(distanceToPlane);

                // Вычисляем направление и расстояние от начальной точки до курсора
                Vector3 direction = cursorWorldPosition - startPoint;
                float distance = direction.magnitude;

                // Нормализуем направление
                direction.Normalize();

                Vector3 endPosition;
                
                RaycastHit[] hits = Physics.RaycastAll(startPoint, direction, maxDistance, LayerMask.GetMask("Default"));

                // Сортируем столкновения по расстоянию от начальной точки
                System.Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance));

                // Вы можете обработать столкновения здесь
                foreach (RaycastHit hit in hits)
                {

                    // Например, вы можете пометить объекты
                    hit.collider.gameObject.GetComponent<CircleOutline>().OutlineCircle();
                }
                
                if (distance <= maxDistance)
                {
                    // Если расстояние меньше или равно максимальной дистанции
                    endPosition = cursorWorldPosition;
                    lineRenderer.startColor = Color.green;
                    lineRenderer.endColor = Color.green;
                }
                else
                {
                    // Если расстояние больше максимальной дистанции
                    endPosition = startPoint + direction * maxDistance;
                    lineRenderer.startColor = Color.red;
                    lineRenderer.endColor = Color.red;
                }

                // Обновляем позиции LineRenderer
                lineRenderer.SetPosition(0, startPoint);
                lineRenderer.SetPosition(1, endPosition);
            }
        }

        public void DisableVisualizer()
        {
            enabled = false;
        }
    }
}