using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Tooltip("Movement speed modifier.")]
    [SerializeField] private float speed = 3;
    private Node currentNode;
    private Node targetNode;
    private Vector3 currentDir;
    private bool playerCaught = false;

    public delegate void GameEndDelegate();
    public event GameEndDelegate GameOverEvent = delegate { };

    // Start is called before the first frame update
    void Start()
    {
        InitializeAgent();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCaught == false)
        {
            if (currentNode != null)
            {
                //If within 0.25 units of the current node.
                if (Vector3.Distance(transform.position, currentNode.transform.position) > 0.25f)
                {
                    transform.Translate(currentDir * speed * Time.deltaTime);
                }
                //Implement path finding here
                else
                {
                    //currentNode = GameManager.Instance.Nodes[5];

                    targetNode = DFS(); // My moving method

                    //Main movement
                    if((targetNode != currentNode) && (targetNode != null))
                    {
                        currentNode = targetNode;
                        currentDir = currentNode.transform.position - transform.position;
                        currentDir = currentDir.normalized;
                    }
                    else if((GameManager.Instance.Player.TargetNode != null) 
                    && 
                    (GameManager.Instance.Player.TargetNode 
                    != GameManager.Instance.Player.CurrentNode))
                    {
                        currentNode = GameManager.Instance.Player.TargetNode;
                        currentDir = currentNode.transform.position - transform.position;
                        currentDir = currentDir.normalized;
                    }
                }
            }
            else
            {
                Debug.LogWarning($"{name} - No current node");

                //Trying to assgin a currunt node.
                targetNode = DFS();
                currentNode = targetNode;
            }

            Debug.DrawRay(transform.position, currentDir, Color.cyan);
        }
    }

    //Called when a collider enters this object's trigger collider.
    //Player or enemy must have rigidbody for this to function correctly.
    private void OnTriggerEnter(Collider other)
    {
        if (playerCaught == false)
        {
            if (other.tag == "Player")
            {
                playerCaught = true;
                GameOverEvent.Invoke(); //invoke the game over event
            }
        }
    }

    /// <summary>
    /// Sets the current node to the first in the Game Managers node list.
    /// Sets the current movement direction to the direction of the current node.
    /// </summary>
    void InitializeAgent()
    {
        currentNode = GameManager.Instance.Nodes[0];
        currentDir = currentNode.transform.position - transform.position;
        currentDir = currentDir.normalized;
    }

    //Implement DFS algorithm method here
    private Node DFS() // My DFS algrothim.
    {
        //creating stack and list
        Stack stack = new Stack();
        List<Node> visitedlist = new List<Node>();

        // Adding the root node to the stack and list
        visitedlist.Add(GameManager.Instance.Nodes[0]);

        //Just checking to see if they are on the root node.
        if (GameManager.Instance.Nodes[0] == GameManager.Instance.Player.CurrentNode)
        {
            return visitedlist[0];
        }

        stack.Push(GameManager.Instance.Nodes[0]);
        //loop while there is something in the stack
        while (stack.Count > 0) 
        {
            Node node = (Node)stack.Pop();
            visitedlist.Add(node); // mark the node into the visted list.
            List<Node> stacklist = new List<Node>();
            //Debug.Log("Main Checking " + node.name);
            foreach (Node child in node.Children)
            {
                if (visitedlist.Contains(child) == false) //&& stack.Contains(child) == false)
                {
                    //Debug.Log("Checking " + node.name + " and child " + child.name);
                    
                    if (child == GameManager.Instance.Player.CurrentNode) //Checking the node with player curr.
                    {
                        //Debug.Log(child);
                        return child; // This is where the player is.
                    }

                    //If they didnt find the player node, adds them to the visted list.
                    visitedlist.Add(child);
                    stacklist.Add(child);
                    //stack.Push(child);
                }
            }

            //We want the Reverse of the list order.
            stacklist.Reverse();
            foreach (Node child in stacklist)
            {
                stack.Push(child);
            }

        }

        return null; // couldnt find any player.
    }
}