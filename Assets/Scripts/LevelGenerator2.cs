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
    private Vector3Int levelScale = new Vector3Int(5, 1, 5);
    private Room[,,] roomMap;
    private List<Tuple<Vector3Int, Vector3Int>> corridors;

    private void Start()
    {
        GenerateLevel();
    }

    private void GenerateLevel()
    {
        int floors = levelScale.y;
        int rows = levelScale.x;
        int columns = levelScale.z;

        roomMap = new Room[rows, floors, columns];
        for (int f = 0; f < roomMap.GetLength(1); f++)
        {
            for (int c = 0; c < roomMap.GetLength(2); c++)
            {
                for (int r = 0; r < roomMap.GetLength(0); r++)
                {
                    Vector3Int newRoomCoord = new Vector3Int(r, f, c);
                    Room newRoomPrefab = GetRandomWithTag(ModulePrefabs, "Room") as Room;
                    newRoomPrefab.Coordinate = newRoomCoord;
                    Room newRoom = Instantiate(newRoomPrefab);
                    newRoom.transform.parent = this.gameObject.transform;
                    roomMap[r, f, c] = newRoom;

                    foreach (Vector3Int neighborCoord in newRoom.GetNeighborCoordinates(Vector3Int.zero, levelScale - Vector3Int.one))
                    {
                        if (!corridors.Any(x => (x.Item1 == newRoomCoord && x.Item2 == neighborCoord) || (x.Item1 == neighborCoord && x.Item2 == newRoomCoord)))
                        {
                            corridors.Add(new Tuple<Vector3Int, Vector3Int>(newRoomCoord, neighborCoord));

                            Exit newRoomExit;
                            if (neighborCoord.x < newRoomCoord.x) //North
                            {
                                newRoomExit = newRoom.GetExit(Direction.N);
                            }
                            else if (neighborCoord.x > newRoomCoord.x) //South
                            {
                                newRoomExit = newRoom.GetExit(Direction.S);
                            }
                            else if (neighborCoord.z < newRoomCoord.z) //East
                            {
                                newRoomExit = newRoom.GetExit(Direction.E);
                            }
                            else if (neighborCoord.z > newRoomCoord.z) //West
                            {
                                newRoomExit = newRoom.GetExit(Direction.W);
                            }
                            else
                            {

                            }
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
