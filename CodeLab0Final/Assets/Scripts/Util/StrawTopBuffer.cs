using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrawTopBuffer : MonoBehaviour
{
    public string bobaTag;
    public GameObject lastBoba;
    public bool bobaAtTop = true;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(bobaTag))
        {
            if (bobaAtTop == false)
            {
                bobaAtTop = true;
            }
        }
    }
}
