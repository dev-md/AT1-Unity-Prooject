using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputChecker : MonoBehaviour
{
    #region
    private static InputChecker _instance; // Getting the instance of the input checker
    private static InputChecker Instance;
    #endregion

    //
    public delegate string InputConfrim(string dirInput); //Input event along with a string
    public static event InputConfrim ConfrimDirInput; // confriming event

    public List<KeyCode> upKeys; // List of all of the possiable keys presses
    public List<KeyCode> downKeys;
    public List<KeyCode> leftKeys;
    public List<KeyCode> rightKeys;
    Dictionary<string, List<KeyCode>> listInputs = new Dictionary<string, List<KeyCode>>();
    //A dictinonary of the direction with the key

    public Button uButton; //The UI Buttons that will be used
    public Button dButton;
    public Button lButton;
    public Button rButton;

    private void Awake()
    {
        // A lazy way of adding every key to a direction
        listInputs.Add("u", upKeys); 
        listInputs.Add("d", downKeys);
        listInputs.Add("l", leftKeys);
        listInputs.Add("r", rightKeys);

        // A Super lazy way of checking to see if the Buttons are being pressed
        uButton.GetComponent<Button>().onClick.AddListener(UpButtonClick);
        dButton.GetComponent<Button>().onClick.AddListener(DownButtonClick);
        lButton.GetComponent<Button>().onClick.AddListener(LeftButtonClick);
        rButton.GetComponent<Button>().onClick.AddListener(RightButtonClick);

    }

    private void Update()
    {
        foreach(KeyValuePair<string, List<KeyCode>> x in listInputs) // getting the list into X
        {
            foreach(KeyCode y in x.Value) //getting the key presses into y
            {
                if (Input.GetKeyDown(y))
                {
                    if (ConfrimDirInput != null) ConfrimDirInput.Invoke(x.Key);
                    // Calls Event with the direction that is done by.
                }
            }
        }
    }

    //A super duper lazy way of creating a direction method,
    void UpButtonClick()
    {
        if (ConfrimDirInput != null) ConfrimDirInput.Invoke("u");
        // Calls Event with the direction that is stated.
    }

    //This is the same for the rest of the direction.
    void DownButtonClick()
    {
        if (ConfrimDirInput != null) ConfrimDirInput.Invoke("d");
    }
    void LeftButtonClick()
    {
        if (ConfrimDirInput != null) ConfrimDirInput.Invoke("l");
    }
    void RightButtonClick()
    {
        if (ConfrimDirInput != null) ConfrimDirInput.Invoke("r");
    }

}
