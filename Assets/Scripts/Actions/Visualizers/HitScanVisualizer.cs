using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Actions.Visualizers
{
    public class HitScanVisualizer : MonoBehaviour, ActionVisualizer
    {
        private LineRenderer _lineRenderer;
        private bool _visualizerEnabled;

        private float _maxDistance;
        private Vector3 _startPoint;

        private Dictionary<DecalProjector, float> _decalProjectors = new();

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _visualizerEnabled = false;
        }

        public void EnableVisualizerFor(GameObject activeChar, ActionInstance actionInstanceParam)
        {
            Debug.Log("Hitscan visualizer" + " is enabled");
            _visualizerEnabled = true;
            _maxDistance = actionInstanceParam.CurrentDistance;
            _lineRenderer.enabled = true;
            _startPoint = new Vector3(
                activeChar.transform.position.x,
                activeChar.transform.position.y + 2.5f,
                activeChar.transform.position.z
            );
        }

        public void DisableVisualizer()
        {
            _visualizerEnabled = false;
            Debug.Log("Hitscan visualizer" + " is disabled5123124");
            _lineRenderer.enabled = false;
        }


        private void Update()
        {
            var decalCopy = new Dictionary<DecalProjector, float>(_decalProjectors);
            foreach (var decalProjector in decalCopy)
            {
                if (Time.time - decalProjector.Value > 1.5f)
                {
                    decalProjector.Key.enabled = false;
                }
            }


            if (!_visualizerEnabled)
                return;

            // print("enabled????");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            int layerMask = (1 << LayerMask.NameToLayer("Default")) | (1 << LayerMask.NameToLayer("BattleFieldLayer"));
            if (Physics.Raycast(ray, out RaycastHit hit, 200f, layerMask))
            {
                Vector3 direction = hit.point - _startPoint;
                float distance = direction.magnitude;

                Vector3 endPosition;

                if (distance <= _maxDistance)
                {
                    endPosition = hit.point;
                    _lineRenderer.startColor = Color.green;
                    _lineRenderer.endColor = Color.green;
                }
                else
                {
                    // Если расстояние больше максимальной дистанции
                    endPosition = _startPoint + direction.normalized * _maxDistance;
                    _lineRenderer.startColor = Color.red;
                    _lineRenderer.endColor = Color.red;
                }

                _lineRenderer.SetPosition(0, _startPoint);
                _lineRenderer.SetPosition(1, endPosition);

                if (hit.collider.gameObject.CompareTag("Enemy"))
                {
                    var decalProjector = hit.collider.gameObject.GetComponentInChildren<DecalProjector>();
                    if (decalProjector != null && _lineRenderer.startColor != Color.red)
                    {
                        decalProjector.enabled = true;
                        _decalProjectors.Add(decalProjector, Time.time);
                    }
                }
            }

            // Vector3 cursorWorldPosition = ray.GetPoint(_maxDistance);
            //
            // cursorWorldPosition = new Vector3(cursorWorldPosition.x, 0, cursorWorldPosition.z);
            //
        }
    }
}