using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject canvas; // Grab the canvas object
    public bool setUpGame = false;

    void ActivateCanvas() // Animation event to trigger once the camera pan is done
    {
        canvas.SetActive(true);
    }

    void LoadBobaArray() // Animation event to trigger once the camera pan is done
    {
        setUpGame = true;
    }
}
