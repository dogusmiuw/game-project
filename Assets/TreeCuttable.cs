using UnityEngine;

public class TreeCuttable : MonoBehaviour
{
    public int treeHealth = 3; // Number of hits before the tree is destroyed

    // Call this method when the player starts chopping
    public void StartChopping()
    {
        if (treeHealth > 0)
        {
            treeHealth--;
            Debug.Log("Tree health: " + treeHealth);
        }

        if (treeHealth <= 0)
        {
            ChopTree();
        }
    }

    // This method is called when the tree is fully chopped
    private void ChopTree()
    {
        // Optionally: Add logic to reward player with wood
        Debug.Log("Tree chopped down!");
        Destroy(gameObject); // Destroy the tree object
    }
}
