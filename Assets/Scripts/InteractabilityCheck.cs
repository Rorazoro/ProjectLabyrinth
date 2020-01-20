using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractabilityCheck : MonoBehaviour
{
    private const int range = 3;

    // Update is called once per frame
    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, range))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            
            if (interactable != null)
            {
                //Debug.Log("Interactable in range: " + interactable);
                interactable.ShowInteractability();

                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactable.Interact();
                }
            }
        }
    }
}
