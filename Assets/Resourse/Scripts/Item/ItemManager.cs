using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private TemperaturePotion temperaturePotion; // the temperature potion item

    void Start()
    {
        temperaturePotion = GetComponent<TemperaturePotion>();

    }
    void Awake()
    {
        DontDestroyOnLoad(gameObject); 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            temperaturePotion.Use(); 
        }
    }
}