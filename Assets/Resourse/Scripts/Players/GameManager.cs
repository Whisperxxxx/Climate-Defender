using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;


public class GameManager : MonoBehaviour
{
    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // get the cineamchine
        CinemachineVirtualCamera cineCamera = FindObjectOfType<CinemachineVirtualCamera>();
        if (cineCamera != null)
        {
            // get the player 
            PlayerController player = FindObjectOfType<PlayerController>();

            if (player != null)
            {
                cineCamera.Follow = player.transform;
                player.MoveToSpawnPoint();
            }

        }
    }
}
