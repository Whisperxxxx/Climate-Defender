using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PotionUI : MonoBehaviour
{
    public TextMeshProUGUI usageCountText;
    public Image countDown; // translucent image
    public Potion potion;


    void Start()
    {
        usageCountText.text = potion.GetUsageCount().ToString();
    }

    void Update()
    {
        usageCountText.text = potion.GetUsageCount().ToString();

    }


    // used to be call by the TemperaturePosion Script
    public void StartCountDown()
    {
        StartCoroutine(CountDownRoutine(potion.GetDuration()));

    }

    // make an effect similar to cooldown animation
    private IEnumerator CountDownRoutine(float duration)
    {
        float timeLeft = duration; // initialize timeLeft

        // continue looping until the timer runs out
        while (timeLeft > 0)
        {
            countDown.fillAmount = timeLeft / duration; // update the fill amount of the countdown based on the left time
            timeLeft -= Time.deltaTime;
            yield return null;
        }
        countDown.fillAmount = 0; 
    }
}