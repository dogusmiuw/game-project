using UnityEngine;

public class TreeCuttable : MonoBehaviour
{
    public int treeHealth = 3; // Number of hits before the tree is destroyed
    //Inventory inventory;


    private void Start()
    {
        //inventory = GetComponent<Inventory>();
    }
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
        Debug.Log("Tree chopped down!");
        Destroy(gameObject);
        Inventory.Instance.GetWood();
        
        // Find and update all bases
        Base[] bases = FindObjectsOfType<Base>();
        foreach(Base baseObj in bases)
        {
            baseObj.UpdateUI();
        }
    }
}