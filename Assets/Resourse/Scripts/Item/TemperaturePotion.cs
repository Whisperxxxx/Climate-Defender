using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemperaturePotion : MonoBehaviour, IItem
{
    public static TemperaturePotion Instance { get; private set; }  // singleton

    private float duration = 30f; // Duration of potion effect
    private int usageCount = 2; // initialize the usage count
    public PotionUI ui;
    private bool isWorking = false;  // Track whether the potion effect is working


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  
        }
        else if (Instance != this)
        {
            Destroy(gameObject);  
        }
    }

    public void Use()
    {
        if (isWorking)
        {
            // the audio of fail to use
            Debug.Log("The item is working.");
        }
        else
        {
            // use the potion
            if (usageCount > 0)
            {
                StartCoroutine(Effect());
                usageCount--;
                ui.StartCountDown();
            }
            else
            {
                // the audio of fail to use
                Debug.Log("No more usage count.");
            }
        }
    }

    // force the temperature be 0 when use the potion
    private IEnumerator Effect()
    {
        isWorking = true;
        TemperatureManager.Instance.SetTemperature(0, TemperatureSource.Potion); // change the temperature
        yield return new WaitForSeconds(duration); // cooldown
        TemperatureManager.Instance.ResetTemperature(TemperatureSource.Potion); // restore the temperature
        isWorking = false;
    }

    // add the usage count
    public void AddUse()
    {
        usageCount++;
    }
    // get the usage count
    public int GetUsageCount()
    {
        return usageCount;
    }
    // get the duration of the potion
    public float GetDuration()
    {
        return duration;
    }


}
