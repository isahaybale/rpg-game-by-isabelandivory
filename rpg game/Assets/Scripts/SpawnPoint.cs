using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnPoint : MonoBehaviour
{
    public string spawnID;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (DoorManager.spawnPointName == spawnID)
        {
            player.transform.position = transform.position;
        }
    }
}