using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrawTopBuffer : MonoBehaviour
{
    public GameObject lastBoba;
    public ArrayController arrayController;
    public bool bobaAtTop = true;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boba") || other.CompareTag("BadBoba"))
        {
            if (bobaAtTop == false)
            {
                bobaAtTop = true;
            }
        }
    }
}
