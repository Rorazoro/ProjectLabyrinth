using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour, IInteractable
{
    public string InteractabilityInfo => throw new System.NotImplementedException();

    private Animator animator;

    public void Interact()
    {
        bool open = animator.GetBool("open");
        animator.SetBool("open", !open);
    }

    public void ShowInteractability()
    {
        Debug.Log("Ready to activate door");
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
