using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject canvas; // Grab the canvas object

    void ActivateCanvas() // Animation event to trigger once the camera pan is done
    {
        canvas.SetActive(true);
        print("Canvas is now active!");
    }
}
