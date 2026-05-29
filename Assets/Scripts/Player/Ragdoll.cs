using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : IObserverClass
{
    [Header("Ragdoll Bones")]
    [SerializeField] private List<Rigidbody> rbs;
    [SerializeField] private List<Collider> colls;

    [Header("Player")]
    [SerializeField] private _CharacterController playerCont;
    [SerializeField] private CharacterController Cont;
    [SerializeField] private Animator anim;

    private bool appliedForce;

    private void FixedUpdate() {

        if (GameManager.instance.GetState() == GameState.Started || GameManager.instance.GetState() == GameState.Scenic)
        {
            rbs[0].position = new Vector3(6.57464398e-07f, 1.60477364f, -0.0169036426f);
            RagdolOff();    
        }
    }

    public override void OnNotify(Task t) {

        switch (t)
        {
            case Task.isDead:
               RagdollOn();
            break;
        }
    }

    private void RagdollOn() {

        if (!appliedForce)
        {
            appliedForce = true;
            rbs[0].AddForce(transform.forward*50, ForceMode.Impulse);
        }
        playerCont.enabled = false;
        Cont.enabled = false;
        anim.enabled = false;

        foreach (Rigidbody rb in rbs)
            rb.isKinematic = false;

        foreach (Collider coll in colls)
            coll.enabled = true;
    }

    private void RagdolOff() {

        foreach (Rigidbody rb in rbs)
            rb.isKinematic = true;

        foreach (Collider coll in colls)
            coll.enabled = false;

        playerCont.enabled = true;
        Cont.enabled = true;
        anim.enabled = true;
    }
}
