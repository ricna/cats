using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unrez.Cats
{

    public class FieldOfView : MonoBehaviour
    {
        [SerializeField]
        private float _viewRadius; // Raio do campo de visão
        [Range(0, 360)]
        [SerializeField]
        private float _viewAngle; // Ângulo do campo de visão

        [SerializeField]
        private LayerMask _targetMask; // Máscara para detectar alvos
        [SerializeField]
        private LayerMask _obstacleMask; // Máscara para detectar obstáculos

        [SerializeField]
        private int _resolution; // Resolução do campo de visão

        [SerializeField]
        private LineRenderer _lineRenderer;

        private void Start()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.positionCount = _resolution + 1;
            _lineRenderer.useWorldSpace = true;
        }

        private void Update()
        {
            DrawFieldOfView();
        }

        void DrawFieldOfView()
        {
            float stepAngleSize = _viewAngle / _resolution;
            List<Vector3> points = new List<Vector3>();

            for (int i = 0; i <= _resolution; i++)
            {
                float angle = transform.eulerAngles.z - _viewAngle / 2 + stepAngleSize * i;
                points.Add(transform.position + DirFromAngle(angle, true) * _viewRadius);
            }

            _lineRenderer.SetPositions(points.ToArray());
        }

        public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
        {
            if (!angleIsGlobal)
            {
                angleInDegrees += transform.eulerAngles.z;
            }
            return new Vector3(Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0);
        }
    }

}
