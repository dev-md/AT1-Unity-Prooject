using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//The Whole idea to make this more clean.
public class Widget : MonoBehaviour
{
    private Image[] icons;
    private float flashTimer = 0.1f;
    private Player player;
    private bool changeGreen;

    private List<GameObject> listChildren = new List<GameObject>();
    private int numChild;
    private List<string> dirList = new List<string>();
    private Dictionary<string, Button> listButtons = new Dictionary<string, Button>();

    private void Awake()
    {
        foreach (string x in new List<string> { "u", "d", "l", "r" })
        {
            dirList.Add(x);
        }

        numChild = this.transform.childCount;
        for (int i = 0; i < numChild; i++)
        {
            //this.transform.GetChild(i).gameObject.SetActive(false);
            //print(this.transform.GetChild(i).gameObject.name);
            listChildren.Add(this.transform.GetChild(i).gameObject);

        }

        for(int i = 0;i < numChild; i++)
        {
            listButtons.Add(dirList[i],listChildren[i].GetComponent<Button>());
        }
        

        FindObjectOfType<Enemy>().GameOverEvent += delegate 
        {
            gameObject.SetActive(false);
        };
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.Instance.Player;
        foreach(GameObject child in listChildren)
        {
            child.GetComponent<Image>().color = Color.grey;
        }

    }

    // Update is called once per frame
    void LateUpdate()
    {
        //The same from the player. note if it changes this needs to change aswell.
        if (player.publicmoving == false)
        {
            if (player.moveDirNode != null) //If it is a direction
            {
                //Changes the colour of the button and finds the node of the direction.

                if (changeGreen == true)
                {
                    ChangeButtonColour(listButtons[player.moveDirNode], Color.green);
                }
                else
                {
                    ChangeButtonColour(listButtons[player.moveDirNode], Color.red);
                }
            }
        }
        else if (player.publicmoving == true)
        {
            if (player.moveDirNode != null) //if does have a direction and is not moving
            {
                //if the colour is not green
                if (listButtons[player.moveDirNode].GetComponent<Image>().color != Color.green)
                {
                    //change it to red.
                    ChangeButtonColour(listButtons[player.moveDirNode], Color.red);
                }
            }
        }
        changeGreen = false;
    }

    public void ChangeGreen()
    {
        changeGreen = true;
    }
    
    private void ChangeButtonColour(Button button, Color colour)
    {
        button.GetComponent<Image>().color = colour;
        if (colour == Color.green) StartCoroutine(ColourTimer(button, 2.4f, colour));
        if (colour == Color.red) StartCoroutine(ColourTimer(button, flashTimer, colour));
    }

    //My Colour timer
    IEnumerator ColourTimer(Button button, float time, Color colour)
    {
        yield return new WaitForSeconds(time);
        if (colour == Color.red)
        {
            if (button.GetComponent<Image>().color == Color.green)
            {
                button.GetComponent<Image>().color = Color.green;
            }
        }
        button.GetComponent<Image>().color = Color.grey;
    }
}