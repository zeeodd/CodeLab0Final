using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayController : MonoBehaviour
{
    // Bool to call when loading the array
    public bool loadArray = false;

    // Gameobj for the boba's spawnpoint
    public GameObject bobaSpawnPoint;

    // Number of bobas to initialize 
    public float maxBobaCount;
    public float minBobaCount;
    private float bobaCount;

    // List of all bobas
    List<GameObject> bobaList = new List<GameObject>();

    void Update()
    {
        if (loadArray)
        {
            bobaCount = Random.Range(minBobaCount, maxBobaCount);

            for (int i = 0; i < bobaCount; i++)
            {
                // Load in a boba
                var boba = Instantiate(Resources.Load("Prefabs/Boba") as GameObject);

                // Snag a temp pos and offset each incoming boba
                var tempPos = bobaSpawnPoint.transform.position;
                tempPos.y += boba.transform.localScale.y * i;
                boba.transform.position = tempPos;

                // Modify each individual boba's data
                boba.GetComponent<BobaController>().order = i;
                boba.GetComponent<BobaController>().popOut = false;
                bobaList.Add(boba);

            }

            loadArray = false;
        }
    }
}
