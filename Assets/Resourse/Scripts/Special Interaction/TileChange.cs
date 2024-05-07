using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileChange : MonoBehaviour
{
    public GameObject newTilePrefab; // the tile after changing

    private void Start()
    {
        newTilePrefab.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "IceBall") // IceBall frozen the tile
        {
            if (!newTilePrefab.activeInHierarchy)
            {
                newTilePrefab.transform.position = transform.position; 
                newTilePrefab.SetActive(true);
            }
            gameObject.SetActive(false);
        }
    }
}
