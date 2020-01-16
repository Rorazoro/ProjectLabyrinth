using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Room : Module
{
    [SerializeField]
    public Vector3Int Coordinate;
    [SerializeField]
    public bool IsElevatorRoom = false;

    public List<Vector3Int> GetNeighborCoordinates(Vector3Int min, Vector3Int max)
    {
        List<Vector3Int> neighborCoordinates = new List<Vector3Int>();
        neighborCoordinates.Add(new Vector3Int(this.Coordinate.x, this.Coordinate.y, this.Coordinate.z - 1));
        neighborCoordinates.Add(new Vector3Int(this.Coordinate.x + 1, this.Coordinate.y, this.Coordinate.z));
        neighborCoordinates.Add(new Vector3Int(this.Coordinate.x, this.Coordinate.y, this.Coordinate.z + 1));
        neighborCoordinates.Add(new Vector3Int(this.Coordinate.x - 1, this.Coordinate.y, this.Coordinate.z));

        neighborCoordinates.RemoveAll(v => v.x < min.x || v.z < min.z || v.x > max.x || v.z > max.z);

        return neighborCoordinates;
    }

    public List<Vector3Int> GetDeadendCoordinates(Vector3Int min, Vector3Int max)
    {
        List<Vector3Int> neighborCoordinates = new List<Vector3Int>();
        neighborCoordinates.Add(new Vector3Int(this.Coordinate.x, this.Coordinate.y, this.Coordinate.z - 1));
        neighborCoordinates.Add(new Vector3Int(this.Coordinate.x + 1, this.Coordinate.y, this.Coordinate.z));
        neighborCoordinates.Add(new Vector3Int(this.Coordinate.x, this.Coordinate.y, this.Coordinate.z + 1));
        neighborCoordinates.Add(new Vector3Int(this.Coordinate.x - 1, this.Coordinate.y, this.Coordinate.z));

        neighborCoordinates.RemoveAll(v => v.x >= min.x && v.z >= min.z && v.x <= max.x && v.z <= max.z);

        return neighborCoordinates;
    }
}
