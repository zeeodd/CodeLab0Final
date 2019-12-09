using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    // Input boxes and order labels for each player, also the start button
    public GameObject opacityBackground;
    public GameObject P1Input;
    public GameObject P2Input;
    public GameObject P3Input;
    public GameObject P4Input;
    public GameObject orderBtn;
    public GameObject P1Order;
    public GameObject P2Order;
    public GameObject P3Order;
    public GameObject P4Order;
    public GameObject startBtn;
    public GameObject loadingLabel;

    // Used for triggering the actual ordering of players after a short anim
    public bool readyForOrdering = false;

    // Bool for creating players
    public bool savePlayers = false;
    public bool clearUI = false;

    // Anim vars
    private float scrambleTimer = 3f;
    private int scrambleInt = 1;
    private float frameCounter = 0;
    private float frameSkip = 5f;

    // List of order values (1, 2, 3, 4) which will be assigned to each player
    private List<string> orderValues = new List<string>();

    void Start()
    {
        // Hook up all of the important player variables
        opacityBackground = transform.GetChild(0).gameObject;
        P1Input = transform.GetChild(1).transform.GetChild(1).gameObject;
        P2Input = transform.GetChild(2).transform.GetChild(1).gameObject;
        P3Input = transform.GetChild(3).transform.GetChild(1).gameObject;
        P4Input = transform.GetChild(4).transform.GetChild(1).gameObject;
        orderBtn = transform.GetChild(5).gameObject;
        P1Order = transform.GetChild(6).gameObject;
        P2Order = transform.GetChild(7).gameObject;
        P3Order = transform.GetChild(8).gameObject;
        P4Order = transform.GetChild(9).gameObject;
        startBtn = transform.GetChild(10).gameObject;
        loadingLabel = transform.GetChild(11).gameObject;

        // Set the order/start button inactive at the start
        // Only want players to start ordering when all names have been entered
        orderBtn.SetActive(false);
        startBtn.SetActive(false);
        loadingLabel.SetActive(false);

        // Initialize the order list with the strings to be assigned
        orderValues.Add("1");
        orderValues.Add("2");
        orderValues.Add("3");
        orderValues.Add("4");

    }

    void Update()
    {
        // Only set the start button active if all of the input boxes are filled
        if (P1Input.GetComponent<Text>().text != "" && P2Input.GetComponent<Text>().text != ""
        && P3Input.GetComponent<Text>().text != "" && P4Input.GetComponent<Text>().text != "")
        {
            orderBtn.SetActive(true);
        }
        else
        {
            orderBtn.SetActive(false);
        }

        // Once the start button is clicked, enter this loop
        if (readyForOrdering)
        {
            // Set the start button inactive so it can't be clicked again
            orderBtn.SetActive(false);

            // Set all the input fields as read only and lock them in
            P1Input.GetComponentInParent<InputField>().readOnly = true;
            P2Input.GetComponentInParent<InputField>().readOnly = true;
            P3Input.GetComponentInParent<InputField>().readOnly = true;
            P4Input.GetComponentInParent<InputField>().readOnly = true;

            // Incredment the framecounter for the number scramble anim
            // But decrement the scramble timer so it doesn't last forever
            frameCounter += Time.deltaTime;
            scrambleTimer -= Time.deltaTime;

            // This if-statement really just scrambles the labels from 1 to 4
            // At a controleld rate dictated by frameSkip
            if (frameCounter > Time.deltaTime * frameSkip)
            {
                scrambleInt++;

                P1Order.GetComponent<Text>().text = scrambleInt.ToString();
                P2Order.GetComponent<Text>().text = scrambleInt.ToString();
                P3Order.GetComponent<Text>().text = scrambleInt.ToString();
                P4Order.GetComponent<Text>().text = scrambleInt.ToString();

                // If the scramble int hits 4, reset it to 1
                if (scrambleInt == 4)
                {
                    scrambleInt = 1;
                }
                frameCounter = 0;
            }

            // Once the anim has run its course, actually order the players
            if (scrambleTimer < 0)
            {
                SetOrder();
            }
        }

        if (clearUI)
        {
            // Disable both buttons
            startBtn.SetActive(false);
            orderBtn.SetActive(false);

            // Disable the ordering labels
            P1Order.SetActive(false);
            P2Order.SetActive(false);
            P3Order.SetActive(false);
            P4Order.SetActive(false);

            // Disable the opacity background
            opacityBackground.SetActive(false);

        }
    }

    // Function to be called once the players hit start (and after they have
    // entered all of their names)
    public void StartOrdering()
    {
        readyForOrdering = true;
    }

    // This function orders the players
    public void SetOrder()
    {
        // Set this bool to false so it stops animating
        readyForOrdering = false;

        // Shuffle the list of orders
        ShuffleList(orderValues);

        // Assign each player an order
        P1Order.GetComponent<Text>().text = orderValues[0];
        P2Order.GetComponent<Text>().text = orderValues[1];
        P3Order.GetComponent<Text>().text = orderValues[2];
        P4Order.GetComponent<Text>().text = orderValues[3];

        // Set the start button active
        startBtn.SetActive(true);
    }

    // This function shuffles a given list
    void ShuffleList(List<string> list)
    {
        var count = list.Count;
        for (var i = 0; i < count - 1; ++i)
        {
            var r = Random.Range(i, count);
            var tmp = list[i];
            list[i] = list[r];
            list[r] = tmp;
        }
    }

    public void StartGame()
    {
        savePlayers = true;
        clearUI = true;
    }

}
