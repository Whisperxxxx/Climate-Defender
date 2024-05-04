using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TemperatureManager : MonoBehaviour
{
    // singleton instance of TemperatureManager
    public static TemperatureManager Instance { get; private set; }
    public float CurrentTemperature { get; private set; }
    private TemperatureSource currentSource = TemperatureSource.Default; // set the default temperature priority


    void Awake()
    {
        // check if an instance already exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;  // subscribe the event
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;  // unsubscribe the event
    }

    // be called when a scence is loaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AdjustTemperature(scene.name, TemperatureSource.Scene); // priority is Scene
    }

    // adjusts the temperature based on the scene name
    private void AdjustTemperature(string sceneName, TemperatureSource source)
    {
        switch (sceneName)
        {
            case "Droughts":
                SetTemperature(5f, source);
                break;
            case "Avalanches":
                SetTemperature(-5f, source);
                break;
            default:
                SetTemperature(0f, source);
                break;
        }
    }

    // according to the priority of temperature source to decide the temperature
    // prevent conflicts between different temperature sources
    public void SetTemperature(float temperature, TemperatureSource source)
    {
        if (source >= currentSource)
        {
            CurrentTemperature = temperature;
            currentSource = source;
        }
    }


    public void ResetTemperature(TemperatureSource source)
    {
        if (source == currentSource)
        {
            // initial the currentSource to the default priority when the temperature is reset
            currentSource = TemperatureSource.Default;
            AdjustTemperature(SceneManager.GetActiveScene().name, TemperatureSource.Scene);
        }
    }
}
