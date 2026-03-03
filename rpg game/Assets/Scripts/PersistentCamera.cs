using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentCamera : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}