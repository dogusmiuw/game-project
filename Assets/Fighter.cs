using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour, IAction

    
{
    [SerializeField] float timeBetweenAttacks = 1f;
    [SerializeField] float weaponDamage=10f;
    [SerializeField] float weaponRange;
    Transform targetObject;
    float timeSinceLastAttack;


    private void Start()
    {
        weaponRange = 2.0f;
        timeBetweenAttacks = 1f;
        weaponDamage = 10f;
    }
    public void Attack(CombatTarget target)
    {
        GetComponent<ActionScheduler>().StartAction(this);
        targetObject = target.transform;
    }

    private void Update()
    {
        timeSinceLastAttack += Time.deltaTime;
        if (targetObject == null) return;

        bool isInRange = Vector3.Distance(transform.position, targetObject.position) < weaponRange;
        if (!isInRange)
        {
            GetComponent<Mover>().MoveTo(targetObject.position);
        }
        else
        {
            GetComponent<Mover>().Cancel();
            if (timeSinceLastAttack >= timeBetweenAttacks)
            {
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
 /*   public void Attack()
    {
        GetComponent<ActionScheduler>().StartAction(this);
        targetObject = targetObject.transform;
    }
*/
    public void Cancel()
    {
        targetObject = null;
    }
}
