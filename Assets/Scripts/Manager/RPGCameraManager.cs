using UnityEngine;
// 1
using Cinemachine;
public class RPGCameraManager : MonoBehaviour
{
    public static RPGCameraManager sharedInstance = null;
    // 2
    [HideInInspector]
    public CinemachineVirtualCamera virtualCamera;
    // 3
    void Awake()
    {
        if (sharedInstance != null && sharedInstance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            sharedInstance = this;
        }
        // 4
        GameObject vCamGameObject = GameObject.FindWithTag("VirtualCamera");

        //5
        virtualCamera = vCamGameObject.GetComponent<CinemachineVirtualCamera>();
    }
}