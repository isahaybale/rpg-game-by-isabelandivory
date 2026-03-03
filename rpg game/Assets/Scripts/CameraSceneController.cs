using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class CameraSceneController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCam;
    [SerializeField] private string[] followScenes;

    private CinemachineConfiner2D confiner;

    private void Awake()
    {
        confiner = virtualCam.GetComponent<CinemachineConfiner2D>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        bool shouldFollow = false;

        foreach (string sceneName in followScenes)
        {
            if (scene.name == sceneName)
            {
                shouldFollow = true;
                break;
            }
        }

        if (shouldFollow)
        {
            virtualCam.gameObject.SetActive(true);

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                virtualCam.Follow = player.transform;

            // 🔥 THIS PART SOLVES YOUR PROBLEM
            GameObject bounds = GameObject.Find("CameraBounds");

            if (bounds != null)
            {
                confiner.m_BoundingShape2D =
                    bounds.GetComponent<Collider2D>();

                confiner.InvalidateCache();
            }
        }
        else
        {
            virtualCam.Follow = null;
            virtualCam.gameObject.SetActive(false);
        }
    }
}