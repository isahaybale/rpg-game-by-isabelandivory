using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootstrapLoader : MonoBehaviour
{
    [SerializeField] private string firstSceneName = "HouseExterior";

    private void Start()
    {
        StartCoroutine(LoadFirstScene());
    }

    private System.Collections.IEnumerator LoadFirstScene()
    {
        yield return null; // wait one frame
        SceneManager.LoadScene(firstSceneName);
    }
}