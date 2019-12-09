using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    public GameObject canvas; // Grab the canvas object
    public bool setUpGame = false;
    public float rotateSpeed; // Skybox rotation speed

    void ActivateCanvas() // Animation event to trigger once the camera pan is done
    {
        canvas.SetActive(true);
    }

    void LoadBobaArray() // Animation event to trigger once the camera pan is done
    {
        setUpGame = true;
    }

    private void Update()
    {
        // Rotate skybox for fun
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * rotateSpeed);
    }
}
