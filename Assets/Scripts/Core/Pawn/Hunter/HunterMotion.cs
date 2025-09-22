using System.Collections.Generic;
using UnityEngine;

namespace Unrez.BackyardShowdown
{
    public class HunterMotion : PawnMotion
    {
        [Header("Settings")]
        [SerializeField]
        private GameObject _segmentPrefab;
        [SerializeField]
        private int _numberOfSegments = 5;
        [SerializeField]
        private float _segmentSpacing = 0.5f;

        private List<Vector3> _positionHistory = new List<Vector3>();
        private List<Transform> _segments = new List<Transform>();
        private Transform _parentBody;
        private Transform _tail;

        protected override void Awake()
        {
            base.Awake();

            _parentBody = _spriteRenderBody.transform;
            _tail = _pawn.GetTail().transform;
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if (_segments.Count == 0)
            {
                GenerateSegments();
            }

            // Inicializa o histórico com corpo já esticado
            for (int i = 0; i < (_numberOfSegments + 1) * 10; i++)
            {
                _positionHistory.Add(_parentBody.position - _parentBody.right * _segmentSpacing * 0.1f * i);
            }
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

            foreach (Transform segment in _segments)
            {
                if (segment != null)
                {
                    Destroy(segment.gameObject);
                }
            }
            _segments.Clear();
            _positionHistory.Clear();
        }

        private void GenerateSegments()
        {
            if (_segmentPrefab == null)
            {
                Debug.LogError("Segment Prefab is not assigned on HunterMotion!");
                return;
            }

            for (int i = 0; i < _numberOfSegments; i++)
            {
                Vector3 initialPosition = _parentBody.position - _parentBody.right * _segmentSpacing * (i + 1);
                GameObject newSegment = Instantiate(_segmentPrefab, initialPosition, Quaternion.identity, transform);
                _segments.Add(newSegment.transform);
            }
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            if (_isMoving)
            {
                Vector3 currentPos = _parentBody.position;
                _positionHistory.Insert(0, currentPos);

                // Mantém o histórico pelo comprimento acumulado
                float totalLength = 0f;
                for (int i = 0; i < _positionHistory.Count - 1; i++)
                {
                    totalLength += Vector3.Distance(_positionHistory[i], _positionHistory[i + 1]);
                    if (totalLength > _numberOfSegments * _segmentSpacing * 3f)
                    {
                        _positionHistory.RemoveRange(i + 1, _positionHistory.Count - (i + 1));
                        break;
                    }
                }

                if (_segments.Count > 0)
                {
                    UpdateSegmentPositions();
                }
            }
        }

        private void UpdateSegmentPositions()
        {
            float distanceBetween = _segmentSpacing;

            for (int i = 0; i < _segments.Count; i++)
            {
                float targetDistance = distanceBetween * (i + 1);
                Vector3 targetPos = GetPointAtDistance(targetDistance);

                // Movimento suave
                _segments[i].position = Vector3.Lerp(_segments[i].position, targetPos, 0.5f);

                // Rotação suave
                Vector3 dir = (targetPos - GetPointAtDistance(targetDistance - 0.01f)).normalized;
                if (dir.sqrMagnitude > 0.0001f)
                {
                    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                    _segments[i].rotation = Quaternion.Lerp(
                        _segments[i].rotation,
                        Quaternion.Euler(0, 0, angle),
                        0.5f
                    );
                }
            }

            // Cauda segue o último segmento
            if (_tail != null && _segments.Count > 0)
            {
                float tailDistance = distanceBetween * (_segments.Count + 1);
                Vector3 tailPos = GetPointAtDistance(tailDistance);
                _tail.position = Vector3.Lerp(_tail.position, tailPos, 0.5f);

                Vector3 dir = (tailPos - GetPointAtDistance(tailDistance - 0.01f)).normalized;
                if (dir.sqrMagnitude > 0.0001f)
                {
                    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                    _tail.rotation = Quaternion.Lerp(
                        _tail.rotation,
                        Quaternion.Euler(0, 0, angle),
                        0.5f
                    );
                }
            }
        }

        private Vector3 GetPointAtDistance(float distance)
        {
            if (_positionHistory.Count < 2)
            {
                return _parentBody.position;
            }

            float accumulated = 0f;
            for (int i = 0; i < _positionHistory.Count - 1; i++)
            {
                Vector3 p1 = _positionHistory[i];
                Vector3 p2 = _positionHistory[i + 1];
                float segmentDist = Vector3.Distance(p1, p2);

                if (accumulated + segmentDist >= distance)
                {
                    float t = (distance - accumulated) / segmentDist;
                    return Vector3.Lerp(p1, p2, t);
                }

                accumulated += segmentDist;
            }

            return _positionHistory[_positionHistory.Count - 1];
        }
    }
}
