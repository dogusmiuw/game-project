using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour, IAction

    
{
    [SerializeField] float weaponRange;
    Transform targetObject;


    private void Start()
    {
        weaponRange = 2.0f;
    }
    public void Attack(CombatTarget target)
    {
        Debug.Log("Attack is done!");
    }


    private void Update()
    {
        if (targetObject == null) { return; }

        bool isInRange= Vector3.Distance(transform.position,targetObject.position)< weaponRange;
        if (isInRange==false) { GetComponent<Mover>().MoveTo(targetObject.position); }
        else {
            GetComponent<Animator>().SetTrigger("attack");
            GetComponent<Mover>().Cancel(); 
        }
    }


    public void Attack()
    {
        GetComponent<ActionScheduler>().StartAction(this);
        targetObject = targetObject.transform;
    }

    public void Cancel()
    {
        targetObject = null;
    }
}
