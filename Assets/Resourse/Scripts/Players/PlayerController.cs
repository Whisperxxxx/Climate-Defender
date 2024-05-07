using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance; // singleton


    [SerializeField]
    float jumpForce = 9;
    [SerializeField]
    float speed = 4;
    [SerializeField]
    LayerMask ground;
    [SerializeField] 
    Transform magicSpawnPoint; // the position of magic 

    public GameObject[] availableMagics; // the list that the player available
    public List<GameObject> magicPrefabs; // store the magic
    private GameObject target;  // store the reference to the target that will receive damage
    private Rigidbody2D rb;
    private Animator anim;
    private bool isGround;
    private bool isJump;
    public bool isHurt;
    private bool isAttack;
    private bool isJumpAttack;
    private bool isMagic;
    public bool isDead = false;
    private bool jumpPressed;
    public int jumpCount;
    public Vector2 bottomOffset; // position for the ground check collision
    private float collisionRadius = 0.5f; // radius for the ground check collision

    public float attackRange = 1.0f; // the range of attack
    private float magicCooldown = 5f;  // the cooldown of the magic
    private float lastMagicTime = -5f;  // record last time magic was used
    public int selectedMagicIndex = 0; // the index of the current selected magic
    public MagicUI ui;

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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }


    void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        SwitchAnim();
        isGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius, ground);    // check if the player is on the ground
        if (!isDead && !isAttack)
        {
            GroundMovement();
        }
        // can't movement when dead or attack
        else if (isDead || isAttack)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    void HandleInput()
    {
        if (!isDead)
        {
            Jump();
            if (Input.GetButtonDown("Jump") && jumpCount > 0)
            {
                jumpPressed = true;
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                Attack();
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                Magic();
            }

            // choose the magic
            if (Input.GetKeyDown(KeyCode.Alpha1) && magicPrefabs.Count > 0)
            {
                selectedMagicIndex = 0;
                ui.UpdateMagicIcon(selectedMagicIndex);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) && magicPrefabs.Count > 1)
            {
                selectedMagicIndex = 1;
                ui.UpdateMagicIcon(selectedMagicIndex);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3) && magicPrefabs.Count > 2)
            {
                selectedMagicIndex = 2;
                ui.UpdateMagicIcon(selectedMagicIndex);
            }
        }
    }
    void GroundMovement()
    {
        // movement
        float horizontalMove = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(horizontalMove * speed, rb.velocity.y);

        // flip player sprite based on the direction of movement
        if (horizontalMove != 0)
        {
            transform.localScale = new Vector3(horizontalMove, 1, 1);
        }
    }

    void Jump()
    {
        if (isGround)
        {
            jumpCount = 2; // reset the jump count to 2, allowing for double jump
            isJump = false; // reset the jump state
        }
        // handle the initial jump from the ground
        if (jumpPressed && isGround)
        {
            isJump = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
            jumpPressed = false;
        }
        // handle the second jump
        else if (jumpPressed && jumpCount > 0 && isJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
            jumpPressed = false;
        }
        if (!isGround)
        {
            // if the player is not on the ground, perform a second jump directly
            if (jumpPressed)
            {
                isJump = true;
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpCount -= 2;
                jumpPressed = false;
            }
            else if (jumpPressed && jumpCount > 0 && isJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpCount--;
                jumpPressed = false;
            }
        }
    }

    void Attack()
    {
        // distinguishing between attack and jump attack is because jump attack is movable while attack is not
        if (isGround)
        {
            isAttack = true;
        }
        else if (rb.velocity.y > 0 || rb.velocity.y < 0)
        {
            isJumpAttack = true;
        }
        // make sure the range can totate the direction
        Vector3 attackCenter = transform.position + new Vector3(attackRange * transform.localScale.x, 0, 0);

        Collider2D[] hitEnemy = Physics2D.OverlapCircleAll(attackCenter, attackRange, LayerMask.GetMask("Enemy")); // check the enemies in the range
        foreach (Collider2D enemy in hitEnemy) // iterate every enemy
        {
            if (enemy.gameObject.CompareTag("Enemy")) // check the tag is Enemy
            {
                target = enemy.gameObject;  // store the enemy
            }
        }
    }

    // the target take damage
    void TriggerDamage()
    {
        isAttack = false;
        isJumpAttack = false;

        if (target != null)
        {
            EnemyHealth enemyHealth = target.GetComponent<EnemyHealth>(); // get the target health script
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(1); // making damage
                target = null;
            }
        }
    }

    public void AddMagic(int magicIndex)
    {   // the magic index must smaller than the number of magicPrefabs and it must , and only add the magic whene the magic prefabs don't contain it
        {
            if (magicIndex < availableMagics.Length && !magicPrefabs.Contains(availableMagics[magicIndex]))
            {
                magicPrefabs.Add(availableMagics[magicIndex]); // add the corresponding magic to the magicPrefabs
            }
        }
    }

    void Magic()
    {

        if (selectedMagicIndex < magicPrefabs.Count) // make sure there is the correct index
        {
            if (Time.time - lastMagicTime >= magicCooldown) // the time passed must be greater than cd
            {
                lastMagicTime = Time.time;  // update the casting time
                Vector2 magicDirection = new Vector2(transform.localScale.x, 0); // dafult casting direction is horizontal
                Quaternion rotation = Quaternion.identity;
                Vector3 scale = new Vector3(transform.localScale.x, 1, 1); // set a normal scale, because
                if (Input.GetKey(KeyCode.W)) 
                {
                    magicDirection = new Vector2(0, 1);
                    rotation = Quaternion.Euler(0, 0, 90); // rotate 90 angle
                    scale = Vector3.one; // make sure all face right before rotate
                }
                else if (Input.GetKey(KeyCode.S)) 
                {
                    magicDirection = new Vector2(0, -1);
                    rotation = Quaternion.Euler(0, 0, -90); // rotate -90 angle
                    scale = Vector3.one; // make sure all face right before rotate
                }

                GameObject magic = Instantiate(magicPrefabs[selectedMagicIndex], magicSpawnPoint.position, rotation); // generate magic Prafabs
                magic.GetComponent<Rigidbody2D>().velocity = magicDirection * 10f; // give magic a force
                magic.transform.localScale = scale; // ensure that the direction is the one the player is facing
                ui.StartCountDown();
                isMagic = true;
                StartCoroutine(MagicCooldown()); //cd
            }
            else
            {
                // the audio of fail to use
                Debug.Log("The magic is not ready yet");
            }
        }
    }

    // wait cd
    IEnumerator MagicCooldown()
    {
        yield return new WaitForSeconds(magicCooldown);
    }

    void SwitchAnim()
    {
        // update animation parameters
        anim.SetFloat("running", Mathf.Abs(rb.velocity.x));
        if (isGround)
        {
            anim.SetBool("falling", false);
        }
        else if (!isGround && rb.velocity.y > 0)
        {
            anim.SetBool("jumping", true);
        }
        else if (rb.velocity.y < 0)
        {
            anim.SetBool("jumping", false);
            anim.SetBool("falling", true);
        }
        if (isAttack || isJumpAttack)
        {
            anim.SetBool("attacking", true);
        }
        else
        {
            anim.SetBool("attacking", false);
        }
        if (isMagic)
        {
            anim.SetBool("magic", true);
        }
        else
        {
            anim.SetBool("magic", false);
        }
        if (isHurt)
        {
            anim.SetBool("hurt", true);

        }
        else
        {
            anim.SetBool("hurt", false);
        }

        if (isDead)
        {
            anim.SetTrigger("death");
        }
    }



    public void MoveToSpawnPoint()
    {
        GameObject spawnPoint = GameObject.FindWithTag("SpawnPoint");
        if (spawnPoint != null)
        {
            transform.position = spawnPoint.transform.position;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("DeathLine"))
        {
            GameManager.Instance.ReloadCurrentScene();

        }
    }


    public void OverHurt()
    {
        isHurt = false;
    }

    public void OverMagic()
    {
        isMagic = false;
    }

    public float GetMagicCoolDown()
    {
        return magicCooldown;
    }

  
    void OnDrawGizmos()
    {
        // draw a gizmo at the position of the ground check
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, collisionRadius);

        // draw a attack range
        if (attackRange > 0)
        {
            Vector3 attackCenter = transform.position + new Vector3(attackRange * transform.localScale.x, 0, 0);
            Gizmos.DrawWireSphere(attackCenter, attackRange);

        }
    }


}

