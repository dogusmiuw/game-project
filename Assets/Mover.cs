using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    //[SerializeField] Transform target;
    Ray ray;
    void Update()
    {
        
        UpdateAnimator();
    }

    public void MoveTo(Vector3 hit)
    {
        GetComponent<NavMeshAgent>().destination = hit;
    }

    private void UpdateAnimator()
    {
        Vector3 velocity=GetComponent<NavMeshAgent>().velocity;
        Vector3 localvelocity = transform.InverseTransformDirection(velocity);
        float speed = localvelocity.z;
        GetComponent<Animator>().SetFloat("forwardSpeed", speed);
    }

   /* private void MoveToCursor()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        bool hasHit = Physics.Raycast(ray, out hit);

        if (hasHit)
        {
            GetComponent<NavMeshAgent>().destination = hit.point;
        }
    }
   */


    public void Cancel()
    {
        GetComponent<NavMeshAgent>().isStopped=true;
    }
}
