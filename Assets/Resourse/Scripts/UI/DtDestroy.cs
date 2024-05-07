using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DtDestroy : MonoBehaviour
{
    public static DtDestroy Instance; // singleton

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
}
