using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    private Vector3Int levelScale = new Vector3Int(5, 1, 5);
    private List<Module> modules;

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
        modules = new List<Module>();
        Module startModule = Instantiate(StartModule, transform.position, transform.rotation);
        List<Exit> pendingExits = new List<Exit>(startModule.GetExits());
        int Iterations = 5;

        for (int iteration = 0; iteration < Iterations; iteration++)
        {
            var newExits = new List<Exit>();

            foreach (var pendingExit in pendingExits)
            {
                var newTag = GetRandom(pendingExit.Tags);
                var newModulePrefab = GetRandomWithTag(ModulePrefabs, newTag);
                var newModule = (Module)Instantiate(newModulePrefab);

                var newModuleExits = newModule.GetExits();
                var exitToMatch = GetRandom(newModuleExits);
                MatchExits(pendingExit, exitToMatch);

                string output = $"Module position: {newModule.transform.position}";
                if (CheckIntersect(newModule))
                {
                    output += " <color=red>OVERLAP!!</color>";
                    Debug.Log(output);
                    newModule.GetComponent<ShowBounds>().color = Color.red;
                    //Destroy(newModule.gameObject);
                }

                newExits.AddRange(newModuleExits.Where(e => e != exitToMatch));
                modules.Add(newModule);
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

    private bool CheckIntersect(Module newModule)
    {
        Collider collider1 = newModule.gameObject.GetComponent<Collider>();
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Corridor");
        foreach (GameObject obj in objs.Where(x => x != newModule))
        {
            Collider collider2 = obj.GetComponent<Collider>();
            if (collider2.bounds.Intersects(collider1.bounds))
            {
                //if (newModule.gameObject.GetInstanceID() !=  obj.GetInstanceID())
                //{
                    return true;
                //}
            }
        }
        return false;
    }

    private bool CheckForOverlap(Module newModule)
    {
        Rigidbody rb = newModule.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Collider[] hitColliders = Physics.OverlapSphere(rb.centerOfMass, 1f, 9);
            if (hitColliders.Count() > 2)
            {
                return true;
            }
        }
        return false;
        //if (modules.Any(x => x.transform.position == newModule.transform.position))
        //{
        //    return true;
        //}

        //return false;
    }
}
