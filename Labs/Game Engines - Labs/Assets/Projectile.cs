using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float Lifetime = 5.0f;
    private float expireTimer;

    public float Damage = 1;

    private void Start() 
    {
        expireTimer = Lifetime;
    }

    private void Update() 
    {
        expireTimer -= Time.deltaTime;

        if (expireTimer <= 0) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.GetComponent<HealthScript>() != null) 
        {   
            HealthScript health = other.gameObject.GetComponent<HealthScript>();
            if (!health.IsDead) {
                health.DealDamage(1.0f);
                Destroy(gameObject);
            }            
        }
    }
}
