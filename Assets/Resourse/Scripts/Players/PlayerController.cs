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
    GameObject[] magicPrefabs; // store different magics
    [SerializeField] 
    Transform magicSpawnPoint; // the position of magic 

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
    public Vector2 bottomOffset;
    private float collisionRadius = 0.5f; // Radius for the ground check collision

    private float magicCooldown = 3f;  // 魔法攻击的冷却时间
    private float lastMagicTime = -3f;  // 记录上次魔法攻击的时间
    private int selectedMagicIndex = 0; // 当前选择的魔法索引

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
        isGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius, ground);    // Check if the player is on the ground
        if (!isDead && !isAttack)
        {
            GroundMovement();
        }
        else if (isDead || isAttack)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    void HandleInput()
    {
        if (Input.GetButtonDown("Jump") && jumpCount > 0)
        {
            jumpPressed = true;
        }
        if (!isDead)
        {
            Jump();
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            Attack();
        }
        // 选择魔法
        if (Input.GetKeyDown(KeyCode.Alpha1)) { selectedMagicIndex = 0; }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { selectedMagicIndex = 1; }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { selectedMagicIndex = 2; }

        // 使用魔法
        if (Input.GetKeyDown(KeyCode.K) && Time.time - lastMagicTime >= magicCooldown)
        {
            Magic();
        }
    }
    void GroundMovement()
    {
        // Movement
        float horizontalMove = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(horizontalMove * speed, rb.velocity.y);

        // Flip player sprite based on the direction of movement
        if (horizontalMove != 0)
        {
            transform.localScale = new Vector3(horizontalMove, 1, 1);
        }
    }

    void Jump()
    {
        if (isGround)
        {
            jumpCount = 2;
            isJump = false;
        }
        if (jumpPressed && isGround)
        {
            isJump = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
            jumpPressed = false;
        }
        else if (jumpPressed && jumpCount > 0 && isJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
            jumpPressed = false;
        }
        if (!isGround)
        {
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
        if (selectedMagicIndex < magicPrefabs.Length)
        {
            lastMagicTime = Time.time;
            GameObject magic = Instantiate(magicPrefabs[selectedMagicIndex], magicSpawnPoint.position, Quaternion.identity);
            magic.GetComponent<Rigidbody2D>().velocity = new Vector2(10f * transform.localScale.x, 0);
            magic.transform.localScale = new Vector3(transform.localScale.x, 1, 1);
            isMagic = true;
            StartCoroutine(MagicCooldown());
        }
    }

    IEnumerator MagicCooldown()
    {
        yield return new WaitForSeconds(magicCooldown);
    }
    void SwitchAnim()
    {
        // Update animation parameters
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

    void OnDrawGizmos()
    {
        // Draw a gizmo at the position of the ground check
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, collisionRadius);
    }

}

