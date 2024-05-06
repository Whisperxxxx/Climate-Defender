using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private HealthPotion healthPotion; // the health potion item
    private TemperaturePotion temperaturePotion; // the temperature potion item

    void Start()
    {
        temperaturePotion = GetComponent<TemperaturePotion>();
        healthPotion = GetComponent<HealthPotion>();

    }
    void Awake()
    {
        DontDestroyOnLoad(gameObject); 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            healthPotion.Use(); 
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            temperaturePotion.Use();
        }
    }
}