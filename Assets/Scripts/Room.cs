using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Room : Module
{
    public bool startRoom = false;
    public bool endRoom = false;

    public Room(int x, int y, int z)
    {
        Coordinate = new Vector3Int(x, y, z);
    }

    public Room(Vector3Int coord)
    {
        Coordinate = coord;
    }

    public string PrefabName()
    {
        return "Room";
    }

    public List<Vector3Int> GetExits(Vector3Int min, Vector3Int max)
    {
        List<Vector3Int> neighborCoordinates = new List<Vector3Int>();
        neighborCoordinates.Add(new Vector3Int(this.Coordinate.x, this.Coordinate.y, this.Coordinate.z - 1));
        neighborCoordinates.Add(new Vector3Int(this.Coordinate.x + 1, this.Coordinate.y, this.Coordinate.z));
        neighborCoordinates.Add(new Vector3Int(this.Coordinate.x, this.Coordinate.y, this.Coordinate.z + 1));
        neighborCoordinates.Add(new Vector3Int(this.Coordinate.x - 1, this.Coordinate.y, this.Coordinate.z));

        neighborCoordinates.RemoveAll(v => v.x < min.x || v.z < min.z || v.x >= max.x || v.z >= max.z);

        return neighborCoordinates;
    }
}
