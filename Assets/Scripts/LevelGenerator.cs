using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    public Module[] ModulePrefabs;
    [SerializeField]
    public Connector BlockerPrefab;
    [SerializeField]
    public Room ElevatorTopPrefab;
    [SerializeField]
    public Room ElevatorBottomPrefab;

    [SerializeField]
    public int RoomSpacing = 33;

    [SerializeField]
    private Vector3Int levelScale = new Vector3Int(5, 1, 5);
    private Room[,,] roomMap;
    private List<Connector> connectors;
    private FloorPortal[] floorPortals;

    private void Start()
    {
        connectors = new List<Connector>();
        GenerateLevel();
        //GeneratePortals();
        GenerateElevators();
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        int r = UnityEngine.Random.Range(0, roomMap.GetLength(0));
        int c = UnityEngine.Random.Range(0, roomMap.GetLength(2));

        Vector3 playerPosition = new Vector3((c * RoomSpacing) + 13, 2, (r * -RoomSpacing) + -14);
        GameObject playerObject = (GameObject)Instantiate(Resources.Load("Player"), playerPosition, Quaternion.identity);
        playerObject.name = "Player";
    }

    private void GenerateLevel()
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

                    GenerateConnectors(newRoom);
                    GenerateBlockers(newRoom);
                }
            }
        }
    }

    private void GenerateConnectors(Room newRoom)
    {
        List<Vector3Int> nc = newRoom.GetNeighborCoordinates(Vector3Int.zero, levelScale - Vector3Int.one);
        foreach (Vector3Int neighborCoord in nc)
        {
            Exit newRoomExit;
            if (neighborCoord.x < newRoom.Coordinate.x) //North
            {
                newRoomExit = newRoom.GetExit(Direction.N);
            }
            else if (neighborCoord.x > newRoom.Coordinate.x) //South
            {
                newRoomExit = newRoom.GetExit(Direction.S);
            }
            else if (neighborCoord.z > newRoom.Coordinate.z) //East
            {
                newRoomExit = newRoom.GetExit(Direction.E);
            }
            else //West
            {
                newRoomExit = newRoom.GetExit(Direction.W);
            }

            if (!connectors.Any(x => x.RoomCoordinates.Contains(newRoom.Coordinate) && x.RoomCoordinates.Contains(neighborCoord)))
            {
                Connector newConnectorPrefab = GetRandomWithTag(ModulePrefabs, "Connector") as Connector;
                Connector newConnector = Instantiate(newConnectorPrefab);
                newConnector.RoomCoordinates.Add(newRoom.Coordinate);
                newConnector.RoomCoordinates.Add(neighborCoord);
                newConnector.transform.parent = this.gameObject.transform;
                connectors.Add(newConnector);
                MatchExits(newRoomExit, newConnector.GetExit(Direction.A));
            }
        }
    }

    private void GenerateBlockers(Room newRoom)
    {
        List<Vector3Int> dc = newRoom.GetDeadendCoordinates(Vector3Int.zero, levelScale - Vector3Int.one);
        foreach (Vector3Int Coord in dc)
        {
            Exit newRoomDeadExit;
            if (Coord.x < newRoom.Coordinate.x) //North
            {
                newRoomDeadExit = newRoom.GetExit(Direction.N);
            }
            else if (Coord.x > newRoom.Coordinate.x) //South
            {
                newRoomDeadExit = newRoom.GetExit(Direction.S);
            }
            else if (Coord.z > newRoom.Coordinate.z) //East
            {
                newRoomDeadExit = newRoom.GetExit(Direction.E);
            }
            else //West
            {
                newRoomDeadExit = newRoom.GetExit(Direction.W);
            }

            Connector newConnector = Instantiate(BlockerPrefab);
            newConnector.RoomCoordinates.Add(newRoom.Coordinate);
            newConnector.RoomCoordinates.Add(Coord);
            newConnector.transform.parent = this.gameObject.transform;
            connectors.Add(newConnector);
            MatchExits(newRoomDeadExit, newConnector.GetExit(Direction.A));
        }
    }

    //private void GeneratePortals()
    //{
    //    floorPortals = new FloorPortal[roomMap.GetLength(1) - 1];
    //    for (int f = 0; f < floorPortals.Length; f++)
    //    {
    //        int r = UnityEngine.Random.Range(0, roomMap.GetLength(0));
    //        int c = UnityEngine.Random.Range(0, roomMap.GetLength(2));

    //        Vector3 portalPosition = new Vector3((c * RoomSpacing) + 13, (f * -RoomSpacing) + 1, (r * -RoomSpacing) + -14);
    //        FloorPortal portal = Instantiate(FloorPortalPrefab, portalPosition, Quaternion.identity);
    //        portal.transform.parent = this.gameObject.transform;

    //        Vector3 receiverPosition = new Vector3((c * RoomSpacing) + 13, ((f + 1) * -RoomSpacing) + 1, (r * -RoomSpacing) + -14);
    //        FloorPortalReceiver receiver = Instantiate(FloorPortalReceiverPrefab, receiverPosition, Quaternion.identity);
    //        receiver.transform.parent = this.gameObject.transform;
    //        portal.Receiver = receiver;

    //        floorPortals[f] = portal;
    //    }
    //}

    private void GenerateElevators()
    {
        floorPortals = new FloorPortal[roomMap.GetLength(1) - 1];
        for (int f = 0; f < floorPortals.Length; f++)
        {
            int r = UnityEngine.Random.Range(0, roomMap.GetLength(0));
            int c = UnityEngine.Random.Range(0, roomMap.GetLength(2));

            Destroy(roomMap[r, f, c].gameObject);
            Room elevatorTop = Instantiate(ElevatorTopPrefab, roomMap[r, f, c].transform.position, roomMap[r, f, c].transform.rotation);
            elevatorTop.transform.parent = this.gameObject.transform;
            roomMap[r, f, c] = elevatorTop;

            Destroy(roomMap[r, f + 1, c].gameObject);
            Room elevatorBottom = Instantiate(ElevatorBottomPrefab, roomMap[r, f + 1, c].transform.position, roomMap[r, f + 1, c].transform.rotation);
            elevatorBottom.transform.parent = this.gameObject.transform;
            roomMap[r, f + 1, c] = elevatorBottom;
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
