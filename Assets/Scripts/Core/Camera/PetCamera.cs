
using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

namespace Unrez.Pets.Cats
{
    public class PetCamera : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private Camera _mainCamera;
        [SerializeField]
        private CinemachineCamera _cinemachineCamera;

        private void Awake()
        {
            _mainCamera = GetComponentInChildren<Camera>();
            _cinemachineCamera = GetComponentInChildren<CinemachineCamera>();
        }
        public void SetupCamera(GameObject toLook, GameObject toFollow)
        {
            _cinemachineCamera.enabled = true;
            _mainCamera.enabled = true;
            _cinemachineCamera.LookAt = toLook.transform;
            _cinemachineCamera.Follow = toFollow.transform;
        }

        public Camera GetCamera()
        {
            return _mainCamera;
        }

        private Coroutine _coroutineOrthoSizeTween;
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