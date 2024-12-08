using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private TreeCuttable detectedTree = null;


    //Inventory inventory;
    Animator animator;
    PhotonView pw;
    private void Start()
    {
        //inventory = GetComponent<Inventory>();
        pw = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();

    }

    void Update()
    {
        if (!pw.IsMine) return;

        if (InteractWithTree())
        {
            if (detectedTree != null && Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Space pressed, cutting tree");
                TriggerChoppingAnimation();
                detectedTree.StartChopping();
                detectedTree = null; // Reset after chopping
            }
            return;
        }

        if (InteractWithCombat())
        {
            return;
        }

        if (InteractWithMovement())
        {
            return;
        }

        Debug.Log("Nothing");
    }


    private void TriggerChoppingAnimation()
    {
        if (animator != null)
        {
            Debug.Log("Triggering chopping animation");
            animator.SetBool("chop", true);  
            StartCoroutine(ResetChoppingAnimation());
        }
        else
        {
            Debug.LogWarning("Animator is null!");
        }
    }

    // Reset the chopping animation after a delay
    private IEnumerator ResetChoppingAnimation()
    {
        yield return new WaitForSeconds(1f);  // Adjust time based on animation length
        if (animator != null)
        {
            Debug.Log("Resetting chopping animation");
            animator.SetBool("chop", false);
        }
    }


    // Example of refactoring the InteractWithCombat method:
    private bool InteractWithCombat()
    {
        RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
        foreach (RaycastHit hit in hits)
        {
          //  CombatTarget target = hit.transform.GetComponent<CombatTarget>();
          //  if (target != null)
            //{
              //  Debug.Log("Combat target found: " + hit.transform.name);
                //if (Input.GetMouseButton(0)) // Right-click to attack
                //{
                  //  Debug.Log("Attack triggered");
                    // Send the RPC to all clients to trigger the attack animation
                    //TriggerAttackAnimation(target.gameObject);
                    //return true;  // Return true to indicate combat interaction is complete
              //  }
           // }
        }
        return false;  // No combat target found
    }


    private bool InteractWithMovement()
    {

        RaycastHit hit;
        bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
        if (hasHit)
        {
            if (Input.GetMouseButton(1))
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
        Ray ray = GetMouseRay();
        int treeLayerMask = LayerMask.GetMask("Tree");  // Ensure "Tree" is the exact name of your layer
        RaycastHit[] hits = Physics.RaycastAll(ray, 100f, treeLayerMask);

        foreach (RaycastHit hit in hits)
        {
            TreeCuttable tree = hit.transform.GetComponent<TreeCuttable>();
            if (tree != null)
            {
                Debug.Log($"Found tree: {hit.transform.name}");
                detectedTree = tree; // Store the detected tree
                return true;
            }
        }

        detectedTree = null; // Reset if no tree is found
        return false;
    }




    [PunRPC]
    private void TriggerAttackAnimation(GameObject target)
    {
        Debug.Log("Attack animation triggered");

        if (animator != null)
        {
            Debug.Log("Setting attack to true");
            animator.SetBool("attack", true); // Set attack animation to true
        }
        else
        {
            Debug.LogWarning("Animator is null!");
        }

        // Apply damage to the target
        if (target != null)
        {
            Health health = target.GetComponent<Health>();
            if (health != null)
            {
                Debug.Log("Applying damage to the target");
                target.GetComponent<PhotonView>().RPC("ApplyDamage", RpcTarget.All, 10f);  // Apply damage across all clients
            }
            else
            {
                Debug.LogWarning("Target does not have Health component");
            }
        }
    }

    private IEnumerator ResetAttackParameter()
    {
        // Wait for a short duration to simulate the attack animation
        yield return new WaitForSeconds(0.5f);  // Adjust this time based on your animation length

        if (animator != null)
        {
            Debug.Log("Resetting attack to false");
            animator.SetBool("attack", false);  // Reset the attack boolean to false
        }
    }


}