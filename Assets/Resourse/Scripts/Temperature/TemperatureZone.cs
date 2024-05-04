using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemperatureZone : MonoBehaviour
{
    // change the temperature
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))  
        {
            TemperatureManager.Instance.SetTemperature(0f, TemperatureSource.Zone); 
        }
    }
    // restore the temperature
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            TemperatureManager.Instance.ResetTemperature(TemperatureSource.Zone);
        }
    }
}