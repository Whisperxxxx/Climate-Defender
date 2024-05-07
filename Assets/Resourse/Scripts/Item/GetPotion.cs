using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPotion : MonoBehaviour
{
    private bool isPlayerNear = false;  // check if player near the potion

    private void Update()
    {
        // player should near the potion and press E
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            TemperaturePotion.Instance.AddUse();
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isPlayerNear = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isPlayerNear = false;
        }
    }
}
