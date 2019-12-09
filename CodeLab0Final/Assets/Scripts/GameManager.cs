using System.Collections;
using System.Collections.Generic;
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

    public Material active;
    public Material inactive;

    private Vector3 uprightCupRot = new Vector3(0f, 0f, 8.4f);
    private int turnOrder = 1;

    // Animator for the camera
    Animator cameraAnimator;

    // List of all players
    List<Player> playerList = new List<Player>();

    // This is a Player class for easy storage of important information
    public class Player
    {
        public string Name;
        public int Order;
        public int Score;
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
                Score = 0,
                Label = canvasController.P1Input
            };

            Player p2 = new Player
            {
                Name = canvasController.P2Input.GetComponent<Text>().text,
                Order = int.Parse(canvasController.P2Order.GetComponent<Text>().text),
                Score = 0,
                Label = canvasController.P2Input
            };

            Player p3 = new Player
            {
                Name = canvasController.P3Input.GetComponent<Text>().text,
                Order = int.Parse(canvasController.P3Order.GetComponent<Text>().text),
                Score = 0,
                Label = canvasController.P3Input
            };

            Player p4 = new Player
            {
                Name = canvasController.P4Input.GetComponent<Text>().text,
                Order = int.Parse(canvasController.P4Order.GetComponent<Text>().text),
                Score = 0,
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

        if (cameraController.setUpGame)
        {
            arrayController.loadArray = true;
            cameraController.setUpGame = false;
        }

        // If an input is registered, increment the turn order
        // But reset the turn order if on the last player
        if (arrayController.incrementTurn)
        {
            if (turnOrder == 4)
            {
                turnOrder = 1;
            }
            else
            {
                turnOrder++;
            }

            // Set the bool back to false
            arrayController.incrementTurn = false;
        }

        // Check the order of each player and assign them the correct position on the canvas
        foreach (Player player in playerList)
        {
            if (player.Order == turnOrder)
            {
                player.Label.transform.parent.GetComponent<Image>().material = active;
            }
            else
            {
                player.Label.transform.parent.GetComponent<Image>().material = inactive;
            }
        }
    }

}
