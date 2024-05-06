using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DtDestroy : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
