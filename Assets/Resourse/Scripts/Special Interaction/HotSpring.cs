using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotSpring : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(HealPlayer(collision.gameObject));
            TemperatureManager.Instance.SetTemperature(0f, TemperatureSource.Zone); //change the temerature
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StopAllCoroutines();  // stop coroutines
            TemperatureManager.Instance.ResetTemperature(TemperatureSource.Zone); // restore the temerature

        }
    }

    private IEnumerator HealPlayer(GameObject player)
    {
        HealthSystem healthSystem = player.GetComponent<HealthSystem>();
        healthSystem.ChangeHeart(1f);  // heal the player
        yield return new WaitForSeconds(2f);  // wait 2s
        
    }
}
