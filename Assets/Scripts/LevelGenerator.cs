using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    private Vector3Int levelScale = new Vector3Int(5, 1, 5);
    private Room[,,] rooms;
    private Room currentRoom;
    private LevelGenerator instance;

    // Start is called before the first frame update
    private void Start()
    {
        this.currentRoom = GenerateLevel();
        string roomPrefabName = this.currentRoom.PrefabName();
        GameObject roomObject = (GameObject)Instantiate(Resources.Load(roomPrefabName));
        GameObject playerObject = (GameObject)Instantiate(Resources.Load("Player"), new Vector3(13, 2, -14), Quaternion.identity);
        //PrintGrid();
    }

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
            currentRoom = GenerateLevel();
        }
        else
        {
            string roomPrefabName = instance.currentRoom.PrefabName();
            GameObject roomObject = (GameObject)Instantiate(Resources.Load(roomPrefabName));
            Destroy(this.gameObject);
        }
    }

    private Room GenerateLevel()
    {
        rooms = new Room[levelScale.x, levelScale.y, levelScale.z];
        int numberOfRooms = levelScale.x * levelScale.y * levelScale.z;

        List<Room> createdRooms = new List<Room>();
        for (int floorIndex = 0; floorIndex < this.rooms.GetLength(1); floorIndex++)
        {
            for (int rowIndex = 0; rowIndex < this.rooms.GetLength(0); rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < this.rooms.GetLength(2); columnIndex++)
                {
                    Room currentRoom = new Room(rowIndex, floorIndex, columnIndex);
                    this.rooms[rowIndex, floorIndex, columnIndex] = currentRoom;
                    createdRooms.Add(currentRoom);
                }
            }
        }

        foreach (Room room in createdRooms)
        {
            List<Vector3Int> neighborCoordinates = room.NeighborCoordinates(Vector3Int.zero, levelScale);
            foreach (Vector3Int coordinate in neighborCoordinates)
            {
                Room neighbor = this.rooms[coordinate.x, coordinate.y, coordinate.z];
                if (neighbor != null)
                {
                    room.Connect(neighbor);
                }
            }
        }

        Vector3Int startRoomCoordinate = new Vector3Int((int)Random.Range(0, (levelScale.x / 2)), 0, (int)Random.Range(0, (levelScale.z / 2)));
        return this.rooms[startRoomCoordinate.x, startRoomCoordinate.y, startRoomCoordinate.z];
    }

    public void MoveToRoom(Room room)
    {
        this.currentRoom = room;
    }

    public Room CurrentRoom()
    {
        return this.currentRoom;
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
}
