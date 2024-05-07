using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTrigger : MonoBehaviour
{
    public GameObject trap;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            if (trap != null)
            {
                trap.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic; // make the trap falling down

            }
        }
    }
}
