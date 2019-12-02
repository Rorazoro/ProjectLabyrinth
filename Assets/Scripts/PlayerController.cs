using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public Canvas canvas;

    //float inputX = Input.GetAxis("Horizontal");
    //float inputY = Input.GetAxis("Vertical");

    // Start is called before the first frame update
    private void Start()
    {
        canvas = GetComponent<Canvas>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            //RaycastHit hit = Physics.Raycast(transform.position, 
        }
    }
}
