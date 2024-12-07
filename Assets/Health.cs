using Photon.Pun;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] float health = 100f;
    PhotonView pw;

    void Start()
    {
        pw = GetComponent<PhotonView>();
        Debug.Log($"[Health] Initialized with {health} health");
    }

    public bool IsDead()
    {
        bool isDead = health <= 0;
        Debug.Log($"[Health] IsDead check: {isDead}");
        return isDead;
    }

    // Apply damage and check if the entity dies
    public void TakeDamage(float damage)
    {
        if (pw.IsMine) // Only apply damage to the local player's instance
        {
            health = Mathf.Max(health - damage, 0);
            if (health <= 0) Die();
        }
    }

    // Handle death and destroy object across all clients
    void Die()
    {
        if (pw.IsMine)
        {
            Debug.Log("[Health] Entity has died");
            PhotonNetwork.Destroy(gameObject); // Destroy object across the network
        }
    }

    // RPC method to apply damage across all clients
    [PunRPC]
    public void ApplyDamage(float damage)
    {
        TakeDamage(damage); // Apply damage locally
    }
}
