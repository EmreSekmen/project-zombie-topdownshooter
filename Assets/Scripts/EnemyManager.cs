using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public float speed = 3;
    public float stopDistance = 0.6f;
    public float detectionRadius = 10f;
    private Vector2 originalScale;
    public Transform player;
    Rigidbody2D rb;
    Animator anim;


    public float health = 2;
    public bool isKnocbacked = false;
    public float knockBackDuration = 0.2f;

    private bool facingRightInitially;

    PlayerControls control;
    private Vector2 moveInputGamepad;

    private SpawnManager spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        spawnManager = FindAnyObjectByType<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    private void Awake()
    {

        rb = GetComponent<Rigidbody2D>();
        var p = GameObject.FindWithTag("Player");
        anim = GetComponent<Animator>();

        if (p != null) player = p.transform;

        originalScale = transform.localScale;
        facingRightInitially = originalScale.x > 0;

        control = new PlayerControls();
        control.Player.Move.performed += ctx => moveInputGamepad = ctx.ReadValue<Vector2>();
        control.Player.Move.canceled += ctx => moveInputGamepad = Vector2.zero;
    }

    private void FixedUpdate()
    {
        if (player == null) return;

        if (isKnocbacked)
        {
            return;
        }



        Vector2 toPlayer = (Vector2)(player.position - transform.position);

        float distSqr = toPlayer.sqrMagnitude;

        if(distSqr > detectionRadius * detectionRadius)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        if(distSqr <= stopDistance * stopDistance)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        Vector2 dir = toPlayer.normalized;
        rb.velocity = dir * speed;

        bool isMoving = rb.velocity.sqrMagnitude > 0.001f;
       


        if (facingRightInitially)
        {
            if (dir.x > 0.1f)
                transform.localScale = new Vector2(Mathf.Abs(originalScale.x), originalScale.y);
            else if (dir.x < -0.1f)
                transform.localScale = new Vector2(-Mathf.Abs(originalScale.x), originalScale.y);
        }
        

        float facing = Mathf.Sign(transform.localScale.x);
        anim.SetBool("isMoving", isMoving);
        anim.SetFloat("MoveX", dir.x * facing);
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        

        if (collision.CompareTag("Bullet"))
        {

            StartCoroutine(KnockbackCouroutine());

            health--;


            Destroy(collision.gameObject);

            if (health <= 0)
            {
                spawnManager.OnEnemyKilled();
                Destroy(gameObject);
            }

        }


        if (collision.CompareTag("Player"))
        {
            Transform player = collision.transform;

            bool playerRight = player.transform.position.x > transform.position.x;


            if (playerRight)
            {
                transform.localScale = new Vector2(Mathf.Abs(originalScale.x), originalScale.y);
            }
            else
            {
                transform.localScale = new Vector2(-Mathf.Abs(originalScale.x), originalScale.y);
            }


            if (playerRight)
            {
                anim.SetTrigger("ZombieAttackLeft");
            }
            else
            {
                anim.SetTrigger("ZombieAttackRight");
            }


            Debug.Log("Attack tetiklendi! Player saðda mý? = " + playerRight);


        }






    }

    IEnumerator KnockbackCouroutine()
    {
        isKnocbacked = true;
        Vector2 knockBackDir = (transform.position - player.position).normalized;
        rb.velocity = knockBackDir * 6;
        yield return new WaitForSeconds(knockBackDuration);
        isKnocbacked = false;
    }


    private void OnEnable()
    {
        control.Enable();
    }

    private void OnDisable()
    {
        control.Disable();
    }


}
