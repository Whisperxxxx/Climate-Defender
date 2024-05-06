using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialog : MonoBehaviour
{
    public int magicIndex; // pass the magic index to player
    public GameObject enterdialog;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>(); // get the Script in order to pass the index
            playerController.AddMagic(magicIndex);
            enterdialog.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            enterdialog.SetActive(false);
        }
    }
}
