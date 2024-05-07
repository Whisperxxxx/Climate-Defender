using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectedHealth : MonoBehaviour
{
    private int currentHealth = 3;

    void Start()
    {
    }
    public void TakeDamage()
    {
        currentHealth -= 1;
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }

    }

}
