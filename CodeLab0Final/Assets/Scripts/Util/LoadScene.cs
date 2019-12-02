using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    // Load the scene with the name s
    public void LoadNewScene(string s)
    {
        SceneManager.LoadScene(s);
    }
}
