using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RPGGameManager : MonoBehaviour
{
    // 1
    public static RPGGameManager sharedInstance = null;
    public SpawnPoint playerSpawnPoint;
    public RPGCameraManager cameraManager;



    void Awake()
    {
        if (sharedInstance != null && sharedInstance != this)
        {
            // 3
            Destroy(gameObject);
        }
        else
        {
            // 4
            sharedInstance = this;
        }      
    }

    void Update()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }

    void Start()
    {
        // 5
        SetupScene();
    }
    // 6
    public void SetupScene()
    {
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        // 1
        if (playerSpawnPoint != null)
        {
            // 2
            GameObject player = playerSpawnPoint.SpawnObject();
            cameraManager.virtualCamera.Follow = player.transform;

        }
    }
}