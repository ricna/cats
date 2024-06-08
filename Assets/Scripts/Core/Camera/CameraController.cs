
using Unity.Cinemachine;
using UnityEngine;

namespace Unrez.Cats
{
    public class CameraController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private Camera cam;
        [SerializeField]
        private CinemachineCamera cinemachineCamera;

        private void Start()
        {

        }

        public void SetupCamera(GameObject toLook, GameObject toFollow)
        {
            cinemachineCamera.enabled = true;
            cam.enabled = true;
            cinemachineCamera.LookAt = toLook.transform;
            cinemachineCamera.Follow = toFollow.transform;
        }

        public Camera GetCamera()
        {
            return cam;
        }
    }
}