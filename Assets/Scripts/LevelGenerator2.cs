using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGenerator2 : MonoBehaviour
{
    [SerializeField]
    public bool activateGenerator;
    [SerializeField]
    public Module[] ModulePrefabs;
    [SerializeField]
    public Module StartModule;

    [SerializeField]
    private Vector3Int levelScale = new Vector3Int(5, 1, 5);
    private List<Module> modules;
    private List<Exit> pendingExits;
    private int currentIteration = 0;

    private void Start()
    {
        modules = new List<Module>();
        var newModulePrefab = GetRandomWithTag(ModulePrefabs, "Room");
        Module startModule = Instantiate(newModulePrefab, transform.position, transform.rotation);
        startModule.transform.parent = this.gameObject.transform;
        pendingExits = new List<Exit>(startModule.GetExits());
        //GameObject playerObject = (GameObject)Instantiate(Resources.Load("Player"), new Vector3(13, 2, -14), Quaternion.identity);
    }

    private void Update()
    {
        if (activateGenerator)
        {
            IterateLevelGeneration();
            currentIteration++;
            activateGenerator = false;
        }
    }

    private void IterateLevelGeneration()
    {
        var newExits = new List<Exit>();
        foreach (var pendingExit in pendingExits)
        {
            var newTag = GetRandom(pendingExit.Tags);
            var newModulePrefab = GetRandomWithTag(ModulePrefabs, newTag);
            var newModule = (Module)Instantiate(newModulePrefab);
            newModule.transform.parent = this.gameObject.transform;

            var newModuleExits = newModule.GetExits();
            var exitToMatch = GetRandom(newModuleExits);
            MatchExits(pendingExit, exitToMatch);

            string output = $"Module position: {newModule.transform.position}";
            if (CheckIntersect(newModule))
            {
                output += " <color=red>OVERLAP!!</color>";
                Debug.Log(output);
                newModule.GetComponent<ShowBounds>().color = Color.red;
                //newModule.gameObject.SetActive(false);
                //Destroy(newModule.gameObject);
            }

            newExits.AddRange(newModuleExits.Where(e => e != exitToMatch));
            modules.Add(newModule);
        }
        pendingExits = newExits;
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

    private bool CheckIntersect(Module newModule)
    {
        //Renderer renderer1 = newModule.gameObject.GetComponent<Renderer>();
        Collider[] hits = Physics.OverlapBox(newModule.transform.position, (newModule.transform.position / 2));
        if (hits.Count() > 0)
        {
            return true;
        }
        //GameObject[] objs = GameObject.FindGameObjectsWithTag("Corridor");
        //foreach (GameObject obj in objs.Where(x => x != newModule))
        //{
        //    Collider collider2 = obj.GetComponent<Collider>();
        //    if (collider2.bounds.Intersects(collider1.bounds))
        //    {
        //        int id1 = newModule.gameObject.GetInstanceID();
        //        int id2 = obj.GetInstanceID();
        //        if (id1 != id2)
        //        {
        //            return true;
        //        }
        //    }
        //}
        return false;
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
