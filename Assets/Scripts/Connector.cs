using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connector : Module
{
    [SerializeField]
    public List<Vector3Int> RoomCoordinates;

    public Connector()
    {
        RoomCoordinates = new List<Vector3Int>();
    }
}
