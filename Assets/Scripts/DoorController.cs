using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour, IInteractable
{
    [SerializeField]
    public string direction;
    public string InteractabilityInfo => throw new System.NotImplementedException();

    private Animator animator;

    public void Interact()
    {
        animator.SetBool("open", true);

        StartCoroutine(AutoClose(5));
    }

    public void ShowInteractability()
    {
        //Debug.Log("Ready to activate door");
    }

    private IEnumerator AutoClose(int secs)
    {
        yield return new WaitForSeconds(secs);
        animator.SetBool("open", false);
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
