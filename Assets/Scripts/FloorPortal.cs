using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorPortal : MonoBehaviour
{
    [SerializeField]
    public FloorPortalReceiver Receiver;

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.transform.position = Receiver.transform.position;
        Debug.Log("Teleport!!!!");
    }
}
