using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        GameObject playerObject = (GameObject)Instantiate(Resources.Load("Player"), new Vector3(13, 2, -14), Quaternion.identity);
    }

    private void GenerateLevel()
    {
        //modules = new Module[(levelScale.x * 2) - 1, levelScale.y, (levelScale.z * 2) - 1];
        Module startModule = Instantiate(StartModule, transform.position, transform.rotation);
        List<Exit> pendingExits = new List<Exit>(startModule.GetExits());
        int Iterations = 8;

        for (int iteration = 0; iteration < Iterations; iteration++)
        {
            var newExits = new List<Exit>();

            foreach (var pendingExit in pendingExits)
            {
                var newTag = GetRandom(pendingExit.Tags);
                var newModulePrefab = GetRandomWithTag(ModulePrefabs, newTag);
                var newModule = (Module)Instantiate(newModulePrefab);

                if (!CheckForOverlap(newModule))
                {
                    var newModuleExits = newModule.GetExits();
                    var exitToMatch = GetRandom(newModuleExits);
                    MatchExits(pendingExit, exitToMatch);
                    newExits.AddRange(newModuleExits.Where(e => e != exitToMatch));
                }
                else
                {
                    Destroy(newModule);
                }
            }

            pendingExits = newExits;
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

    private bool CheckForOverlap(Module newModule)
    {
        Collider[] hitColliders = Physics.OverlapSphere(newModule.transform.position, 1);
        return false;
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
