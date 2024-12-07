using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour, IAction
{
    [SerializeField] float timeBetweenAttacks = 1f;
    [SerializeField] float weaponRange = 2.0f;
    [SerializeField] float weaponDamage = 10f;

    Transform targetObject;
    float timeSinceLastAttack;
    private void Start()
    {
        weaponRange = 2.0f;
        weaponDamage = 10f;
        timeBetweenAttacks = 1f;
    }
  private void Update()
{
    timeSinceLastAttack += Time.deltaTime;

    if (targetObject == null)
    {
        GetComponent<Animator>().SetBool("attack", false); // Stop attacking
        return;
    }

    bool isInRange = Vector3.Distance(transform.position, targetObject.position) < weaponRange;

    if (!isInRange)
    {
        GetComponent<Mover>().MoveTo(targetObject.position);
    }
    else
    {
        AttackMethod();
        GetComponent<Mover>().Cancel();
    }
}

    /*  void Hit()
   {
       if (targetObject == null) return; // Ensure the target still exists

       Health health = targetObject.GetComponent<Health>();
       if (health != null)
       {
           health.TakeDamage(weaponDamage);
       }
   }
    */
    void Hit()
    {
        if (targetObject == null) return;

        PhotonView targetPhotonView = targetObject.GetComponent<PhotonView>();
        if (targetPhotonView != null && targetPhotonView.IsMine == false)
        {
            targetPhotonView.RPC("ApplyDamage", RpcTarget.All, weaponDamage);
        }
    }


    private void AttackMethod()
    {
        if (timeSinceLastAttack > timeBetweenAttacks)
        {
            
            GetComponent<Animator>().SetBool("attack",true);
            timeSinceLastAttack = 0;
        }
    }



    /*  public void Attack(CombatTarget target)
      {
          GetComponent<ActionScheduler>().StartAction(this);
          targetObject = target.transform;
          //Debug.Log("Attack is done");
      }
    */

    public void Attack(CombatTarget target)
    {
        Debug.Log("Attacking target: " + target.name);
        PhotonView targetPhotonView = target.GetComponent<PhotonView>();
        if (targetPhotonView != null && !targetPhotonView.IsMine)
        {
            Debug.Log("Sending damage RPC to: " + target.name);
            targetPhotonView.RPC("ApplyDamage", RpcTarget.All, weaponDamage);
        }
    }

    public void Cancel()
{
    targetObject = null; // Ensure no target
    Animator animator = GetComponent<Animator>();
   animator.SetBool("attack", false);
  //  animator.SetTrigger("stopAttack");
}

}
