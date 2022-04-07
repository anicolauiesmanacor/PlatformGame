using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton_manager : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    private int currentWaypointIndex = 0;
    
    
    [SerializeField] private float speed = 1f; 
    private float dirX;
    
    private Rigidbody2D rb;
    private Animator anim;
    private BoxCollider2D coll;
    
    [SerializeField] private LayerMask jumpableGround;
    
    //STATES
    private enum MovementState {idle, walking, shield, hit, hurt, dead, back};
    int state = 0;
    private bool isStriking;
    private bool isHurt;
    private bool isDead;
    
    void Start() {
        state = 1;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
    }
    
    private void Update() {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.right);
        Debug.DrawRay( transform.position, Vector2.right);
        
//        if (Physics2D.Raycast(hit, ))
        
        if (state == (int) MovementState.walking) {
            if (Vector2.Distance(waypoints[currentWaypointIndex].transform.position, transform.position) < 0.2f) {
                currentWaypointIndex++;
                if (currentWaypointIndex >= waypoints.Length) {
                    currentWaypointIndex = 0;
                }
                transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
            }
            transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypointIndex].transform.position, speed * Time.deltaTime);
        }
        UpdateAnimationState();
    }

    private void UpdateAnimationState() {
        
        anim.SetInteger("state", (int) state);
    }
    
    private bool IsGrounded() {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }

    private void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.CompareTag("Player")) {
            rb.bodyType = RigidbodyType2D.Static;
        }
    }
    
    private void OnCollisionExit2D(Collision2D col) {
        if (col.gameObject.CompareTag("Player")) {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }
}
