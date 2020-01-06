using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public Exit GetExit(Direction direction)
    {
        return GetComponentsInChildren<Exit>().First(x => x.direction == direction);
    }
}
