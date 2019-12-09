using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayController : MonoBehaviour
{
    // Bool to call when loading the array
    public bool loadArray = false;
    public bool poppedOut = false;
    public bool arrayUpdated = true;
    public bool bobasMoved = true;
    public bool firstPush = false;
    public bool incrementTurn = false;
    public bool gameIsUnderway = false;
    public bool sippedBadBoba = false;
    private float timeElapsed = 0f;

    // Gameobj for the boba's spawnpoint
    public GameObject bobaSpawnPoint;
    public GameObject strawBottom;
    public GameObject strawTop;

    // Number of bobas to initialize 
    public int bobaCount;
    public float thrust;
    public int badBoba1;
    public int badBoba2;
    public int badBoba3;
    public int upperRandom;
    public int lowerRandom;

    // List of all bobas
    List<GameObject> bobaList = new List<GameObject>();
    private GameObject lastBoba;
    private GameObject secondToLastBoba;
    private int bobaToSubtract;

    void Update()
    {
        // Initialize this array only when prompted by this bool
        if (loadArray)
        {
            // Randomly assign three bad boba...
            badBoba1 = bobaCount - Random.Range(lowerRandom, upperRandom); // Offset the first one from the back (since we don't want a bad one upfront)
            badBoba2 = badBoba1 - Random.Range(lowerRandom, upperRandom); // Just pick another random one for the second
            // If the last boba were to be too close to the first round of play so that
            // it's possible for player 3 or 4 to lose automatically, pick the safe zone
            var lastNum = Random.Range(lowerRandom, upperRandom);
            var safeZone = Random.Range(lowerRandom, upperRandom);
            badBoba3 = (badBoba2 - lastNum < safeZone) ? safeZone : badBoba2 - lastNum;

            for (int i = 0; i < bobaCount; i++)
            {
                // Load in a boba
                var boba = Instantiate(Resources.Load("Prefabs/Boba") as GameObject);

                // Change the bad boba's materials
                if (i == badBoba1 || bobaCount - i == badBoba2 || bobaCount - i == badBoba3)
                {
                    boba.GetComponent<MeshRenderer>().material = boba.GetComponent<MeshRenderer>().materials[1];
                    boba.tag = "BadBoba";
                }
                else
                {
                    boba.GetComponent<MeshRenderer>().material = boba.GetComponent<MeshRenderer>().materials[0];
                }

                // Snag a temp pos and offset each incoming boba
                var tempPos = bobaSpawnPoint.transform.position;
                tempPos.y += boba.transform.localScale.y * i;
                boba.transform.position = tempPos;

                // Modify each individual boba's data
                boba.GetComponent<BobaController>().order = bobaCount - i;
                bobaList.Add(boba);

            }

            loadArray = false;
            gameIsUnderway = true;
        }

        if (gameIsUnderway)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                // Only call this function if no boba are currently popped out and the array is updated
                if (!poppedOut && arrayUpdated && bobasMoved)
                {
                    incrementTurn = true;
                    bobaToSubtract = 1;
                    PopOutOneBoba();
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                if (!poppedOut && arrayUpdated && bobasMoved)
                {
                    incrementTurn = true;
                    bobaToSubtract = 2;
                    PopOutTwoBoba();
                }
            }
        }

        if (!bobasMoved)
        {
            MoveStrawBottom();
        }

        // Invoke this method if a boba is popped so players can see the animation
        if (poppedOut)
        {
            Invoke("UpdateArray", 1f);
            poppedOut = false;
        }
    }


    // This function updates the array once the boba has been popped out
    void UpdateArray()
    {
        if (bobaToSubtract == 1)
        {
            // Check if player drank a bad boba
            if (lastBoba.GetComponent<MeshRenderer>().material == lastBoba.GetComponent<MeshRenderer>().materials[1])
            {
                sippedBadBoba = true;
            }
            bobaList.Remove(lastBoba); // Remove the boba that was popped out
            Destroy(lastBoba); // Destroy the boba that was popped out

        } else if (bobaToSubtract == 2)
        {

            if (lastBoba.GetComponent<MeshRenderer>().material == lastBoba.GetComponent<MeshRenderer>().materials[1])
            {
                sippedBadBoba = true;
            } 
            else if (secondToLastBoba.GetComponent<MeshRenderer>().material == secondToLastBoba.GetComponent<MeshRenderer>().materials[1])
            {
                sippedBadBoba = true;
            }
            bobaList.Remove(lastBoba);
            Destroy(lastBoba);
            bobaList.Remove(secondToLastBoba); // Also remove the second to last boba
            Destroy(secondToLastBoba);
        }

        // Reorder all the remaining bobas in the list
        foreach (GameObject boba in bobaList)
        {
            boba.GetComponent<BobaController>().order -= bobaToSubtract;
        }

        // Set back to true so players can input functions again
        arrayUpdated = true;
    }

    // This function physically moves the boba out of the straw
    void MoveStrawBottom()
    {
        // Check if there's not a boba at the top
        if (!strawTop.GetComponent<StrawTopBuffer>().bobaAtTop)
        {
            // If it's the first push, add a little more force to this push
            if (!firstPush)
            {
                strawBottom.transform.Translate(new Vector3(0, 1f, 0) * Time.deltaTime, Space.World);
                timeElapsed += Time.deltaTime;
            }
            else
            {
                strawBottom.transform.Translate(new Vector3(0, 0.1f, 0) * Time.deltaTime, Space.World);
            }

            // Only add the extra force for a second
            if (timeElapsed > 1f)
            {
                firstPush = true;
            }

        }
        else
        {
            bobasMoved = true;
        }
    }

    void PopOutOneBoba()
    {
        // Find the first boba and push it out
        foreach (GameObject boba in bobaList)
        {
            if (boba.GetComponent<BobaController>().order == 1)
            {
                boba.GetComponent<Rigidbody>().AddForce(0, thrust, 0, ForceMode.Impulse);
                poppedOut = true;
                lastBoba = boba;
                strawTop.GetComponent<StrawTopBuffer>().bobaAtTop = false;
                arrayUpdated = false;
                bobasMoved = false;
            }
        }
    }

    void PopOutTwoBoba()
    {
        // Find the first boba and push it out
        foreach (GameObject boba in bobaList)
        {
            if (boba.GetComponent<BobaController>().order == 1)
            {
                boba.tag = "Untagged";
                boba.GetComponent<Rigidbody>().AddForce(0, thrust, 0, ForceMode.Impulse);
                secondToLastBoba = boba;
            }

            if (boba.GetComponent<BobaController>().order == 2)
            {
                boba.tag = "Untagged";
                boba.GetComponent<Rigidbody>().AddForce(0, thrust * 1.5f, 0, ForceMode.Impulse);
                lastBoba = boba;
            }
        }
        poppedOut = true;
        strawTop.GetComponent<StrawTopBuffer>().bobaAtTop = false;
        arrayUpdated = false;
        bobasMoved = false;
    }

}
