using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FishBomb : MonoBehaviour
{
    public float speed;
    public float timeToDisapear = 10;
    public float radius = 5.0F;
    public float power = 100.0F;
    public float upwardsModifier = 1.0F;

    bool readyToExplode = true;

    void Update()
    {
        MoveUp();
    }

    private void MoveUp()
    {
        transform.position += new Vector3(0, speed, 0) * Time.deltaTime;
    }

    private void OnTriggerEnter()
    {
        if (readyToExplode)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
            foreach (Collider hit in colliders)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Explosion(hit, rb);
                }
            }
            readyToExplode = false;
        }
    }

    private void Explosion(Collider hit, Rigidbody rb)
    {
        var navAgent = hit.GetComponent<NavMeshAgent>();
        var munchie = hit.GetComponent<Munchie>();
        if (navAgent) navAgent.enabled = false;
        rb.AddExplosionForce(power, transform.position, radius, upwardsModifier);
        if (munchie) munchie.Falling(1, true);       
    }


}
