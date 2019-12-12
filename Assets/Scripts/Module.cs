using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module : MonoBehaviour
{
    [SerializeField]
    public Vector3Int Coordinate;
    [SerializeField]
    public string[] Tags;
    [SerializeField]
    public bool overlap;

    public Exit[] GetExits()
    {
        return GetComponentsInChildren<Exit>();
    }

    private void OnTriggerEnter(Collider other)
    {
        overlap = true;
    }
}
