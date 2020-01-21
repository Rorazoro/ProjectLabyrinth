using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : NetworkBehaviour, IInteractable
{
    public string InteractabilityInfo => throw new System.NotImplementedException();

    private Animator animator;

    [ClientRpc]
    public void RpcInteract()
    {
        Interact();
    }

    public void Interact()
    {
        animator.SetBool("Activate", true);
    }

    public void ShowInteractability()
    {
        //throw new System.NotImplementedException();
    }

    private void Start()
    {
        animator = this.transform.parent.GetComponent<Animator>();
    }
}
