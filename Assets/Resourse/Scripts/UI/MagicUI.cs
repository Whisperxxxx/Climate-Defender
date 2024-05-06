using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicUI : MonoBehaviour
{
    public Image magicIcon;
    public Image countDown; // translucent image
    public PlayerController playerController;

    void Update()
    {
        UpdateMagicIcon(playerController.selectedMagicIndex);
    }

    // update the UI to show the correct magic icon
    public void UpdateMagicIcon(int magicIndex)
    {
        if (magicIndex >= 0 && magicIndex < playerController.magicPrefabs.Count) // update the icon only when the magic exists
        {
            SpriteRenderer renderer = playerController.magicPrefabs[magicIndex].GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                magicIcon.sprite = renderer.sprite;  // set the sprite of the magic icon to the sprite of the selected magic
            }
        }
    }

    // used to be call by the PlayerController Script
    public void StartCountDown()
    {
        StartCoroutine(CountDownRoutine(playerController.GetMagicCoolDown()));

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
