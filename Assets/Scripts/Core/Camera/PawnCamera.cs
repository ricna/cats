using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

namespace Unrez.BackyardShowdown
{
    public class PawnCamera : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private Camera _mainCamera;
        [SerializeField]
        private CinemachineCamera _cinemachineCamera;

        private Coroutine _coroutineOrthoSizeTween;

        private void Awake()
        {
            _mainCamera = GetComponentInChildren<Camera>();
            _cinemachineCamera = GetComponentInChildren<CinemachineCamera>();
            _mainCamera.enabled = false;
            _cinemachineCamera.enabled = false;
        }

        public void SetupCamera(GameObject toLook, GameObject toFollow)
        {
            _mainCamera.enabled = true;
            _cinemachineCamera.enabled = true;
            _cinemachineCamera.LookAt = toLook.transform;
            _cinemachineCamera.Follow = toFollow.transform;
            StartCoroutine(RestartConfiner());

        }

        private IEnumerator RestartConfiner()
        {
            CinemachineConfiner2D confiner = _cinemachineCamera.GetComponent<CinemachineConfiner2D>();
            confiner.enabled = false;
            yield return new WaitForSeconds(1f);
            confiner.enabled = true;
            confiner.InvalidateLensCache();
            confiner.InvalidateBoundingShapeCache();
        }

        public Camera GetCamera()
        {
            return _mainCamera;
        }

        public void SetOrthoSize(float size, float duration = 0)
        {
            if (duration == 0)
            {
                _cinemachineCamera.Lens.OrthographicSize = size;
                return;
            }
            if (_coroutineOrthoSizeTween != null)
            {
                StopCoroutine(_coroutineOrthoSizeTween);
            }
            _coroutineOrthoSizeTween = StartCoroutine(TweenToOrthoSize(size, duration));
        }

        private IEnumerator TweenToOrthoSize(float size, float duration)
        {
            float currentSize = _cinemachineCamera.Lens.OrthographicSize;
            float t = 0;
            while (t < 1)
            {
                _cinemachineCamera.Lens.OrthographicSize = Mathf.Lerp(currentSize, size, t);
                t += Time.deltaTime / duration;
                yield return null;
            }
            _cinemachineCamera.Lens.OrthographicSize = size;
        }
    }
}