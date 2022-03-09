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

    private string moveDirNode;

    // Start is called before the first frame update
    void Start()
    {
        moveDirNode = null;
        InputChecker.ConfrimDirInput += InputChecker_ConfrimDirInput;
        foreach (Node node in GameManager.Instance.Nodes)
        {
            if (node.Parents.Length > 2 && node.Children.Length == 0)
            {
                CurrentNode = node;
                CurrentNode.tag = "Player_Node";
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (moving == false)
        {
            if (moveDirNode != null)
            {
                //Debug.Log("HIT");
                MoveToNode(FindClosest());
                moveDirNode = null;
            }
        }
        else
        {
            moveDirNode = null;
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


    /// <summary>
    /// Sets the players target node and current directon to the specified node.
    /// </summary>
    /// <param name="node"></param>
    public void MoveToNode(Node node)
    {
        if (moving == false)
        {
            TargetNode = node;
            CurrentNode.tag = "Untagged";
            TargetNode.tag = "Player_Node";
            currentDir = TargetNode.transform.position - transform.position;
            currentDir = currentDir.normalized;
            moving = true;
        }
    }

    private Node FindClosest()
    {
        List<Node> listParentChild = new List<Node>();
        List<Node> listClose = new List<Node>();
        Dictionary<string, Node> whereNode = new Dictionary<string, Node>();
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 10f);
        Node whichNode;

        foreach (Node node in GameManager.Instance.Nodes)
        {
            listParentChild.Add(node);
        }

        foreach (Collider hitCol in hitColliders)
        {
            if (hitCol.transform.name.Contains("Node"))
            {
                foreach (Node node in listParentChild)
                {
                    if (node.name == hitCol.transform.name)
                    {
                        //Debug.Log(node.name + " and " + hitCol.transform.name);
                        listClose.Add(node);
                    }
                }
            }
        }

        foreach (Node node in listParentChild)
        {
            if (node == CurrentNode)
            {
                listClose.Remove(node);
            }
        }
        foreach (string x in new List<string> { "u", "d", "l", "r" })
        {
            whereNode[x] = CurrentNode;
        }

        foreach (string x in new List<string> { "u", "d", "l", "r" })
        {
            foreach (Node node in listClose)
            {
                //Debug.DrawRay(transform.position, transform.forward * 10f, Color.blue,3f);
                switch (x)
                {
                    case "u":
                        if(Physics.Raycast(transform.position, transform.forward * 10f, out RaycastHit otherU, 12f))
                        {
                            if(otherU.transform.name == node.name)
                            {
                                whereNode["u"] = node;
                            }
                        }
                        break;
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

        whichNode = whereNode[moveDirNode];

        return whichNode;
    }

    private string InputChecker_ConfrimDirInput(string dirInput)
    {
        moveDirNode = dirInput;
        //Debug.Log(dirInput);
        return dirInput;
    }
}