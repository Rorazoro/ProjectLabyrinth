using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGenerator2 : MonoBehaviour
{
    [SerializeField]
    public Module[] ModulePrefabs;
    [SerializeField]
    public int RoomSpacing;

    [SerializeField]
    private Vector3Int levelScale = new Vector3Int(5, 1, 5);
    private Room[,,] roomMap;
    private List<Connector> connectors;

    private void Start()
    {
        connectors = new List<Connector>();
        GenerateRooms();
        GameObject playerObject = (GameObject)Instantiate(Resources.Load("Player"), new Vector3(13, 2, -14), Quaternion.identity);
    }

    private void GenerateRooms()
    {
        int floors = levelScale.y;
        int rows = levelScale.x;
        int columns = levelScale.z;

        roomMap = new Room[rows, floors, columns];
        for (int f = 0; f < roomMap.GetLength(1); f++)
        {
            for (int r = 0; r < roomMap.GetLength(0); r++)
            {
                for (int c = 0; c < roomMap.GetLength(2); c++)
                {
                    Vector3Int newRoomCoord = new Vector3Int(r, f, c);
                    Room newRoomPrefab = GetRandomWithTag(ModulePrefabs, "Room") as Room;
                    Room newRoom = Instantiate(newRoomPrefab);
                    newRoom.transform.position -= new Vector3(c * -RoomSpacing, f * RoomSpacing, r * RoomSpacing);
                    newRoom.Coordinate = newRoomCoord;
                    newRoom.transform.parent = this.gameObject.transform;
                    roomMap[r, f, c] = newRoom;

                    List<Vector3Int> nc = newRoom.GetNeighborCoordinates(Vector3Int.zero, levelScale - Vector3Int.one);
                    foreach (Vector3Int neighborCoord in nc)
                    {
                        Exit newRoomExit;
                        if (neighborCoord.x < newRoomCoord.x) //North
                        {
                            newRoomExit = newRoom.GetExit(Direction.N);
                        }
                        else if (neighborCoord.x > newRoomCoord.x) //South
                        {
                            newRoomExit = newRoom.GetExit(Direction.S);
                        }
                        else if (neighborCoord.z > newRoomCoord.z) //East
                        {
                            newRoomExit = newRoom.GetExit(Direction.E);
                        }
                        else //West
                        {
                            newRoomExit = newRoom.GetExit(Direction.W);
                        }

                        if (!connectors.Any(x => x.RoomCoordinates.Contains(newRoomCoord) && x.RoomCoordinates.Contains(neighborCoord)))
                        {
                            Connector newConnectorPrefab = GetRandomWithTag(ModulePrefabs, "Connector") as Connector;
                            Connector newConnector = Instantiate(newConnectorPrefab);
                            newConnector.RoomCoordinates.Add(newRoomCoord);
                            newConnector.RoomCoordinates.Add(neighborCoord);
                            newConnector.transform.parent = this.gameObject.transform;
                            connectors.Add(newConnector);
                            MatchExits(newRoomExit, newConnector.GetExit(Direction.A));
                        }
                    }
                }
            }
        }

    }

    private void MatchExits(Exit oldExit, Exit newExit)
    {
        var newModule = newExit.transform.parent;
        var forwardVectorToMatch = -oldExit.transform.forward;
        var correctiveRotation = Azimuth(forwardVectorToMatch) - Azimuth(newExit.transform.forward);
        newModule.RotateAround(newExit.transform.position, Vector3.up, correctiveRotation);
        var correctiveTranslation = oldExit.transform.position - newExit.transform.position;
        newModule.transform.position += correctiveTranslation;
    }

    private static float Azimuth(Vector3 vector)
    {
        return Vector3.Angle(Vector3.forward, vector) * Mathf.Sign(vector.x);
    }

    private static Module GetRandomWithTag(IEnumerable<Module> modules, string tagToMatch)
    {
        var matchingModules = modules.Where(m => m.Tags.Contains(tagToMatch)).ToArray();
        if (matchingModules.Any())
        {
            return GetRandom(matchingModules);
        }
        else
        {
            return null;
        }
    }

    private static TItem GetRandom<TItem>(TItem[] array)
    {
        try
        {
            return array[UnityEngine.Random.Range(0, array.Length)];
        }
        catch (Exception)
        {
            throw;
        }
    }
}
