using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Tooltip("Movement speed modifier.")]
    [SerializeField] private float speed = 3;
    private Node currentNode;
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
                    currentNode = DFS();
                    currentDir = currentNode.transform.position - transform.position;
                    currentDir = currentDir.normalized;
                }
            }
            else
            {
                Debug.LogWarning($"{name} - No current node");
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


        for (int i = 0; i < currentNode.Children.Length; i++)
        {
            
        }
    }

    //Implement DFS algorithm method here
    public Node DFS()
    {
        Stack stack = new Stack();
        List<Node> visitedlist = new List<Node>();
        visitedlist.Add(currentNode);
        stack.Push(currentNode);

        while (stack.Count > 0)
        {
            Node node = (Node)stack.Pop();
            //visitedlist.Add(node);
            //Debug.Log("Checking " + node.name);
            foreach (Node child in node.Children)
            {
                if (visitedlist.Contains(child) == false) //&& stack.Contains(child) == false)
                {
                    //Debug.Log("Checking " + node.name + " and child " + child.name);
                    if (child.tag == "Player_Node")
                    {
                        //Debug.Log(child);
                        return child;
                    }
                    visitedlist.Add(child);
                    stack.Push(child);
                }
            }
        }

        return null;
    }
}