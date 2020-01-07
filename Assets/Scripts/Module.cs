using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Module : MonoBehaviour
{
    [SerializeField]
    public string[] Tags;

    public Exit[] GetExits()
    {
        return GetComponentsInChildren<Exit>();
    }

    public Exit GetExit(Direction direction)
    {
        return GetComponentsInChildren<Exit>().First(x => x.direction == direction);
    }
}
