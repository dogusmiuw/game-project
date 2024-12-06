using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] float health = 100f;
    
    void Start()
    {
        Debug.Log($"[Health] Initialized with {health} health");
    }
    
    public bool IsDead()
    {
        bool isDead = health <= 0;
        Debug.Log($"[Health] IsDead check: {isDead}");
        return isDead;
    }
    
    public void TakeDamage(float damage)
    {
        Debug.Log($"[Health] Taking damage: {damage}");
        float previousHealth = health;
        health = Mathf.Max(health - damage, 0);
        Debug.Log($"[Health] Health reduced from {previousHealth} to {health}");
        
        if (health <= 0)
        {
            Debug.Log("[Health] Entity has died");
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"[Health] Destroying game object: {gameObject.name}");
        Destroy(gameObject);
    }
}
