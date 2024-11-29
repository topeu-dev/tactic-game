using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Utility
{
    public class NavMeshHelper
    {
        public static Vector3[] GetPathCornersWithinDistance(NavMeshPath _path, float maxDistance)
        {
            if (_path == null || _path.corners == null || _path.corners.Length == 0)
            {
                return Array.Empty<Vector3>();
            }

            List<Vector3> pointsWithinDistance = new List<Vector3>();
            float accumulatedDistance = 0f;

            // Начинаем с первой точки в corners
            pointsWithinDistance.Add(_path.corners[0]);

            // Проходим по всем углам, начиная с первого
            for (int i = 1; i < _path.corners.Length; i++)
            {
                float segmentDistance = Vector3.Distance(_path.corners[i - 1], _path.corners[i]);
                if (accumulatedDistance + segmentDistance > maxDistance)
                {
                    // Если следующий сегмент превышает максимальную дистанцию, добавим точку на границе доступной дистанции
                    float remainingDistance = maxDistance - accumulatedDistance;
                    Vector3 direction = (_path.corners[i] - _path.corners[i - 1]).normalized;
                    Vector3 newPoint = _path.corners[i - 1] + direction * remainingDistance;
                    pointsWithinDistance.Add(newPoint);
                    break;
                }

                accumulatedDistance += segmentDistance;
                pointsWithinDistance.Add(_path.corners[i]);
            }

            return pointsWithinDistance.ToArray();
        }


        public static Vector3[] GetPathCornersBeyondDistance(NavMeshPath _path, float maxDistance)
        {
            if (_path == null || _path.corners == null || _path.corners.Length == 0)
            {
                return Array.Empty<Vector3>();
            }

            List<Vector3> pointsBeyondDistance = new List<Vector3>();
            float accumulatedDistance = 0f;

            // Проходим по всем углам пути
            for (int i = 1; i < _path.corners.Length; i++)
            {
                float segmentDistance = Vector3.Distance(_path.corners[i - 1], _path.corners[i]);

                if (accumulatedDistance + segmentDistance > maxDistance)
                {
                    // Если максимальная дистанция превышена, добавляем точку, которая находится на границе
                    if (accumulatedDistance < maxDistance)
                    {
                        float remainingDistance = maxDistance - accumulatedDistance;
                        Vector3 direction = (_path.corners[i] - _path.corners[i - 1]).normalized;
                        Vector3 newPoint = _path.corners[i - 1] + direction * remainingDistance;

                        pointsBeyondDistance.Add(newPoint);
                    }

                    // Добавляем все оставшиеся точки, начиная с текущей
                    for (int j = i; j < _path.corners.Length; j++)
                    {
                        pointsBeyondDistance.Add(_path.corners[j]);
                    }

                    break;
                }

                accumulatedDistance += segmentDistance;
            }

            return pointsBeyondDistance.ToArray();
        }
    }
}