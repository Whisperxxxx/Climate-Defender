using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // singleton

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        SceneManager.sceneLoaded += OnSceneLoaded; // add event
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // remove event
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // reload current scene
    public void ReloadCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(currentScene.name);
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
