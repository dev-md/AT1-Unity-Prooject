using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //Define delegate types and events here

    public Node CurrentNode { get; private set; }
    public Node TargetNode { get; private set; }

    [SerializeField] private float speed = 4;
    private bool moving = false;
    private Vector3 currentDir;

    //My added Vairables 
    private string moveDirNode;

    public Button uButton; //the list of buttons, Note: need to find better way to do this.
    public Button dButton;
    public Button lButton;
    public Button rButton;
    Dictionary<string, Button> listButtons = new Dictionary<string, Button>();
    //the list of buttons with the direction of them

    // Start is called before the first frame update
    void Start()
    {
        
        moveDirNode = null; //starting in no direction

        // Adds the Buttons with the direction.
        listButtons.Add("u", uButton);
        listButtons.Add("d", dButton);
        listButtons.Add("l", lButton);
        listButtons.Add("r", rButton);

        //Input Event to find what direction the player hit
        InputChecker.ConfrimDirInput += InputChecker_ConfrimDirInput;

        foreach (Node node in GameManager.Instance.Nodes)
        {
            if (node.Parents.Length > 2 && node.Children.Length == 0)
            {
                CurrentNode = node;
                CurrentNode.tag = "Player_Node"; // The Node that the player is on.
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (moving == false)
        {
            if (moveDirNode != null) //If it is a direction
            {
                //Debug.Log("HIT");
                //Changes the colour of the button and finds the node of the direction.
                ChangeButtonColour(listButtons[moveDirNode], Color.green);
                MoveToNode(FindClosest());
                moveDirNode = null; //Finish with the direction.
            }
        }
        else
        {

            if (moveDirNode != null) //if does have a direction and is not moving
            {
                //if the colour is not green
                if(listButtons[moveDirNode].GetComponent<Image>().color != Color.green)
                {
                    //change it to red.
                    ChangeButtonColour(listButtons[moveDirNode], Color.red);
                }
                moveDirNode = null; // Changes back to nonething.
            }

            if (Vector3.Distance(transform.position, TargetNode.transform.position) > 0.25f)
            {
                transform.Translate(currentDir * speed * Time.deltaTime);
            }
            else
            {
                moving = false;
                CurrentNode = TargetNode;
            }
        }
    }

    //Implement mouse interaction method here

    //The Colour changing method.
    private void ChangeButtonColour(Button button, Color colour)
    {
        button.GetComponent<Image>().color = colour;
        if(colour == Color.green) StartCoroutine(ColourTimer(button, 2.4f));
        if(colour == Color.red) StartCoroutine(ColourTimer(button, 0.5f));
    }

    //My Colour timer
    IEnumerator ColourTimer(Button button, float time)
    {
        yield return new WaitForSeconds(time);
        button.GetComponent<Image>().color = Color.white;
    }


    /// <summary>
    /// Sets the players target node and current directon to the specified node.
    /// </summary>
    /// <param name="node"></param>
    /// 

    //changed some things inside here,
    public void MoveToNode(Node node)
    {
        if (moving == false)
        {
            TargetNode = node;
            CurrentNode.tag = "Untagged"; //Player is not on the node
            TargetNode.tag = "Player_Node"; //Player is moving to the node
            currentDir = TargetNode.transform.position - transform.position;
            currentDir = currentDir.normalized;
            moving = true;
        }
    }

    //My only Finding the node in the direction,
    private Node FindClosest()
    {
        //Set vars that is needed.
        List<Node> listParentChild = new List<Node>();
        List<Node> listClose = new List<Node>(); //The nodes list.

        //What direction is the closest nodes.
        Dictionary<string, Node> whereNode = new Dictionary<string, Node>();
        //Checking closest nodess
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 10f);
        Node whichNode;


        // lists all nodes and adds them to a list
        foreach (Node node in GameManager.Instance.Nodes)
        {
            listParentChild.Add(node);
        }

        //Lists all in a sphere collider.
        foreach (Collider hitCol in hitColliders)
        {
            if (hitCol.transform.name.Contains("Node")) //If this hit is named node.
            {
                foreach (Node node in listParentChild) //List thru all of the nodes
                {
                    if (node.name == hitCol.transform.name) //If the name is the same. for both.
                    {
                        //Debug.Log(node.name + " and " + hitCol.transform.name);
                        //Addes node to the list.
                        listClose.Add(node);
                    }
                }
            }
        }

        //Removes the players current node.
        foreach (Node node in listParentChild)
        {
            if (node == CurrentNode)
            {
                listClose.Remove(node);
            }
        }
        //Clearing the dict list, to avoid errors.
        foreach (string x in new List<string> { "u", "d", "l", "r" })
        {
            whereNode[x] = CurrentNode;
        }
        //For each direction
        foreach (string x in new List<string> { "u", "d", "l", "r" })
        {
            //and for each node
            foreach (Node node in listClose)
            {
                //Debug.DrawRay(transform.position, transform.forward * 10f, Color.blue,3f);
                //Case statement the direction and see if the node is in the direction.
                switch (x)
                {
                    case "u":
                        //Ray cast to see if the node is in the direction
                        if(Physics.Raycast(transform.position, transform.forward * 10f, out RaycastHit otherU, 12f))
                        {
                            //if the nodes are the same from the list and the raycast.
                            if(otherU.transform.name == node.name)
                            {
                                //Add to direction.
                                whereNode["u"] = node;
                            }
                        }
                        break;
                    //repeat This for each direciton.
                    case "d":
                        if (Physics.Raycast(transform.position, transform.forward * -10f, out RaycastHit otherD, 12f))
                        {
                            if (otherD.transform.name == node.name)
                            {
                                whereNode["d"] = node;
                            }
                        }
                        break;
                    case "l":

                        if (Physics.Raycast(transform.position, transform.right * -10f, out RaycastHit otherL, 12f))
                        {
                            if (otherL.transform.name == node.name)
                            {
                                whereNode["l"] = node;
                            }
                        }
                        break;
                    case "r":
                        if (Physics.Raycast(transform.position, transform.right * 10f, out RaycastHit otherR, 12f))
                        {
                            if (otherR.transform.name == node.name)
                            {
                                whereNode["r"] = node;
                            }
                        }
                        break;
                }
            }
        }

        //Test what is the things.
        //foreach (KeyValuePair<string, Node> i in whereNode)
        //{
        //    Debug.Log(i);
        //}
        ////listClose.Add(CurrentNode.Children);

        //From the event that is called.
        //set the var from the list.
        whichNode = whereNode[moveDirNode];
        //Final return of the node.
        return whichNode;
    }

    //Listening on the event.
    private string InputChecker_ConfrimDirInput(string dirInput)
    {
        //Changes the global direction var.
        moveDirNode = dirInput;
        //Debug.Log(dirInput);

        return dirInput; //Doesnt retrun anything.
    }
}