using System;
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
    public Guid Id = Guid.NewGuid();

    public Exit[] GetExits()
    {
        return GetComponentsInChildren<Exit>();
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnCollisionEnter(Collision col)
    {
    }
}
