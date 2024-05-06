using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private EnemyAttack enemyAttack;
    private int currentHealth = 3;

    void Start()
    {
        enemyAttack = GetComponent<EnemyAttack>();
    }
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        enemyAttack.Knockback(10f); // give a force to let the enemy back when take damage
        if (currentHealth <= 0)
        {
            enemyAttack.isDead = true;
        }

    }

    void Die()
    {
        Destroy(gameObject);
    }
}
