using Cinemachine;
using UnityEngine;

public class CinemachineCamareSetting : MonoBehaviour
{
    public static CinemachineCamareSetting Instance;

    private CinemachineVirtualCamera virtualCamera;
    public GameObject player;
    void Awake()
    {
        Instance = this;
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        virtualCamera.Follow = player.transform;
        virtualCamera.LookAt = player.transform;
    }
}
