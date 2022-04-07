using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour {
    public int points = 0;
    public int life = 3;
    
    //private bool isGrounded;
    private bool isStriking;
    private bool isHurt;
    private bool isDead;

    private Rigidbody2D rb;
    private Animator anim;
    private BoxCollider2D coll;
    
    private float moveSpeed;
    private float jumpSpeed;
    private float dirX;

    private float initDeathTime;

    [SerializeField] private LayerMask jumpableGround;
    private enum MovementState {idle, running, jumping, falling, strike, hurt, dead};
    
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();

        initLevel();
    }

    void Update() {
        if (!isDead && !isHurt) {
            dirX = Input.GetAxisRaw("Horizontal");
        
            rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
        
            if (Input.GetKeyDown("space") && IsGrounded()) {
                GetComponent<Rigidbody2D>().velocity = new Vector2(rb.velocity.y, moveSpeed);
            }

            if (Input.GetKeyDown("left ctrl")) {
                isStriking = true;
            }    
        }

        if (transform.position.y < -15) {
            if (!isDead)
                killPlayer();
        }

        if (isDead) {
            resetLevel();
        }
        
        UpdateAnimationState();
    }

    void initLevel() {
        rb.bodyType = RigidbodyType2D.Dynamic;
        Time.timeScale = 1;
        isStriking = isHurt = isDead = false;
        points = 0;
        life = 1;
        moveSpeed = 7f;
        jumpSpeed = 7f;
        transform.position = GameObject.Find("SpawnPoint").transform.position;
    }

    void resetLevel() {
        if (Time.realtimeSinceStartup > initDeathTime + 3f) {
            initLevel();
            SceneManager.LoadScene(1);    
        }
    }
    
    private void UpdateAnimationState() { 
        MovementState state = 0;
        
        if (IsGrounded()) {
            if (dirX > 0f) {
                state = MovementState.running;
                transform.localScale = new Vector3(1, 1, 1);
                //spr.flipX = false;
            } else if (dirX < 0f) {
                state = MovementState.running;
                //spr.flipX = true;
                transform.localScale = new Vector3(-1, 1, 1);
            } else {
                state = MovementState.idle;
            }
        } else {
            if (dirX > 0f) {
                transform.localScale = new Vector3(1, 1, 1);
            } else if (dirX < 0f) {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            if (rb.velocity.y > .1f) {
                state = MovementState.jumping;
            } else {
                state = MovementState.falling;
            }
        }
            
        if (isStriking) {
            state = MovementState.strike;
        }
        anim.SetInteger("state", (int) state);
    }

    private bool IsGrounded() {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
    
    void HurtAnimIsDone() {
        isHurt = false;
    }
    
    void StrikeAnimIsDone() {
        isStriking = false;
    }
    
    void death() {
        Time.timeScale = 0;
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.CompareTag("CollectGold")) {
            points = points + 100;
            Destroy(col.gameObject);
        }
        
        if (col.gameObject.CompareTag("CollectHealth")) {
            if (life < 3) 
                life+=1;
            Destroy(col.gameObject);
        }

        if (col.gameObject.CompareTag("Trap")) {
            if (life > 1) {
                life -= 1;
                isHurt = true;
                rb.AddForce(transform.up * 5f, ForceMode2D.Impulse);
                anim.SetTrigger("hurt");
            } else
            {
                if (!isDead)
                    killPlayer();
            }
        }
    }

    void killPlayer() {
        isDead = true;
        anim.SetInteger("state", 6);
        anim.SetTrigger("death");
        rb.bodyType = RigidbodyType2D.Static;
        initDeathTime = Time.realtimeSinceStartup;
    }
}