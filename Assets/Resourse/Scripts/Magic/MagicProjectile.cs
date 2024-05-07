using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicProjectile : MonoBehaviour
{
    public GameObject explosionPrefabs;
    public int damage = 2;

    void Start()
    {
        Destroy(gameObject, 5f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
                Instantiate(explosionPrefabs, transform.position, Quaternion.identity); // instantiate a explode animation
                Destroy(gameObject);
            }
        }
        if (collision.gameObject.CompareTag("Ice"))
        {
            Instantiate(explosionPrefabs, transform.position, Quaternion.identity); // instantiate a explode animation
            Destroy(gameObject);
        }
    }
}

