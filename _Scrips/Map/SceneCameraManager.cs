using UnityEngine;
using Cinemachine;

public class SceneCameraManager : MonoBehaviour
{
    private CinemachineVirtualCamera cinemachineCamera;

    private void Start()
    {
        cinemachineCamera = FindObjectOfType<CinemachineVirtualCamera>();

        if (cinemachineCamera == null)
        {
            GameObject cam = new GameObject("Cinemachine Camera");
            cinemachineCamera = cam.AddComponent<CinemachineVirtualCamera>();
            cinemachineCamera.Priority = 10; // Đảm bảo nó là camera chính
        }
    }
}
