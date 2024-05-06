using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : Potion
{
    public static HealthPotion Instance { get; private set; }  // singleton

    public HealthSystem healthSystem;
    private float duration = 30f; // Duration of potion effect
    private int usageCount = 1; // initialize the usage count
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

    public override void Use()
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
        healthSystem.ChangeHeart(1f); // heal 1 heart
        yield return new WaitForSeconds(duration); // cooldown
        isWorking = false;
    }

    // add the usage count
    public void AddUse()
    {
        usageCount++;
    }
    // get the usage count
    public override int GetUsageCount()
    {
        return usageCount;
    }
    // get the duration of the potion
    public override float GetDuration()
    {
        return duration;
    }


}
