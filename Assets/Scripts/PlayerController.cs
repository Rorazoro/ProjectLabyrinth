using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Canvas canvas;
    private GameObject playerCamera;
    private FPSMouseLook playerMouseLook;
    private FPSMouseLook camMouseLook;

    //float inputX = Input.GetAxis("Horizontal");
    //float inputY = Input.GetAxis("Vertical");

    // Start is called before the first frame update
    private void Start()
    {
        canvas = GetComponent<Canvas>();
        playerCamera = transform.Find("Player Cam").gameObject;
        camMouseLook = playerCamera.GetComponent<FPSMouseLook>();
        playerMouseLook = GetComponent<FPSMouseLook>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            //RaycastHit hit = Physics.Raycast(transform.position, 
        }
        if (Input.GetKeyDown(KeyCode.Slash) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (playerMouseLook.lockCursor && camMouseLook.lockCursor)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                playerMouseLook.lockCursor = false;
                camMouseLook.lockCursor = false;
            }
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                playerMouseLook.lockCursor = true;
                camMouseLook.lockCursor = true;
            }
        }
    }
}
