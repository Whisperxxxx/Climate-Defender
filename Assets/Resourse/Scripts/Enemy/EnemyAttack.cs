using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private GameObject target;  // store the reference to the target that will receive damage
   

    public Transform patrolPointA;
    public Transform patrolPointB;
    private Transform currentTarget;
    public float patrolSpeed = 2f; // set the patrol speed
    private float currentSpeed = 0f;
    public float attackRange = 1.0f;
    private float attackCooldown = 2f; 
    private float lastAttackTime; // record the last attack time
    private bool isAttack = false; 
    public bool isDead = false;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentTarget = patrolPointA; // initial the destination
    }

    void Update()
    {
        if (!isDead && Time.time - lastAttackTime >= attackCooldown) // attack cd
        {
            Attack();
        }
        if (!isAttack && !isDead) // if not attacking, continue patrolling
        {
            Patrol();
        }
        SwitchAnim();
        UpdateDirection();
    }

    void Attack()
    {
        // detect the player
        Collider2D player = Physics2D.OverlapCircle(transform.position, attackRange, LayerMask.GetMask("Player")); // Check the player in the Player Layer
        if (player != null && player.CompareTag("Player")) // check if the tag is player
        {
            target = player.gameObject;  // store the player
            isAttack = true;
            lastAttackTime = Time.time;
            return; 
        }

        // detect the sth need to be protected, but the priority lower than the player
        Collider2D beProtected = Physics2D.OverlapCircle(transform.position, attackRange, LayerMask.GetMask("Protected"));
        if (beProtected != null && beProtected.CompareTag("Protected"))
        {
            target = beProtected.gameObject;
            isAttack = true;
            lastAttackTime = Time.time;
            
        }
    }

    // the target take damage
    void TriggerDamage()
    {
        if (target != null)
        {
            HealthSystem healthSystem = target.GetComponent<HealthSystem>(); // get the target health script
            if (healthSystem != null)
            {
                healthSystem.TakeDamage(); // making damage
                isAttack = false;
            }
            else
            {
                isAttack = false;
            }
        }
    }

    void Patrol()
    {
        if (currentTarget != null)
        {
            float step = patrolSpeed * Time.deltaTime; // calculate the step every frame
            Vector2 newPosition = Vector2.MoveTowards(transform.position, new Vector2(currentTarget.position.x, transform.position.y), step);
            currentSpeed = Mathf.Abs(newPosition.x - transform.position.x) / Time.deltaTime; // calculate the current speed
            transform.position = newPosition; // update the position

            // check if arrive the destination
            if (Mathf.Abs(transform.position.x - currentTarget.position.x) < 0.1f)
            {
                // switch the point
                if (currentTarget == patrolPointA)
                {
                    currentTarget = patrolPointB;  
                }
                else
                {
                    currentTarget = patrolPointA;  
                }
            }
        }
    }

    void UpdateDirection()
    {
        if (currentTarget != null)
        {
            // calculate the direction to the current target
            float direction = currentTarget.position.x - transform.position.x;

            // flip the enemy sprite based on the direction of movement to the target
            if (direction != 0)
            {
                transform.localScale = new Vector3(Mathf.Sign(direction), 1, 1);
            }
        }
    }

    public void Knockback(float force)
    {
        if (rb != null)
        {
            Vector2 knockbackDirection = new Vector2(-Mathf.Sign(transform.localScale.x) * force, force * 0.3f);
            rb.AddForce(knockbackDirection, ForceMode2D.Impulse);
        }
    }


    void SwitchAnim()
    {
        anim.SetFloat("running", currentSpeed);
        anim.SetBool("attacking", isAttack);
        if (isDead)
        {
            anim.SetTrigger("death");
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
