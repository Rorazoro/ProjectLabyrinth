using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    private Vector3Int levelScale = new Vector3Int(5, 1, 5);
    private Module[,,] modules;

    [SerializeField]
    public Module[] ModulePrefabs;
    [SerializeField]
    public Module StartModule;

    // Start is called before the first frame update
    private void Start()
    {
        GenerateLevel();
    }

    private void GenerateLevel()
    {
        modules = new Module[(levelScale.x * 2) - 1, levelScale.y, (levelScale.z * 2) - 1];
        StartModule = Instantiate(StartModule, transform.position, transform.rotation);

        for (int floorIndex = 0; floorIndex < this.modules.GetLength(1); floorIndex++)
        {
            for (int rowIndex = 0; rowIndex < this.modules.GetLength(0); rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < this.modules.GetLength(2); columnIndex++)
                {
                    Room currentRoom = new Room(rowIndex, floorIndex, columnIndex);


                    this.modules[rowIndex, floorIndex, columnIndex] = currentRoom;
                    //createdRooms.Add(currentRoom);
                }
            }
        }

        //foreach (Room room in createdRooms)
        //{
        //    List<Vector3Int> neighborCoordinates = room.NeighborCoordinates(Vector3Int.zero, levelScale);
        //    foreach (Vector3Int coordinate in neighborCoordinates)
        //    {
        //        Room neighbor = this.rooms[coordinate.x, coordinate.y, coordinate.z];
        //        if (neighbor != null)
        //        {
        //            room.Connect(neighbor);
        //        }
        //    }
        //}

        Vector3Int startRoomCoordinate = new Vector3Int((int)Random.Range(0, (levelScale.x / 2)), 0, (int)Random.Range(0, (levelScale.z / 2)));
        //return this.rooms[startRoomCoordinate.x, startRoomCoordinate.y, startRoomCoordinate.z];
    }

    private void PrintGrid()
    {
        for (int floorIndex = 0; floorIndex < this.modules.GetLength(1); floorIndex++)
        {
            for (int rowIndex = 0; rowIndex < this.modules.GetLength(0); rowIndex++)
            {
                string row = "";
                for (int columnIndex = 0; columnIndex < this.modules.GetLength(2); columnIndex++)
                {
                    if (this.modules[rowIndex, floorIndex, columnIndex] == null)
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
