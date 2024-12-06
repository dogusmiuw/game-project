using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour, IAction
{
    [SerializeField] float timeBetweenAttacks = 1f;
    [SerializeField] float weaponDamage = 10f;
    [SerializeField] float weaponRange = 2f;
    [SerializeField] float woodcuttingRange = 2f;
    
    Transform targetObject;
    TreeCuttable currentTree;
    float timeSinceLastAttack = 0;

    private void Start()
    {
        weaponRange = 2.0f;
        timeBetweenAttacks = 1f;
        weaponDamage = 10f;
    }

    public void StartChopping(TreeCuttable tree)
    {
        if (tree == null)
        {
            Debug.LogWarning("Attempted to start chopping with null tree!");
            return;
        }
        Debug.Log($"Starting to chop tree {tree.gameObject.name}");
        GetComponent<ActionScheduler>().StartAction(this);
        currentTree = tree;
    }

    private void Update()
    {
        timeSinceLastAttack += Time.deltaTime;
        
        if (currentTree != null)
        {
            if (!currentTree.CanBeChopped())
            {
                Debug.Log($"Tree {currentTree.gameObject.name} cannot be chopped right now");
                Cancel();
                return;
            }

            bool isInRange = Vector3.Distance(transform.position, currentTree.transform.position) < woodcuttingRange;
            if (!isInRange)
            {
                Debug.Log("Moving to tree...");
                GetComponent<Mover>().MoveTo(currentTree.transform.position);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                if (timeSinceLastAttack >= timeBetweenAttacks)
                {
                    Debug.Log("Triggering chop animation");
                    GetComponent<Animator>().ResetTrigger("attack");
                    GetComponent<Animator>().SetTrigger("chop");
                    timeSinceLastAttack = 0;
                }
            }
            return;
        }

        if (targetObject != null && targetObject.gameObject == null)
        {
            Debug.Log("Target was destroyed, canceling attack");
            Cancel();
            return;
        }

        if (targetObject == null) return;

        Health targetHealth = targetObject.GetComponent<Health>();
        if (targetHealth == null || targetHealth.IsDead())
        {
            Debug.Log("Target is dead or has no health component, canceling attack");
            Cancel();
            return;
        }

        bool isInCombatRange = Vector3.Distance(transform.position, targetObject.position) < weaponRange;
        if (!isInCombatRange)
        {
            GetComponent<Mover>().MoveTo(targetObject.position);
        }
        else
        {
            GetComponent<Mover>().Cancel();
            if (timeSinceLastAttack >= timeBetweenAttacks)
            {
                GetComponent<Animator>().ResetTrigger("chop");
                GetComponent<Animator>().SetTrigger("attack");
                timeSinceLastAttack = 0;
            }
        }
    }

    void Hit()
    {
        if (targetObject == null) return;
        
        Health health = targetObject.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(weaponDamage);
        }
    }

    void ChopHit()
    {
        if (currentTree == null)
        {
            Debug.LogWarning("ChopHit called but no tree is targeted!");
            return;
        }
        Debug.Log($"ChopHit executed on tree {currentTree.gameObject.name}");
        currentTree.ChopTree();
        GetComponent<Inventory>().AddItem("Wood");
        currentTree = null;
    }

    public void Attack(CombatTarget target)
    {
        GetComponent<ActionScheduler>().StartAction(this);
        targetObject = target.transform;
    }

    public void Cancel()
    {
        targetObject = null;
        currentTree = null;
    }
}
