using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Controller objects we need to keep track of
    public CanvasController canvasController;
    public CameraController cameraController;
    public ArrayController arrayController;

    // Gameobjects we need to keep track of
    public GameObject cup;
    public GameObject tinyStar;
    public GameObject bigStar;

    // Materials for the bobas
    public Material active;
    public Material inactive;

    private string loading = "Don't Sip Yet...";
    private string ready = "Okay, Sip Away!";
    private string playerLost = "That's Some Bad Boba.";

    private Vector3 uprightCupRot = new Vector3(0f, 0f, 8.4f);
    private int turnOrder = 1;
    private int lastTurn;
    private int upperPlayerRange = 3;
    private int lowerPlayerRange = 0;

    // Animator for the camera
    Animator cameraAnimator;

    // List of all players
    List<Player> playerList = new List<Player>();

    // This is a Player class for easy storage of important information
    public class Player
    {
        public string Name;
        public int Order;
        public bool Alive;
        public GameObject Label;
    }

    // Ordered Vector2 positions
    private Vector2 p1anchorMin = new Vector2(0.15f, 0.1f);
    private Vector2 p1anchorMax = new Vector2(0.15f, 0.1f);

    private Vector2 p2anchorMin = new Vector2(0.385f, 0.1f);
    private Vector2 p2anchorMax = new Vector2(0.385f, 0.1f);

    private Vector2 p3anchorMin = new Vector2(0.615f, 0.1f);
    private Vector2 p3anchorMax = new Vector2(0.615f, 0.1f);

    private Vector2 p4anchorMin = new Vector2(0.85f, 0.1f);
    private Vector2 p4anchorMax = new Vector2(0.85f, 0.1f);

    void Update()
    {
        // If the game starts, init the four player variables with name, order, and score
        if (canvasController.savePlayers)
        {

            Player p1 = new Player
            {
                Name = canvasController.P1Input.GetComponent<Text>().text,
                Order = int.Parse(canvasController.P1Order.GetComponent<Text>().text),
                Alive = true,
                Label = canvasController.P1Input
            };

            Player p2 = new Player
            {
                Name = canvasController.P2Input.GetComponent<Text>().text,
                Order = int.Parse(canvasController.P2Order.GetComponent<Text>().text),
                Alive = true,
                Label = canvasController.P2Input
            };

            Player p3 = new Player
            {
                Name = canvasController.P3Input.GetComponent<Text>().text,
                Order = int.Parse(canvasController.P3Order.GetComponent<Text>().text),
                Alive = true,
                Label = canvasController.P3Input
            };

            Player p4 = new Player
            {
                Name = canvasController.P4Input.GetComponent<Text>().text,
                Order = int.Parse(canvasController.P4Order.GetComponent<Text>().text),
                Alive = true,
                Label = canvasController.P4Input
            };

            // Add all the players to a list for future ease of access
            playerList.Add(p1);
            playerList.Add(p2);
            playerList.Add(p3);
            playerList.Add(p4);

            // Check the order of each player and assign them the correct position on the canvas
            foreach(Player player in playerList)
            {

                if (player.Order == 1)
                {
                    player.Label.transform.parent.GetComponent<RectTransform>().anchorMin = p1anchorMin;
                    player.Label.transform.parent.GetComponent<RectTransform>().anchorMax = p1anchorMax;
                    player.Label.transform.parent.GetComponent<Image>().material = active;
                }

                if (player.Order == 2)
                {
                    player.Label.transform.parent.GetComponent<RectTransform>().anchorMin = p2anchorMin;
                    player.Label.transform.parent.GetComponent<RectTransform>().anchorMax = p2anchorMax;
                    player.Label.transform.parent.GetComponent<Image>().material = inactive;
                }

                if (player.Order == 3)
                {
                    player.Label.transform.parent.GetComponent<RectTransform>().anchorMin = p3anchorMin;
                    player.Label.transform.parent.GetComponent<RectTransform>().anchorMax = p3anchorMax;
                    player.Label.transform.parent.GetComponent<Image>().material = inactive;
                }

                if (player.Order == 4)
                {
                    player.Label.transform.parent.GetComponent<RectTransform>().anchorMin = p4anchorMin;
                    player.Label.transform.parent.GetComponent<RectTransform>().anchorMax = p4anchorMax;
                    player.Label.transform.parent.GetComponent<Image>().material = inactive;
                }
            }

            // Stop other animations
            tinyStar.GetComponent<InfiniteRotate>().shouldBeSpinning = false;
            bigStar.GetComponent<InfiniteRotate>().shouldBeSpinning = false;
            tinyStar.gameObject.SetActive(false);
            bigStar.gameObject.SetActive(false);
            cup.GetComponent<InfiniteRotate>().shouldBeSpinning = false;
            cup.transform.eulerAngles = uprightCupRot;


            // Move the camera to a new pos
            cameraAnimator = cameraController.gameObject.GetComponent<Animator>();
            cameraAnimator.SetBool("ReposCamera", true);

            // Set this bool false so it runs only once
            canvasController.savePlayers = false;
        }
        // END OF GAME SET UP

        // Add the bobas when the camera has
        if (cameraController.setUpGame)
        {
            arrayController.loadArray = true;
            cameraController.setUpGame = false;
            canvasController.loadingLabel.gameObject.SetActive(true);
            canvasController.loadingLabel.GetComponent<Text>().text = ready;
        }

        // This controls the turn increments after every move
        if (arrayController.incrementTurn)
        {
            IncrementTurn();
        }

        print("CURRENT TURN: " + turnOrder + " // " + "LAST TURN: " + lastTurn);

        // Flip the text depending on whether the array is getting updated or not
        // To let the player know if they can input anything
        if (arrayController.gameIsUnderway)
        {
            if (!arrayController.bobasMoved && !arrayController.sippedBadBoba)
            {
                canvasController.loadingLabel.GetComponent<Text>().text = loading;
            }
            else if (arrayController.bobasMoved && !arrayController.sippedBadBoba)
            {
                canvasController.loadingLabel.GetComponent<Text>().text = ready;
            }
            else if (arrayController.sippedBadBoba) //  If a player has lost
            {
                DeletePlayer();
                canvasController.loadingLabel.GetComponent<Text>().text = playerLost;
                Invoke("ResetBadBoba", 2f);
            }
        }
    }

    void IncrementTurn()
    {
        // Save the last turn
        lastTurn = turnOrder;

        // Increment the turn order
        turnOrder++;

        // If we hit the upper boundary, reset
        if (turnOrder > upperPlayerRange || turnOrder < lowerPlayerRange)
        {
            turnOrder = lowerPlayerRange;
        }

        // If the current player is dead, move to the next player
        if (!playerList[turnOrder - 1].Alive)
        {
            turnOrder++;
        }

        var alivePlayers = new List<int>();

        // Check the order of each player and assign them the correct material
        foreach (Player player in playerList)
        {
            if (player.Order == turnOrder && player.Alive)
            {
                player.Label.transform.parent.GetComponent<Image>().material = active;
            }
            else if (player.Order != turnOrder && player.Alive)
            {
                player.Label.transform.parent.GetComponent<Image>().material = inactive;
            }

            if (player.Alive)
            {
                alivePlayers.Add(player.Order);
            }
        }

        // Re-assign the boundaries of movement within turn order
        lowerPlayerRange = alivePlayers.Min();
        upperPlayerRange = alivePlayers.Max();

        print(lowerPlayerRange);
        print(upperPlayerRange);

        // End the bool
        arrayController.incrementTurn = false;
    }

    // Just reset this bool so the bad boba text returns to normal
    void ResetBadBoba()
    {
        arrayController.sippedBadBoba = false;
    }

    // Deletes a player who drinks some bad boba
    void DeletePlayer()
    {
        foreach (Player player in playerList)
        {

            if (player.Order == lastTurn)
            {
                player.Label.transform.parent.gameObject.SetActive(false);
                player.Alive = false;
            }

        }
    }

}
