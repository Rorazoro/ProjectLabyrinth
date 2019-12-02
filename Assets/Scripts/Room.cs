using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Room
{
    [SerializeField]
    public Vector3Int roomCoordinate;
    public Dictionary<string, Room> neighbors;

    public Room(int x, int y, int z)
    {
        roomCoordinate = new Vector3Int(x, y, z);
        neighbors = new Dictionary<string, Room>();
    }

    public Room(Vector3Int coord)
    {
        roomCoordinate = coord;
        neighbors = new Dictionary<string, Room>();
    }

    public string PrefabName()
    {
        return "Room";
    }

    public List<Vector3Int> NeighborCoordinates(Vector3Int min, Vector3Int max)
    {
        List<Vector3Int> neighborCoordinates = new List<Vector3Int>();
        neighborCoordinates.Add(new Vector3Int(this.roomCoordinate.x, this.roomCoordinate.y, this.roomCoordinate.z - 1));
        neighborCoordinates.Add(new Vector3Int(this.roomCoordinate.x + 1, this.roomCoordinate.y, this.roomCoordinate.z));
        neighborCoordinates.Add(new Vector3Int(this.roomCoordinate.x, this.roomCoordinate.y, this.roomCoordinate.z + 1));
        neighborCoordinates.Add(new Vector3Int(this.roomCoordinate.x - 1, this.roomCoordinate.y, this.roomCoordinate.z));

        neighborCoordinates.RemoveAll(v => v.x < min.x || v.z < min.z || v.x >= max.x || v.z >= max.z);

        return neighborCoordinates;
    }

    public void Connect(Room neighbor)
    {
        string direction = "";
        if (neighbor.roomCoordinate.z < this.roomCoordinate.z)
        {
            direction = "N";
        }
        if (neighbor.roomCoordinate.x > this.roomCoordinate.x)
        {
            direction = "E";
        }
        if (neighbor.roomCoordinate.z > this.roomCoordinate.z)
        {
            direction = "S";
        }
        if (neighbor.roomCoordinate.x < this.roomCoordinate.x)
        {
            direction = "W";
        }
        this.neighbors.Add(direction, neighbor);
    }
}
