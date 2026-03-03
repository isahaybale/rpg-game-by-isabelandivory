using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorTrigger : MonoBehaviour
{
    [Header("Scene Settings")]
    public string sceneToLoad;
    public string spawnPointToUse;

    [Header("Transition Settings")]
    [SerializeField] private bool useFade = true;   // 👈 Toggle in Inspector

    private bool canTrigger = false;

    void Start()
    {
        Invoke(nameof(EnableTrigger), 0.5f);
    }

    void EnableTrigger()
    {
        canTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!canTrigger) return;

        if (other.CompareTag("Player"))
        {
            DoorManager.spawnPointName = spawnPointToUse;

            if (useFade)
            {
                UIManager.Instance.TransitionToScene(sceneToLoad);
            }
            else
            {
                SceneManager.LoadScene(sceneToLoad);
            }
        }
    }
}