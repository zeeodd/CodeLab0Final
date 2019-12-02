using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteRotate : MonoBehaviour
{
    public bool shouldBeSpinning; // Easy toggle so it can stop spinning
    public float rotateSpeed = 1f; // How fast game obj should spin

    public bool spinX;
    public bool spinY;
    public bool spinZ;

    void Awake()
    {
        shouldBeSpinning = true; // Set to true in the beginning
    }

    void Update()
    {
        // Only spin if the bool flag is up
        if (shouldBeSpinning)
        {
            var xRotate = (spinX) ? rotateSpeed : 0f;
            var yRotate = (spinY) ? rotateSpeed : 0f;
            var zRotate = (spinZ) ? rotateSpeed : 0f;

            transform.Rotate(xRotate, yRotate, zRotate, Space.Self);
        }
    }
}
