using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float jumpForce = 9;
    [SerializeField]
    float speed = 4;
    [SerializeField]
    LayerMask ground;
    [SerializeField] 
    Transform magicSpawnPoint; // the position of magic 

    public GameObject[] magicPrefabs; // store different magics
    private Rigidbody2D rb;
    private Animator anim;
    public AudioSource jumpAudio, jumpAudio2, runAudio, hurtAudio;
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

    private float magicCooldown = 5f;  // the cooldown of the magic
    private float lastMagicTime = -5f;  // record last time magic was used
    public int selectedMagicIndex = 0; // the index of the current selected magic
    public MagicUI ui;

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
            if (Input.GetKeyDown(KeyCode.Alpha1) && magicPrefabs.Length > 0)
            {
                selectedMagicIndex = 0;
                ui.UpdateMagicIcon(selectedMagicIndex);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) && magicPrefabs.Length > 1)
            {
                selectedMagicIndex = 1;
                ui.UpdateMagicIcon(selectedMagicIndex);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3) && magicPrefabs.Length > 2)
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
    }

    void Magic()
    {

        if (selectedMagicIndex < magicPrefabs.Length) // make sure there is the correct index
        {
            if (Time.time - lastMagicTime >= magicCooldown) // the time passed must be greater than cd
            {
                lastMagicTime = Time.time;  // update the casting time
                GameObject magic = Instantiate(magicPrefabs[selectedMagicIndex], magicSpawnPoint.position, Quaternion.identity); // generate a magic Prefab
                magic.GetComponent<Rigidbody2D>().velocity = new Vector2(10f * transform.localScale.x, 0); // give magic a force
                magic.transform.localScale = new Vector3(transform.localScale.x, 1, 1); // magic direction
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


    private void OnTriggerEnter2D(Collider2D collision)
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (transform.position.x < collision.gameObject.transform.position.x)
            {
                rb.velocity = new Vector2(-5, rb.velocity.y);
                isHurt = true;
            }
            if (transform.position.x > collision.gameObject.transform.position.x)
            {
                rb.velocity = new Vector2(5, rb.velocity.y);
                isHurt = true;
            }
        }
    }

    public void OverHurt()
    {
        isHurt = false;
    }

    public void OverAttack()
    {
        isAttack = false;
        isJumpAttack = false;
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
    }

}

