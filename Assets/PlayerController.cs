using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Debug.Log("Checking for interactions...");
        
        if (InteractWithTree() == true)
        {
            Debug.Log("Tree interaction detected");
            return;
        }
        if (InteractWithCombat() == true)
        {
            return;
        }
        if (InteractWithMovement() == true)
        {
            return;
        }
        Debug.Log("Nothing");
    }
    private bool InteractWithCombat()
    {
        RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
        foreach (RaycastHit hit in hits)
        {
            CombatTarget target = hit.transform.GetComponent<CombatTarget>();
            if (target == null)
            {
                continue;
            }

            if (Input.GetMouseButtonDown(0))
            {
                GetComponent<Fighter>().Attack(target);
            }
            return true;
        }
        return false;
    }
    private bool InteractWithMovement()
    {
        RaycastHit hit;
        bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
        if (hasHit)
        {
            if(Input.GetMouseButton(0))
            {
                GetComponent<Mover>().MoveTo(hit.point);
                Debug.Log("Move");
            }
            return true;
            
        }
        return false;
    }
    private static Ray GetMouseRay()
    {
        return Camera.main.ScreenPointToRay(Input.mousePosition);
    }

    private bool InteractWithTree()
    {
        Debug.Log("Checking for tree interactions...");
        RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
        Debug.Log($"Found {hits.Length} objects in raycast");
        
        foreach (RaycastHit hit in hits)
        {
            Debug.Log($"Checking object: {hit.transform.name}");
            TreeCuttable tree = hit.transform.GetComponent<TreeCuttable>();
            if (tree == null)
            {
                Debug.Log($"No TreeCuttable component on {hit.transform.name}");
                continue;
            }

            Debug.Log($"Found tree: {hit.transform.name}");
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Space pressed, starting to chop");
                GetComponent<Fighter>().StartChopping(tree);
            }
            return true;
        }
        Debug.Log("No trees found in raycast");
        return false;
    }
}
