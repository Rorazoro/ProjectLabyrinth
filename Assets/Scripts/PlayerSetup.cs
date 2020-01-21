using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetup : NetworkBehaviour
{
    [SerializeField]
    private Behaviour[] componentsToDisable;

    private Camera sceneCamera;

    private void Start()
    {
        if (!hasAuthority)
        {
            foreach (Behaviour b in componentsToDisable)
            {
                b.enabled = false;
            }
        }
        else {
            sceneCamera = Camera.main;
            if (sceneCamera != null){
                sceneCamera.gameObject.SetActive(false);
            }
        }
    }

    private void OnDisable() {
        if (sceneCamera != null) {
            sceneCamera.gameObject.SetActive(true);
        }    
    }
}
