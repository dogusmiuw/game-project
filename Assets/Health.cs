using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] float health = 100f;
    
    public bool IsDead()
    {
        return health <= 0;
    }
    
    public void TakeDamage(float damage)
    {
        health = Mathf.Max(health - damage, 0);
        Debug.Log($"Health remaining: {health}");
    }
}
