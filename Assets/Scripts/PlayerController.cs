using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float lookSensitivity = 3f;

    private PlayerMotor motor;
    private Vector3 _velocity = Vector3.zero;
    private Vector3 _rotation = Vector3.zero;
    private Vector3 _cameraRotation = Vector3.zero;

    private void Start()
    {
        motor = GetComponent<PlayerMotor>();
    }

    private void Update()
    {
        //Calculate movement velocity as a 3D vector
        float _xMov = Input.GetAxisRaw("Horizontal");
        float _zMov = Input.GetAxisRaw("Vertical");

        Vector3 _movHorizontal = transform.right * _xMov;
        Vector3 _movVertical = transform.forward * _zMov;

        //Final movement vector
        _velocity = (_movHorizontal + _movVertical).normalized * speed;

        //Apply movement
        motor.Move(_velocity);

        //Calculate rotation as a 3D vector (turning around)
        float _yRot = Input.GetAxisRaw("Mouse X");

        _rotation = new Vector3(0f, _yRot, 0f) * lookSensitivity;

        //Apply rotation
        motor.Rotate(_rotation);

        //Calculate camera rotation as a 3D vector (turning around)
        float _xRot = Input.GetAxisRaw("Mouse Y");

        _cameraRotation = new Vector3(_xRot, 0f, 0f) * lookSensitivity;

        //Apply rotation
        motor.RotateCamera(_cameraRotation);
    }

    //private Canvas canvas;
    //private GameObject playerCamera;s
    //private FPSMouseLook playerMouseLook;
    //private FPSMouseLook camMouseLook;

    ////float inputX = Input.GetAxis("Horizontal");
    ////float inputY = Input.GetAxis("Vertical");

    //// Start is called before the first frame update
    //private void Start()
    //{
    //    canvas = GetComponent<Canvas>();
    //    playerCamera = transform.Find("Player Cam").gameObject;
    //    camMouseLook = playerCamera.GetComponent<FPSMouseLook>();
    //    playerMouseLook = GetComponent<FPSMouseLook>();
    //    Cursor.visible = false;
    //    Cursor.lockState = CursorLockMode.Locked;
    //}

    //// Update is called once per frame
    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.E))
    //    {
    //        //RaycastHit hit = Physics.Raycast(transform.position, 
    //    }
    //    if (Input.GetKeyDown(KeyCode.Slash) || Input.GetKeyDown(KeyCode.Escape))
    //    {
    //        if (playerMouseLook.lockCursor && camMouseLook.lockCursor)
    //        {
    //            Cursor.visible = false;
    //            Cursor.lockState = CursorLockMode.Locked;
    //            playerMouseLook.lockCursor = false;
    //            camMouseLook.lockCursor = false;
    //        }
    //        else
    //        {
    //            Cursor.visible = true;
    //            Cursor.lockState = CursorLockMode.None;
    //            playerMouseLook.lockCursor = true;
    //            camMouseLook.lockCursor = true;
    //        }
    //    }
    //}
}
