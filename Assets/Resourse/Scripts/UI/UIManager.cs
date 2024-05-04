using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [System.Serializable] // make the Class Heart can be edit and visual
    public class Heart
    {
        public GameObject leftHalf;
        public GameObject rightHalf;
    }

    public Heart[] hearts; // there are multiple hearts
    public Slider temperatureSlider; // thermometer
    private float transitionSpeed = 0.8f; // the speed of the slider changes


    void Update()
    {
        if (TemperatureManager.Instance != null)
        {
            float targetTemperature = TemperatureManager.Instance.CurrentTemperature;
            // use Lerp to change the value, in order to get a smooth anim
            temperatureSlider.value = Mathf.Lerp(temperatureSlider.value, targetTemperature, transitionSpeed * Time.deltaTime);
        }
    }
    // update the icon of the hearts
    public void UpdateHearts(float currentHearts)
    {
        // calculate if there is the half of heart.
        int fullHearts = Mathf.FloorToInt(currentHearts);
        float partialHeart = currentHearts - fullHearts;

        // iterate each heart icon
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < fullHearts)
            {
                // show the entire hearts
                hearts[i].leftHalf.SetActive(true);
                hearts[i].rightHalf.SetActive(true);
            }
            else if (i == fullHearts && partialHeart >= 0.5f)
            {
                // show the half of the heart
                hearts[i].leftHalf.SetActive(true);
                hearts[i].rightHalf.SetActive(false);
            }
            else
            {
                // hide the entire hearts
                hearts[i].leftHalf.SetActive(false);
                hearts[i].rightHalf.SetActive(false);
            }
        }
    }
}
