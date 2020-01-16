using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour, IInteractable
{
    public string InteractabilityInfo => throw new System.NotImplementedException();

    private Animator animator;

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
