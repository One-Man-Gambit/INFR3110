using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [Header("Components")]
    public Animator animator;
    public HealthScript health;
    public Rigidbody rigid;
    public BoxCollider col;

    [Header("Behaviour")]
    public GameObject playerTarget = null;
    public float MoveSpeed = 1.0f;
    public float AttackRange = 1.0f;
    public float AttackPower = 1.0f;

    [Header("Item Dropping")]
    public GameObject CoinPrefab;
    public float DropRate = 0.3f; // 30%

    [Header("State Management")]
    private bool isAttacking;
    private bool isDancing;

    private void Start() 
    {
        health.OnHit += OnHit;
        health.OnKilled += OnDeath;
    }

    private void Update() 
    {
        // Try to find the player if there is no current target
        if (playerTarget == null) {
            playerTarget = GameObject.Find("Player");
            return;
        } 

        // The enemies have one, no need to do anything else, just dance.
        if (isDancing) return;

        // Don't do anything if the enemy is dead.
        if (health.IsDead) return;
        
        // We don't want to do anything else if we're in the middle of an attack either
        if (isAttacking) return;

        // If the player dies, all enemies will enter dance mode
        if (GameManager.GetInstance().pcRef.health.IsDead && !isDancing) {
            animator.SetTrigger("OnDance");
            isDancing = true;
            return;
        }

        // Rotate towards the player target
        transform.LookAt(playerTarget.transform.position);

        // Move towards the player (simple motion)
        rigid.MovePosition(transform.position + transform.forward * MoveSpeed * Time.deltaTime);

        // If within range of the target, try to attack it
        if (Vector3.Distance(transform.position, playerTarget.transform.position) <= AttackRange) 
        {
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack() 
    {
        isAttacking = true;
        animator.SetTrigger("OnAttack");
        yield return new WaitForSeconds(0.5f);

        // Raycast hit in forward direction to detect if damage is dealt
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, AttackRange)) 
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                HealthScript health = hit.collider.gameObject.GetComponent<HealthScript>();
                health.DealDamage(AttackPower);
            }
        }

        yield return new WaitForSeconds(0.5f);
        isAttacking = false;
    }

    private void OnHit() 
    {
        animator.SetTrigger("OnHit");
    }

    private void OnDeath() 
    {
        // Disable Rigidbody and Collider physics
        rigid.useGravity = false;
        col.isTrigger = true;

        // Trigger Animation Events
        animator.SetBool("IsDead", true);
        animator.SetTrigger("OnDeath");

        // Add a point to player kill counter
        GameManager.GetInstance().AddKill();

        // Roll for chance to randomly drop a coin
        float rand = Random.Range(0.0f, 1.0f);
        if (rand <= DropRate) {
            GameObject newCoin = Instantiate(CoinPrefab, transform.position + Vector3.up, Quaternion.Euler(-90, 0, 0));
        }
    }
}
