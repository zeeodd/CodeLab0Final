using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobaController : MonoBehaviour
{
    public int order;

    private void OnTriggerEnter(Collider other)
    {
        gameObject.SetActive(false);
    }
}
