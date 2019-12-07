using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayController : MonoBehaviour
{
    // Bool to call when loading the array
    public bool loadArray = false;
    public bool poppedOut = false;
    public bool arrayUpdated = true;

    // Gameobj for the boba's spawnpoint
    public GameObject bobaSpawnPoint;

    // Boba Controller script
    BobaController bobaController;

    // Number of bobas to initialize 
    public int bobaCount;
    public float thrust;

    // List of all bobas
    List<GameObject> bobaList = new List<GameObject>();
    private GameObject lastBoba;

    void Update()
    {
        // Initialize this array only when prompted by this bool
        if (loadArray)
        {

            for (int i = 0; i < bobaCount; i++)
            {
                // Load in a boba
                var boba = Instantiate(Resources.Load("Prefabs/Boba") as GameObject);

                // Snag a temp pos and offset each incoming boba
                var tempPos = bobaSpawnPoint.transform.position;
                tempPos.y += boba.transform.localScale.y * i;
                boba.transform.position = tempPos;

                // Modify each individual boba's data
                boba.GetComponent<BobaController>().order = bobaCount - i;
                bobaList.Add(boba);

            }

            loadArray = false;
        }

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            // Only call this function if no boba are currently popped out and the array is updated
            if(!poppedOut && arrayUpdated)
            {
                // Find the first boba and push it out
                foreach (GameObject boba in bobaList)
                {
                    if (boba.GetComponent<BobaController>().order == 1)
                    {
                        bobaController = boba.GetComponent<BobaController>();
                        boba.GetComponent<Rigidbody>().AddForce(0, thrust, 0, ForceMode.Impulse);
                        poppedOut = true;
                        lastBoba = boba;
                        arrayUpdated = false;
                    }
                }
            }
        }

        // TODO: Add input control for hitting two bobas

        // Invoke this method if a boba is popped so players can see the animation
        if(poppedOut)
        {
            Invoke("UpdateArray", 1f);
            poppedOut = false;
        }
    }


    // This function updates the array once the boba has been popped out
    void UpdateArray()
    {
        bobaList.Remove(lastBoba); // Remove the boba that was popped out
        Destroy(lastBoba); // Destroy the boba that was popped out

        // Reorder all the remaining bobas in the list
        foreach (GameObject boba in bobaList)
        {
            boba.GetComponent<BobaController>().order -= 1;
        }

        // TODO: Move the array of bobas upward after each boba is tossed out

        // Set back to true so players can input functions again
        arrayUpdated = true;
    }
}
