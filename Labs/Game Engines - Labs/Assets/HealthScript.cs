using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    public float MaximumHealth = 3;
    public float CurrentHealth;

    public bool IsDead = false;

    public delegate void OnKilledDelegate();
    public OnKilledDelegate OnKilled;

    public delegate void OnHitDelegate();
    public OnHitDelegate OnHit;

    private void Start() 
    {
        CurrentHealth = MaximumHealth;
    }

    public void DealDamage(float amount) 
    {
        CurrentHealth -= amount;
        
        if (OnHit != null) {
            OnHit?.Invoke();
        }


        if (CurrentHealth <= 0) {
            IsDead = true;

            if (OnKilled != null) {                
                OnKilled?.Invoke();
            }

            StartCoroutine(Cleanup());
        }
    }

    // Used to clean up any bodies registered as dead.
    private IEnumerator Cleanup() 
    {
        yield return new WaitForSeconds(6.0f);
        if (gameObject.tag != "Player") {
            Destroy(gameObject);
        }        
    }
}
