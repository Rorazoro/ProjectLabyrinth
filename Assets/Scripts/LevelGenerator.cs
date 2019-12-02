using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    private Vector3Int levelScale = new Vector3Int(5, 1, 5);
    private Room[,,] rooms;

    // Start is called before the first frame update
    private void Start()
    {
        GenerateLevel();
        PrintGrid();
    }

    private void PrintGrid()
    {
        for (int floorIndex = 0; floorIndex < this.rooms.GetLength(1); floorIndex++)
        {
            for (int rowIndex = 0; rowIndex < this.rooms.GetLength(0); rowIndex++)
            {
                string row = "";
                for (int columnIndex = 0; columnIndex < this.rooms.GetLength(2); columnIndex++)
                {
                    if (this.rooms[rowIndex, floorIndex, columnIndex] == null)
                    {
                        row += "X";
                    }
                    else
                    {
                        row += "R";
                    }
                }
                Debug.Log(row);
            }
        }
    }

    private void GenerateLevel()
    {
        rooms = new Room[levelScale.x, levelScale.y, levelScale.z];
        int numberOfRooms = levelScale.x * levelScale.y * levelScale.z;

        Vector3Int initialRoomCoordinate = new Vector3Int((levelScale.x / 2) - 1, 0, (levelScale.z / 2) - 1);

        Queue<Room> roomsToCreate = new Queue<Room>();
        roomsToCreate.Enqueue(new Room(initialRoomCoordinate.x, initialRoomCoordinate.y, initialRoomCoordinate.z));

        List<Room> createdRooms = new List<Room>();
        while (roomsToCreate.Count > 0 && createdRooms.Count < numberOfRooms)
        {
            Room currentRoom = roomsToCreate.Dequeue();
            this.rooms[currentRoom.roomCoordinate.x, currentRoom.roomCoordinate.y, currentRoom.roomCoordinate.z] = currentRoom;
            createdRooms.Add(currentRoom);
            AddNeighbors(currentRoom, roomsToCreate);
        }

        foreach (Room room in createdRooms)
        {
            List<Vector3Int> neighborCoordinates = room.NeighborCoordinates();
            foreach (Vector3Int coordinate in neighborCoordinates)
            {
                Room neighbor = this.rooms[coordinate.x, coordinate.y, coordinate.z];
                if (neighbor != null)
                {
                    room.Connect(neighbor);
                }
            }
        }
    }

    private void AddNeighbors(Room currentRoom, Queue<Room> roomsToCreate)
    {
        List<Vector3Int> neighborCoordinates = currentRoom.NeighborCoordinates();
        List<Vector3Int> availableNeighbors = new List<Vector3Int>();
        foreach (Vector3Int coordinate in neighborCoordinates)
        {
            if (this.rooms[coordinate.x, coordinate.y, coordinate.z] == null)
            {
                availableNeighbors.Add(coordinate);
            }
        }

        int numberOfNeighbors = (int)Random.Range(1, availableNeighbors.Count);

        for (int neighborIndex = 0; neighborIndex < numberOfNeighbors; neighborIndex++)
        {
            float randomNumber = Random.value;
            float roomFrac = 1f / (float)availableNeighbors.Count;
            Vector3Int chosenNeighbor = new Vector3Int(0, 0, 0);
            foreach (Vector3Int coordinate in availableNeighbors)
            {
                if (randomNumber < roomFrac)
                {
                    chosenNeighbor = coordinate;
                    break;
                }
                else
                {
                    roomFrac += 1f / (float)availableNeighbors.Count;
                }
            }
            roomsToCreate.Enqueue(new Room(chosenNeighbor));
            availableNeighbors.Remove(chosenNeighbor);
        }
    }
}
