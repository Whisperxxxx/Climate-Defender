using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public UIManager uiManager;
    private PlayerController playerController; // get the PlayerController script
    public int maxHearts = 3; 
    public float currentHearts;
    private float comfortTemperature = 0f; 

    void Start()
    {
        currentHearts = maxHearts;  
        StartCoroutine(HeartRecoveryRoutine());
        playerController = GetComponent<PlayerController>();
    }

    // check the temperature every 5s, This determines whether to restore health or deduct health
    IEnumerator HeartRecoveryRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f); // wait 5s
            float temperature = TemperatureManager.Instance.CurrentTemperature;
            float tempDifference = Mathf.Abs(temperature - comfortTemperature);

            if (tempDifference > 3)  // hurt by the temperature difference
            {
                ChangeHeart(-0.5f);
                playerController.isHurt = true;
            }
            else if (tempDifference < 3)  
            {
                yield return new WaitForSeconds(3f);  // every 8s resotre the health
                ChangeHeart(0.5f);
            }
        }
    }

    // update the hearts
    public void ChangeHeart(float change)
    {
        currentHearts += change;
        uiManager.UpdateHearts(currentHearts); // pass the number of current hearts
        if (currentHearts > maxHearts)
        {
            currentHearts = maxHearts;
        }
        else if (currentHearts <= 0)
        {
            currentHearts = 0;
            playerController.isDead = true;
        }
    }

    // be attacked
    public void TakeDamage()
    {
        ChangeHeart(-0.5f);
        playerController.isHurt = true;

    }

}
