using IngameDebugConsole;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCommands : MonoBehaviour
{
    [ConsoleMethod("teleport", "Teleports gameobject of {name} to {position}")]
    public static void TeleportPlayerTo(string name, Vector3 position)
    {
        GameObject player = GameObject.Find("Player");
        player.transform.position = position;
    }
}
