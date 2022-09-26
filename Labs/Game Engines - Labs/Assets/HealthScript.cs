using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    public float MaximumHealth = 3;
    public float CurrentHealth;

    private void Start() 
    {
        CurrentHealth = MaximumHealth;
    }

    public void DealDamage(float amount) 
    {
        CurrentHealth -= amount;

        if (CurrentHealth <= 0) {
            Destroy(gameObject);
        }
    }
}
